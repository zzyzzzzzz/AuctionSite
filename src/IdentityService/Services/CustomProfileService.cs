using System;
using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);//take the userId from the context
    //then we can get the claims from the user
    //现有的声明是我们真正为用户添加的唯一声明the existing claim the only claim that we really added for our user was let full name and we stored that in the name claim.
        var existingClaims = await _userManager.GetClaimsAsync(user);    
        
        var claims = new List<Claim>
        {
            new Claim("username", user.UserName),
        };
        context.IssuedClaims.AddRange(claims);
        context.IssuedClaims.Add(existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name));
    }
    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;

        //然后去前面来告诉身份服务器这个自定义配置文件服务,去hostingExtensions加上.AddProfileService<CustomProfileService>();
    }
}
