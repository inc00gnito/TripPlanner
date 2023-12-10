using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Logic;
using api.Models;
using api.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("TripPlanner")));

builder.Services.AddCors(p => p.AddPolicy("MyPolicy", build =>
{
    build.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
}));

builder.Services
    .AddScoped<IAuthorization, Authorization>()
    .AddScoped<IAccount, AccountData>()
    .AddScoped<IPlaces, Places>();

var app = builder.Build();

// Configure the HTTP request pipeline.


if( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("MyPolicy");
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
