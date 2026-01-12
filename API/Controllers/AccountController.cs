using System;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(SignInManager<AppUser> signInManager): BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDTO registerDTO)
    {
        var user = new AppUser
        {
            FirstName = registerDTO.FirstName,
            LastName = registerDTO.LastName,
            Email = registerDTO.Email,
            UserName = registerDTO.Email
        };

        var result = await signInManager.UserManager.CreateAsync(user, registerDTO.Password);

        if (result.Succeeded == false)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }

    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            return NoContent();
        }

        var user = await signInManager.UserManager.GetUserWithAddressByEmail(User);

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Address = user.Address?.ToDto()  
        }); 
    }

    [HttpGet("authentication-status")]
    public ActionResult<bool> GetAuthenticationState()
    {
        var IsAuthenticated = false;

        if(User.Identity != null && User.Identity.IsAuthenticated == true)
        {
            IsAuthenticated = true;
        }

        return Ok(IsAuthenticated);
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDTO addressDTO)
    {
        var user = await signInManager.UserManager.GetUserWithAddressByEmail(User);

        if (user.Address == null)
        {
            user.Address = addressDTO.ToEntity();
        }
        else
        {
            user.Address.UpdateFromDto(addressDTO);
        }

        var result = await signInManager.UserManager.UpdateAsync(user);

        if (result.Succeeded == false)
        {
            return BadRequest("A problem occurred while updating the user address.");
        }

        return Ok(user.Address.ToDto());
    }
}
