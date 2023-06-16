using BusinessObject;
using DataAccess;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using EBookStoreWebAPI.Helpers;
using EBookStoreWebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);
static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Author>("Authors");
    builder.EntitySet<Book>("Books");
    builder.EntitySet<Publisher>("Publishers");
    builder.EntitySet<Role>("Roles");
    builder.EntitySet<User>("Users");

    return builder.GetEdmModel();
}
// Add services to the container.
string? connectionString = builder.Configuration.GetConnectionString("EBookStoreDB");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString!);
});
builder.Services.AddTransient<ApplicationDbContext>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddControllers();
builder.Services.AddControllers().AddOData(options =>
            options.Select().Filter().Count().OrderBy().Expand().SetMaxTop(100)
                .AddRouteComponents("Odata", GetEdmModel()));

builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EBookStoreWebAPI", Version = "v1" });
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<DefaultAccount>(builder.Configuration.GetSection("DefaultAccount"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EBookStoreWebAPI v1"));
}

app.UseODataBatching();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x
    => x.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
