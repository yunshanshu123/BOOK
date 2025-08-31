
using backend.Common.MiddleWare;
using backend.Repositories.Admin;
using backend.Repositories.BorrowRecordRepository;
using backend.Repositories.ReaderRepository;
using backend.Services.Admin;
using backend.Services.BorrowingService;
using backend.Services.ReaderService;
using backend.Services.Web;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// 输出当前环境
Console.WriteLine($"当前运行环境: {builder.Environment.EnvironmentName}");

// 添加控制器服务
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// 添加 Swagger 支持
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 日志配置
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// 添加服务
var services = builder.Services;

// 添加 CORS 支持（便于前端访问）
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()      // 生产环境可替换为具体域名
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 添加 Swagger 服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "图书馆管理系统 API",
        Version = "v1",
        Description = "基于 ASP.NET Core 的图书馆后台接口文档"
    });
});

// 注册 IHttpContextAccessor
services.AddHttpContextAccessor();

// 读取连接字符串（根据环境自动读取 appsettings.Development.json 或 appsettings.Production.json）
var connectionString = builder.Configuration.GetConnectionString("OracleDB")
                      ?? throw new InvalidOperationException("缺少 OracleDB 连接字符串配置");

var redisConnStr = builder.Configuration.GetConnectionString("Redis")
                  ?? throw new InvalidOperationException("缺少 Redis 连接字符串配置");

// 注册 Redis
services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrEmpty(configuration))
    {
        throw new ArgumentException("未配置 Redis 连接字符串");
    }
    return ConnectionMultiplexer.Connect(configuration);
});
services.AddSingleton<RedisService>();

// 注册业务服务
services.AddScoped<TokenService>();
services.AddScoped<SecurityService>();
services.AddScoped<LoginService>();

// 注册 ReaderRepository 和 ReaderService
services.AddSingleton(new ReaderRepository(connectionString));
services.AddTransient<ReaderService>();

//注册 BorrowingService 和 BorrowingRepository
services.AddSingleton(new BorrowRecordRepository(connectionString));
services.AddTransient<BorrowingService>();

// 注册服务依赖（Repository 使用 Singleton，Service 使用 Transient）
builder.Services.AddSingleton(new BookRepository(connectionString));
builder.Services.AddSingleton(new CommentRepository(connectionString));
builder.Services.AddSingleton(new BookCategoryTreeOperation(connectionString));
builder.Services.AddSingleton(new LogService(connectionString));
builder.Services.AddSingleton(new BookShelfRepository(connectionString));
builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<CommentService>();
builder.Services.AddTransient<BookCategoryService>();
builder.Services.AddTransient<BookShelfService>();

// 注册管理员服务
builder.Services.AddSingleton(new PurchaseAnalysisRepository(connectionString));
builder.Services.AddTransient<PurchaseAnalysisService>();
builder.Services.AddSingleton(new ReportRepository(connectionString));
builder.Services.AddTransient<ReportService>();
builder.Services.AddSingleton(new AnnouncementRepository(connectionString));
builder.Services.AddTransient<AnnouncementService>();

var app = builder.Build();

// 启用 Swagger（开发环境）
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "图书馆 API v1");
        c.RoutePrefix = "api/docs";
    });
}

// 获取 logger
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("StartupLogger");
logger.LogInformation($"当前运行环境: {app.Environment.EnvironmentName}");

app.UseStaticFiles(); // 启用 wwwroot 目录下的静态文件

app.UseCors(); // 启用跨域

app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>(); // 自定义异常中间件

app.UseMiddleware<JwtAuthenticationMiddleware>(); // JWT 认证中间件

app.UseAuthorization(); // 授权中间件

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// 启动应用
app.Run();
