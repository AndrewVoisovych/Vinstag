using Microsoft.AspNetCore.Mvc;
using Vinstag.API.Services;
using Vinstag.Common.Models.Enums;
using Vinstag.InstagramAPI.Models;

namespace Vinstag.API.Controllers;

[ApiController]
[Route("[controller]")]
public class InstaController : ControllerBase
{
    private readonly ConnectionService _connectionService;
    private readonly UserService _userService;

    public InstaController(
        ConnectionService connectionService,
        UserService userService)
    {
        _connectionService = connectionService;
        _userService = userService;
    }


    [HttpGet("/TestApi")]
    public async Task<IActionResult> TestApi()
    {
        return Ok("VOIS SUCCESS");
    }

    [HttpGet("/GetFollowersDiff")]
    public async Task<IActionResult> GetFollowersDiff()
    {
        var id = await _userService.GetIdByUsername("");
        var usersLogs = await _connectionService.GetDifferents(id, ConnectionsTypes.Followers);

        return Ok(usersLogs);
    }

    [HttpGet("/GetFollowingDiff")]
    public async Task<IActionResult> GetFollowingDiff()
    {
        var id = await _userService.GetIdByUsername("");
        var usersLogs = await _connectionService.GetDifferents(id, ConnectionsTypes.Following);

        return Ok(usersLogs);
    }


    [HttpGet("/GetFirstFollowing")]
    public async Task<IActionResult> GetFirstFollowing()
    {
        var id = await _userService.GetIdByUsername("");
        InstaUsers? result = await _connectionService.GetFollowing(id);
        if (result?.Users == null)
        {
            return NoContent();
        }
        
        return Ok(result.Users.Select(x => x.Username).ToList());
    }

    [HttpGet("/GetFollowing")]
    public async Task<IActionResult> GetFollowing()
    {
        var id = await _userService.GetIdByUsername("");
        InstaUsers? result = await _connectionService.GetFollowing(id, false, true);
        if (result?.Users == null)
        {
            return NoContent();
        }

        return Ok(result.Users.Select(x => x.Username).ToList());
    }

    [HttpGet("/GetFirstFollowers")]
    public async Task<IActionResult> GetFirstFollowers()
    {
        var id = await _userService.GetIdByUsername("");
        var result = await _connectionService.GetFollowers(id);
        if (result?.Users == null)
        {
            return NoContent();
        }

        return Ok(result.Users.Select(x => x.Username).ToList());
    }

    [HttpGet("/GetFollowers")]
    public async Task<IActionResult> GetFollowers()
    {
        var id = await _userService.GetIdByUsername("");
        var result = await _connectionService.GetFollowers(id, false, true);
        if (result?.Users == null)
        {
            return NoContent();
        }

        return Ok(result.Users.Select(x => x.Username).ToList());
    }
}