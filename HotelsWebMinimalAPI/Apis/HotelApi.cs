using HotelsWebMinimalAPI.Auth;
using HotelsWebMinimalAPI.Data;

namespace HotelsWebMinimalAPI.Apis
{
    public class HotelApi
    {
        public void Register(WebApplication app)
        {
            app.MapGet("/hotels", [Authorize] async (IHotelRepository repository) =>
                Results.Extensions.Xml(await repository.GetHotelsAsync()))
                .Produces<List<Hotel>>(StatusCodes.Status200OK)
                .WithName("GetAllHotels")
                .WithTags("Getters");

            app.MapGet("/hotels/{id}", async (IHotelRepository repository, int id) =>
                await repository.GetHotelAsync(id) is Hotel hotel ? Results.Ok(hotel) : Results.NotFound())
                .Produces<Hotel>(StatusCodes.Status200OK)
                .WithName("GetHotel")
                .WithTags("Getters");

            app.MapPost("/hotels", [Authorize] async ([FromServices] IHotelRepository repository, [FromBody] Hotel hotel, HttpResponse response) =>
            {
                await repository.InsertHotelAsync(hotel);
                await repository.SaveAsync();
                return Results.Created($"/hotels/{hotel.Id}", hotel);
            })
                .Accepts<Hotel>("application/json")
                .Produces<Hotel>(StatusCodes.Status201Created)
                .WithName("CreateHotel")
                .WithTags("Creators");

            app.MapPut("/hotels", [Authorize] async ([FromServices] IHotelRepository repository, [FromBody] Hotel hotel) =>
            {
                await repository.UpdateHotelAsync(hotel);
                await repository.SaveAsync();
                return Results.NoContent();
            })
                .Accepts<Hotel>("application/json")
                .WithName("UpdateHotel")
                .WithTags("Updaters");

            app.MapDelete("/hotels/{id}", [Authorize] async (IHotelRepository repository, int id) =>
            {
                await repository.DeleteHotelAsync(id);
                await repository.SaveAsync();
                return Results.NoContent();
            })
                .WithName("DeleteHotel")
                .WithTags("Deleters");

            app.MapGet("/hotels/search/name/{query}", [Authorize] async (string query, IHotelRepository repository) =>
                await repository.GetHotelsAsync(query) is IEnumerable<Hotel> hotels
                    ? Results.Ok(hotels)
                    : Results.NotFound(Array.Empty<Hotel>()))
                .Produces<List<Hotel>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("SearchHotels")
                .WithTags("Getters")
                .ExcludeFromDescription();

            app.MapGet("/hotels/search/location/{coordinate}", [Authorize] async (IHotelRepository repository, Coordinate coordinate) =>
                await repository.GetHotelsAsync(coordinate) is IEnumerable<Hotel> hotels
                    ? Results.Ok(hotels)
                    : Results.NotFound(Array.Empty<Hotel>()))
                .Produces<List<Hotel>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("SearchHotelsUseCoordinates")
                .WithTags("Getters")
                .ExcludeFromDescription();

        }
    }
}
