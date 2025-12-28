using Kitapix.Application;
using Kitapix.Infrastructure;
using Kitapix.WebAPI;
using Kitapix.WebAPI.Middlewares.Exceptionhandler;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationService();
builder.Services.AddInfrastructorService(builder.Configuration);
builder.Services.AddWebApiService();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        return Task.CompletedTask;
    });
});
var app = builder.Build();

app.UseCors("AllowAll");

app.MapOpenApi("/openapi/v1.json");
app.UseApplicationMiddlewares();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapScalarApiReference();

app.MapControllers();

app.Run();