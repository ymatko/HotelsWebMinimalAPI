using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HotelDb>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlLite"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<HotelDb>();
    db.Database.EnsureCreated();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}


app.MapGet("/hotels", async (HotelDb db) => await db.Hotels.ToListAsync());
app.MapGet("/hotels/{id}", async (HotelDb db, int id) => 
    await db.Hotels.FirstOrDefaultAsync(h => h.Id == id) is Hotel hotel? Results.Ok(hotel): Results.NotFound());
app.MapPost("/hotels", async ([FromServices] HotelDb db, [FromBody] Hotel hotel, HttpResponse response) =>
    {
        db.Hotels.Add(hotel);
        await db.SaveChangesAsync();
        response.StatusCode = 201;
        response.Headers.Location = $"/hotels/{hotel.Id}";
    });
app.MapPut("/hotels", async([FromServices] HotelDb db, [FromBody] Hotel hotel) =>
{
    var hotelFromDb = await db.Hotels.FindAsync(new object[] { hotel.Id });
    if (hotelFromDb == null) return Results.NotFound();
    hotelFromDb.Latitude = hotel.Latitude;
    hotelFromDb.Longitude = hotel.Longitude;
    hotelFromDb.Name = hotel.Name;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/hotels/{id}", async (HotelDb db, int id) =>
{
    var hotelFromDb = await db.Hotels.FindAsync(new object[] { id });
    if (hotelFromDb == null) return Results.NotFound();
    db.Hotels.Remove(hotelFromDb);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.Run();

public class HotelDb : DbContext
{
    public HotelDb(DbContextOptions<HotelDb> options) : base(options)
    {
        
    }
    public DbSet<Hotel> Hotels => Set<Hotel>();
}


public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}