using Microsoft.AspNetCore.Cors;
using MinimalAPIMovies.Entities;

var builder = WebApplication.CreateBuilder(args);
var origins = builder.Configuration.GetValue<string>("origins")!;

//Start services area

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("Allow", configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//End services area

var app = builder.Build();

//Start middleware area

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseOutputCache();

app.MapGet("/", [EnableCors(policyName: "Allow")] () => "Hello World!");

app.MapGet("/genders", () => {
    
    var genders = new List<Gender>
    {   
        new Gender
        {
            Id = 1,
            Name = "Drama"
        },
        new Gender
        {
            Id = 2,
            Name = "Accion"
        },
        new Gender
        {
            Id = 3,
            Name = "Comedia"
        }
    };

    return genders;
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

//End middleware area


app.Run();
