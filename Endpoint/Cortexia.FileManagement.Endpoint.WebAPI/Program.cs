using Cortexia.FileManagement.Endpoint.WebAPI.Extensions.DependencyInjections;
using FluentValidation;
using System.Reflection;
using Zamin.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddZaminParrotTranslator(builder.Configuration, "ParrotTranslator");
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddWebAPIFileManagement(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();
