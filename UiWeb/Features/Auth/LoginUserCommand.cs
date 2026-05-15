//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Domain.Entities;

//namespace UiWeb.Features.Auth
//{
//    public class LoginUserCommand : IRequest<bool>
//    {
//        public string UserNameOrEmail { get; set; } = string.Empty;
//        public string Password { get; set; } = string.Empty;
//        public bool RememberMe { get; set; } = false;
//    }

//    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, bool>
//    {
//        private readonly SignInManager<User> _signInManager;
//        private readonly ILogger<LoginUserCommandHandler> _logger;

//        public LoginUserCommandHandler(SignInManager<User> signInManager, ILogger<LoginUserCommandHandler> logger)
//        {
//            _signInManager = signInManager;
//            _logger = logger;
//        }

//        public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                _logger.LogInformation("Attempting login for user: {UserNameOrEmail}", request.UserNameOrEmail);

//                var result = await _signInManager.PasswordSignInAsync(
//                    request.UserNameOrEmail,
//                    request.Password,
//                    request.RememberMe,
//                    lockoutOnFailure: false
//                );

//                if (result.Succeeded)
//                {
//                    _logger.LogInformation("User {UserNameOrEmail} logged in successfully", request.UserNameOrEmail);
//                }
//                else if (result.IsLockedOut)
//                {
//                    _logger.LogWarning("User {UserNameOrEmail} is locked out", request.UserNameOrEmail);
//                }
//                else if (result.IsNotAllowed)
//                {
//                    _logger.LogWarning("User {UserNameOrEmail} is not allowed to login", request.UserNameOrEmail);
//                }
//                else
//                {
//                    _logger.LogWarning("Failed login attempt for user: {UserNameOrEmail}", request.UserNameOrEmail);
//                }

//                return result.Succeeded;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error during login for user: {UserNameOrEmail}", request.UserNameOrEmail);
//                return false;
//            }
//        }
//    }
//}
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UiWeb.Services;

namespace UiWeb.Features.Auth
{
    public class LoginUserCommand : IRequest<bool>
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, bool>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<LoginUserCommandHandler> logger
               )
            
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            }

            if (user == null) return false;

            _logger.LogInformation("User: {UserName}, EmailConfirmed: {EmailConfirmed}, Lockout: {Lockout}",
                user.UserName, user.EmailConfirmed, user.LockoutEnabled);

            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            if (user.LockoutEnabled)
            {
                user.LockoutEnabled = false;
                await _userManager.UpdateAsync(user);
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                return false;

            await _signInManager.SignInAsync(user, request.RememberMe);

            _logger.LogInformation("User {UserName} logged in", user.UserName);

            return true;
        }
    }
}