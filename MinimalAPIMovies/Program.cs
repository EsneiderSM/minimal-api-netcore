using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIMovies.Entities;
using MinimalAPIMovies.Repositories;

var builder = WebApplication.CreateBuilder(args);
var origins = builder.Configuration.GetValue<string>("origins")!;

//Start services area

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

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

builder.Services.AddScoped<IRepositoryGender, RepositoryGender>();

//End services area

var app = builder.Build();

//Start middleware area

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseOutputCache();

app.MapGet("/", [EnableCors(policyName: "Allow")] () => "Hello World!");

app.MapGet("/genders", async (IRepositoryGender repository) => 
{
    List<Gender> genders = await repository.GetAll();
    return Results.Ok(genders);

}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genders-get"));

app.MapGet("/genders/{id:int}", async (int id, IRepositoryGender repository) =>
{
    var gender = await repository.GetGenderById(id);

    if(gender is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(gender);
});

app.MapPost("/gender", async (Gender gender, IRepositoryGender repository, IOutputCacheStore outputCacheStore) =>
{
    var id = await repository.Create(gender);
    await outputCacheStore.EvictByTagAsync("genders-get", default);
    return Results.Created($"/genders/{id}", gender);
}); 

app.MapPut("/genders/{id:int}", async (int id, Gender gender, IRepositoryGender repository, IOutputCacheStore outputCacheStore) =>
{
    var exist = await repository.Exists(id);

    if(!exist)
    {
        return Results.NotFound();
    }
    
    await repository.Update(gender);
    await outputCacheStore.EvictByTagAsync("genders-get", default);
    return Results.NoContent();
});

app.MapDelete("/genders/{id:int}", async (int id, IRepositoryGender repository, IOutputCacheStore outputCacheStore) =>
{
    var exist = await repository.Exists(id);

    if (!exist)
    {
        return Results.NotFound();
    }

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("genders-get", default);
    return Results.NoContent();
});

//End middleware area


app.Run();
