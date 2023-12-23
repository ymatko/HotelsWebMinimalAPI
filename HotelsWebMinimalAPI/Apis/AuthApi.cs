using HotelsWebMinimalAPI.Auth;

namespace HotelsWebMinimalAPI.Apis
{
    public class AuthApi
    {
        public void Register(WebApplication app)
        {
            app.MapGet("/login", [AllowAnonymous] async (HttpContext context, ITokenService tokenService, IUserRepository userRepository) =>
            {
                UserModel userModel = new()
                {
                    UserName = context.Request.Query["username"],
                    Password = context.Request.Query["password"],
                };
                var userDto = userRepository.GetUser(userModel);
                if (userDto == null) return Results.Unauthorized();
                var token = tokenService.BuildToken(app.Configuration["Jwt:Key"],
                    app.Configuration["Jwt:Issuer"], userDto);
                return Results.Ok(token);
            });
        }
    }
}
