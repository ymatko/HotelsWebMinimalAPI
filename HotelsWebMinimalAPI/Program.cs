using HotelsWebMinimalAPI.Data;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HotelDb>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlLite"));
});

builder.Services.AddScoped<IHotelRepository, HotelRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<HotelDb>();
    db.Database.EnsureCreated();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/hotels", async (IHotelRepository repository) =>
    Results.Ok(await repository.GetHotelsAsync()))
    .Produces<List<Hotel>>(StatusCodes.Status200OK)
    .WithName("GetAllHotels")
    .WithTags("Getters");
app.MapGet("/hotels/{id}", async (IHotelRepository repository, int id) => 
    await repository.GetHotelAsync(id) is Hotel hotel? Results.Ok(hotel): Results.NotFound())
    .Produces<Hotel>(StatusCodes.Status200OK)
    .WithName("GetHotel")
    .WithTags("Getters"); ;
app.MapPost("/hotels", async ([FromServices] IHotelRepository repository, [FromBody] Hotel hotel, HttpResponse response) =>
    {
        await repository.InsertHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.Created($"/hotels/{hotel.Id}", hotel);
    })
    .Accepts<Hotel>("application/json")
    .Produces<Hotel>(StatusCodes.Status201Created)
    .WithName("CreateHotel")
    .WithTags("Creators");
app.MapPut("/hotels", async([FromServices] IHotelRepository repository, [FromBody] Hotel hotel) =>
    {
        await repository.UpdateHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.NoContent();
    })
    .Accepts<Hotel>("application/json")
    .WithName("UpdateHotel")
    .WithTags("Updaters"); ;
app.MapDelete("/hotels/{id}", async (IHotelRepository repository, int id) =>
    {
    await repository.DeleteHotelAsync(id);
    await repository.SaveAsync();
    return Results.NoContent();
    })
    .WithName("DeleteHotel")
    .WithTags("Deleters");

app.UseHttpsRedirection();

app.Run();
