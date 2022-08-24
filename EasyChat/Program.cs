using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.InteropServices;
using Dapper;
using EasyChat.Interfaces;
using EasyChat.Models;
using EasyChat.Services;
using NLog;
using NLog.Web;
using Npgsql;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try
{
    logger.Info("EasyChat by TheKarixPL");
    foreach (var prop in typeof(RuntimeInformation).GetProperties(BindingFlags.Public | BindingFlags.Static))
        logger.Info($"{prop.Name}: {prop.GetValue(null)}");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.Logging
        .ClearProviders()
        .SetMinimumLevel(LogLevel.Debug);
    builder.Host.UseNLog();
    
    if (builder.Environment.IsDevelopment())
        builder.Configuration.AddJsonFile("secret.Development.json");
    else
        builder.Configuration.AddJsonFile("secret.json");
    builder.Services.AddOptions<DatabaseOptions>()
        .Configure<IConfiguration>((options, configuration) => configuration.Bind(DatabaseOptions.SectionName, options))
        .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString),
            $"{DatabaseOptions.SectionName}:{nameof(DatabaseOptions.ConnectionString)} cannot be empty");
    
    logger.Info("Starting database test");
    try
    {
        using var conn = new NpgsqlConnection(builder.Configuration["Database:ConnectionString"]);
        conn.Open();
        var info = conn.QuerySingle(@"select version() as version;");
        logger.Info($"Connected to \"{info.version}\" as {conn.UserName}@{conn.Host}:{conn.Port}");

        var tables = new string[]
        {
            "users",
            "login_history",
            "messages",
            "attachments",
            "messages_has_attachments"
        };
        foreach (var table in tables)
        {
            var sql = $"select count(*) as c from {table}";
            logger.Trace($"Table \"{table}\" has {conn.QuerySingle(sql).c} records");
        }
        
        logger.Info("Database test successfull");
    }
    catch (Exception e)
    {
        logger.Fatal(e, "Database test failed");
        Environment.Exit(-1);
    }

    var databaseModels = new Type[]
    {
        typeof(AttachmentModel),
        typeof(LoginHistoryModel),
        typeof(MessageModel),
        typeof(UserModel)
    };
    foreach (var model in databaseModels)
        SqlMapper.SetTypeMap(
            model,
            new CustomPropertyTypeMap(
                model,
                (type, columnName) =>
                    type.GetProperties().FirstOrDefault(prop =>
                        prop.GetCustomAttributes(false)
                            .OfType<ColumnAttribute>()
                            .Any(attr => attr.Name == columnName))));

    builder.Services.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();

    builder.Services.AddMvcCore().AddRazorViewEngine();
    
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseStaticFiles();

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute("default", "/{controller=Home}/{action=Index}");
    });

    app.Run();
}
catch (Exception e)
{
    logger.Fatal(e);
    Environment.Exit(-1);
}