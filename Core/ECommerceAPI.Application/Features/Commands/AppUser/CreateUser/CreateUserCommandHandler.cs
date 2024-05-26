using ECommerceAPI.Application.Abstraction.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;
        readonly UserManager<ECommerceAPI.Domain.Entities.Identity.AppUser> _userManager;
        public CreateUserCommandHandler(IUserService userService, UserManager<Domain.Entities.Identity.AppUser> userManager)
        {

            _userService = userService;
            _userManager = userManager;
        }


        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                NameSurname = request.NameSurname,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                Username = request.Username,
                
            });
            if (response.Succeeded) {
                
                string[] defaultRole = new string[] { "Normal Kullanıcı" };
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    await _userService.AssignRoleToUserAsnyc(user.Id, defaultRole);
                }
            }

            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
            };


        }
    }
}
