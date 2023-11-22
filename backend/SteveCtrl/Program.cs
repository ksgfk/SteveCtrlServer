using Microsoft.AspNetCore.Authentication.Cookies;
using SteveCtrl.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); //控制器DI
//使用cookie作为身份证明
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });
builder.Services.AddSingleton<IUserRepository, SimpleUserRepository>();
builder.Logging.AddConsole(); //控制台打印日志

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication(); //身份验证
app.UseAuthorization(); //授权

app.MapControllers(); //控制器路由

app.Run();
