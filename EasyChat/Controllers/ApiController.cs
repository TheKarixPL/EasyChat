using EasyChat.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyChat.Controllers;

public class ApiController : Controller
{
    private readonly ILogger _logger;
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public ApiController(ILogger<ApiController> logger, IDatabaseConnectionFactory connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
    }
}