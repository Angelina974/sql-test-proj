using Microsoft.AspNetCore.Mvc;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using sql_test.Buisness;
using System.Net;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

[ApiController]
[Route("auth")]
public class AuthController(IUserService _userService, IConfiguration _configuration) : ControllerBase
{
    private bool IsRedirectUriAllowed(string redirectUri)
    {
        var allowedOrigins = _configuration.GetValue<string>("AllowedRedirectOrigins");
        return !redirectUri.StartsWith("/")
        && allowedOrigins != null
        && allowedOrigins.Any(origin => redirectUri.StartsWith($"{origin}/"));
    }

     [HttpGet("signin")]
     [ProducesResponseType(302)]
     public IActionResult SignIn([FromQuery] string redirectUri = "/auth/userinfo")
    {
        if (!IsUnknownRedirectUri(redirectUri))
        {
            return BadRequest();
        }
        var properties = new AuthenticationProperties 
        { 
            RedirectUri = $"/auth/callback?redirect_uri={WebUtility.UrlEncode(redirectUri)}", 
        };

        return Challenge(properties, GitHubAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet("callback")]
    [ProducesResponseType(302)]
    public async Task<IActionResult> HandleCallback([FromQuery] string redirectUri = "/auth/userinfo")
    {
        await _userService.HandleSuccessfulSignin(HttpContext.User.Claims);

        if (IsRedirectUriAllowed(redirectUri))
        {
            return BadRequest();
        }
        await _userService.HandleSuccessfulSignin(HttpContext.User.Claims);
        return Redirect(redirectUri);
    }

    [Authorize]
    [HttpGet("userinfo")]
    [ProducesResponseType<User>(200)]
    public async Task<IActionResult> GetUserInfo()
    {
       var user = await _userService.GetCurrent(User.Claims);
        return Ok(user);
    }

    public bool IsUnknownRedirectUri(string redirectUri)
    {
        var allowedOrigins = _configuration.GetValue<string[]>("AllowedOrigins");
        return !redirectUri.StartsWith("/") && !allowedOrigins.Any(origin => redirectUri.StartsWith(origin));
    }

    [HttpGet("signout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}
