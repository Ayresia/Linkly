using Linkly.Api.Interfaces;
using Linkly.Api.Models;
using Linkly.Api.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisURL = Environment.GetEnvironmentVariable("REDIS_URL");
var redisUsername = Environment.GetEnvironmentVariable("REDIS_USERNAME");
var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
var redisOptions = ConfigurationOptions.Parse(redisURL ?? "192.168.0.31");

if (redisUsername != null && redisPassword != null)
{
    redisOptions.User = redisUsername;
    redisOptions.Password = redisPassword;
}

var multiplexer = ConnectionMultiplexer.Connect(redisOptions);
WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<LinkContext>();
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }
        );
    }
);
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo 
            {
                Title = "Linkly API",
                Version = "v1"
            }
        );
    }
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var linkContext = services.GetService<LinkContext>();

    if (linkContext != null)
        linkContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }
);

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
