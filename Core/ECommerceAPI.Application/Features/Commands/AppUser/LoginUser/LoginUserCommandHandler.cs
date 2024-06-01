using ECommerceAPI.Application.Abstraction.Services;
using ECommerceAPI.Application.Abstraction.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly IAuthService _authService;
        readonly IUserService _userService;
        public LoginUserCommandHandler(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
           
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, 900);
            var user = await _userService.GetByEMail(request.UsernameOrEmail);

                return new LoginUserSuccessCommandResponse()
                {
                    Token = token,
                    IsAdmin = user.IsAdmin
                };
            

        }
    }
}
