using Linkly.Api.Interfaces;
using Linkly.Api.Models;
using Linkly.Api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<LinkContext>();
builder.Services.AddScoped<ILinkService, LinkService>();
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

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
