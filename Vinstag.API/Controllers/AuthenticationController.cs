using Microsoft.AspNetCore.Mvc;
using Vinstag.API.Services;

namespace Vinstag.API.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthenticationController: ControllerBase
{
    private readonly AuthenticationService authenticationService;

    public AuthenticationController(AuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    [HttpPost("authenticate")]
    public IActionResult SignIn([FromBody] string authKey)
    {
        var id = authenticationService.SignIn(authKey);

        return !string.IsNullOrEmpty(id)
            ? Ok($"User with id: {id} is authenticate")
            : BadRequest("You could not be authorized");
    }


}