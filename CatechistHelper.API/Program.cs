using CatechistHelper.API.Configurations;
using CatechistHelper.API.Middlewares;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Rewrite;


try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpContextAccessor();
    // Config builder
    builder.ConfigureAutofacContainer();

    // Allow access
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    // Add Configuration
    builder.Configuration.SettingsBinding();

    builder.Services.AddSwaggerGenOption();
    builder.Services.AddDbContext();
    builder.Services.AddMvc()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    ConfigFirebase.ConfigureFirebase();

    builder.Services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
    });

    var app = builder.Build();

    var options = new RewriteOptions()
    .AddRedirect("^$", "/swagger/index.html");
    app.UseRewriter(options);

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });

    app.UseCors("AllowAll");

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
    await app.RunAsync();

}
catch (Exception ex)
{
}
