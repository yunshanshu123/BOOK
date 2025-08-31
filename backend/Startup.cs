// using backend.Common.MiddleWare;
// using backend.Repositories.ReaderRepository;
// using backend.Services.ReaderService;
// using backend.Services.Web;
// using StackExchange.Redis;

// namespace backend
// {
//     public class Startup
//     {
//         private readonly IConfiguration _configuration;

//         public Startup(IConfiguration configuration)
//         {
//             _configuration = configuration;
//         }

//         // 注册服务
//         public void ConfigureServices(IServiceCollection services)
//         {
//             services.AddControllers();

//             // 添加 CORS 支持
//             services.AddCors(options =>
//             {
//                 options.AddDefaultPolicy(policy =>
//                 {
//                     policy
//                         .AllowAnyOrigin()
//                         .AllowAnyHeader()
//                         .AllowAnyMethod();
//                 });
//             });


//             // 添加 Swagger 服务
//             services.AddEndpointsApiExplorer();
//             services.AddSwaggerGen(options =>
//             {
//                 options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//                 {
//                     Title = "图书馆管理系统 API",
//                     Version = "v1",
//                     Description = "基于 ASP.NET Core 的图书馆后台接口文档"
//                 });
//             });

//             // 注册依赖
//             // 1. 注册 IHttpContextAccessor
//             services.AddHttpContextAccessor();

//             // 2. 注册 Redis（使用 StackExchange.Redis）
//             services.AddSingleton<IConnectionMultiplexer>(sp =>
//             {
//                 var configuration = _configuration.GetConnectionString("Redis");
//                 if (string.IsNullOrEmpty(configuration))
//                 {
//                     throw new ArgumentException("未配置 Redis 连接字符串");
//                 }
//                 return ConnectionMultiplexer.Connect(configuration);
//             });
//             services.AddSingleton<RedisService>();

//             // 3. 注册 TokenService
//             services.AddScoped<TokenService>();

//             //  注册 SecurityService
//             services.AddScoped<SecurityService>();

//             // 4. 注册 LoginService
//             services.AddScoped<LoginService>();

//             // 5. 注册 ReaderRepository 和 ReaderService
//             services.AddSingleton(ocr => {
//                 // 读取连接字符串
//                 var connectionString = _configuration.GetConnectionString("OracleDB");
//                 if (string.IsNullOrEmpty(connectionString))
//                 {
//                     throw new ArgumentException("未配置 Oracle 数据库连接字符串");
//                 }
//                 return new ReaderRepository(connectionString);
//             });
//             services.AddTransient<ReaderService>();
//         }

//         // 配置中间件
//         public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
//         {
//             logger.LogInformation($"当前运行环境: {env.EnvironmentName}");

//             // 开发环境启用 Swagger
//             if (env.IsDevelopment())
//             {
//                 app.UseSwagger();
//                 app.UseSwaggerUI(c =>
//                 {
//                     c.SwaggerEndpoint("/swagger/v1/swagger.json", "图书馆 API v1");
//                     c.RoutePrefix = "api/docs"; // Swagger UI 设为根路径
//                 });
//             }


//             app.UseCors(); // 必须在 MapControllers 前调用

//             app.UseRouting();

//             // 使用jwt认证中间件
//             app.UseMiddleware<JwtAuthenticationMiddleware>();

//             app.UseAuthorization(); // 如果有授权中间件

//             app.UseEndpoints(endpoints =>
//             {
//                 endpoints.MapControllers();
//             });
//         }
//     }
// }
