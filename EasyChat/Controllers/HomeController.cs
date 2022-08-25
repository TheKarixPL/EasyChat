using Dapper;
using EasyChat.Extensions;
using EasyChat.Interfaces;
using EasyChat.Models;
using EasyChat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyChat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger _logger;
    private readonly IDatabaseConnectionFactory _connectionFactory;

    private ISession Session => HttpContext.Session;

    public HomeController(ILogger<HomeController> logger, IDatabaseConnectionFactory connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (Session.GetInt64("UID") != null)
            return View();
        else
            return RedirectToAction("Login");
    }

    [HttpGet]
    [Route("Home/Login")]
    [Route("Home/Login/{error?}")]
    public IActionResult Login(string error)
    {
        if (Session.GetInt64("UID") == null)
            return View((object) error);
        else
            return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("Home/Login")]
    public async Task<IActionResult> DoLogin([FromBody] LoginViewModel model)
    {
        if (Session.GetInt64("UID") != null)
            return RedirectToAction("Index");
        if (!ModelState.IsValid)
            return RedirectToAction("Login", new { error = "Informations are invalid"});
        
        await using var conn = await _connectionFactory.CreateConnectionAsync();
        var user = await conn.QuerySingleAsync<UserModel>(@"select * from users where name = @name;",
            new { name = model.Name });
        if (user.Password.EqualsRawPassword(model.Password))
        {
            await conn.ExecuteAsync("insert into login_history(ip, time, users_id) values (@ip, @time, @id);",
                new
            {
                ip = HttpContext.Connection.RemoteIpAddress,
                time = DateTime.Now,
                id = user.Id
            });
            Session.SetInt64("UID", (long)user.Id);
            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Login", new { error = "Wrong password" });
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Session.Remove("UID");
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("Home/Register/{error?}")]
    public IActionResult Register(string error)
    {
        if (Session.GetInt64("UID") == null)
            return View((object)error);
        else
            return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("Home/Register")]
    public async Task<IActionResult> DoRegister([FromBody] RegisterViewModel model)
    {
        if (Session.GetInt64("UID") != null)
            return RedirectToAction("Index");
        if (!ModelState.IsValid)
            return RedirectToAction("Register", new { error = "Informations are invalid" });

        await using var conn = await _connectionFactory.CreateConnectionAsync();
        if (await conn.QuerySingleAsync<bool>(@"select count(*) > 0 from users where name = @name;",
                new { name = model.Name }))
            return RedirectToAction("Register", new { error = "User already exists" });
        var id = await conn.QuerySingleAsync<ulong>(
            @"insert into users(name, email_address, account_creation_time, password) values (@name, @email_address, @account_creation_time, @password) returning id;",
            new
            {
                name = model.Name,
                email_address = model.EmailAddress,
                account_creation_time = DateTime.Now,
                password = new Password(model.Password).ToString()
            });
        await conn.ExecuteAsync(@"insert into login_history(ip, time, users_id) values (@ip, @time, @id);",
            new
            {
                ip = HttpContext.Connection.RemoteIpAddress,
                time = DateTime.Now,
                id
            });
        Session.SetInt64("UID", (long)id);
        return RedirectToAction("Index");
    }
}