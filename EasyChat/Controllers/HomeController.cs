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
    public IActionResult Login()
    {
        if (Session.GetInt64("UID") == null)
            return View();
        else
            return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DoLogin([FromForm] LoginViewModel model)
    {
        if (Session.GetInt64("UID") != null)
            return RedirectToAction("Index");
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Informations are invalid";
            return View("Login");
        }

        await using var conn = await _connectionFactory.CreateConnectionAsync();
        var users = (await conn.QueryAsync<UserModel>(@"select * from users where name = @name;",
            new { name = model.Name })).ToArray();
        if (users.Length == 0)
        {
            ViewBag.Error = $"User \"{model.Name}\" does not exist";
            return View("Login");
        }

        var user = users[0];
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
            ViewBag.Error = "Wrong password";
            return View("Login");
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Session.Remove("UID");
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (Session.GetInt64("UID") == null)
            return View();
        else
            return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DoRegister([FromForm] RegisterViewModel model)
    {
        if (Session.GetInt64("UID") != null)
            return RedirectToAction("Index");
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Informations are invalid";
            return View("Register");
        }

        await using var conn = await _connectionFactory.CreateConnectionAsync();
        if (await conn.QuerySingleAsync<bool>(@"select count(*) > 0 from users where name = @name;",
                new { name = model.Name }))
        {
            ViewBag.Error = "User already exists";
            return View("Register");
        }

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