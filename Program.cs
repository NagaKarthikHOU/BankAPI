using BankAPI;
using BankAPI.Endpoints;
using BankAPI.Repository;
using BankAPI.Services;
using BankAPI.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Builder;
using System.Data.SqlClient;
using BankAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<BankRepository>();
builder.Services.AddSingleton<IBankService,BankService>();

builder.Services.AddSingleton<StaffRepository>();
builder.Services.AddSingleton<IStaffService, StaffService>();

builder.Services.AddSingleton<AccountRepository>();
builder.Services.AddSingleton<IAccountService,AccountService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AuthenticationMiddleware>();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapBankEndpoints();

app.MapStaffEndpoints();

app.MapAccountEndpoints();
app.Run();


