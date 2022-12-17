using CardGame.Core.Context;
using CardGame.Core.Domain;
using CardGame.Core.Extensions;
using CardGame.Core.LogSystem;
using CardGame.Core.Repositories;
using CardGame.Core.Services;
using CardGame.Core.Services.Implementations;
using CardGame.Core.UnitOfWork;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Logs.ConfigureLogging();
builder.Host.UseSerilog();
builder.Services.AddDbContext<CardGameContext>(b =>
{
    b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        options => { options.CommandTimeout(120); });
});

builder.Services.AddControllerExtension();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork<CardGameContext>>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddScoped<IGameRepository, GameRepository>();

builder.Services.AddScoped<ICreateGameService, CreateGameService>();
builder.Services.AddScoped<IJoinGameService, JoinGameService>();
builder.Services.AddScoped<IGuessCardService, GuessCardService>();
builder.Services.AddScoped<ILeftGameService, LeftGameService>();
builder.Services.AddScoped<IGetGameListService, GetGameListService>();
builder.Services.AddScoped<IStartGameService, StartGameService>();
builder.Services.AddScoped<IConfirmationCardService, ConfirmationCardService>();

builder.Services.AddCustomSwagger();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.InitializeBasketDatabase().Wait();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCustomSwagger();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();