using Application.GroupItems.UseCases.Queries;
using Application.RepetitionGroups.UseCases.Queries;
using Application.Users.UseCases.Commands;
using Application.Users.UseCases.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using UiWeb.Features.Auth;
using UiWeb.Features.Dtos;
using UiWeb.Models;

namespace UiWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var result = await _mediator.Send(new LoginUserCommand
            {
                UserNameOrEmail = model.UserNameOrEmail,
                Password = model.Password,
                RememberMe = model.RememberMe
            });

            if (result)
                return RedirectToAction("Index", "GroupItems");

            ModelState.AddModelError("", "Invalid email/username or password");
            return View(model);
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                return View(model);
            }



            try
            {
                var user = await _mediator.Send(new CreateUserCommand
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password
                });

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new LogoutUserCommand());
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> ProfileUser()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Users");
            }


            var user = await _mediator.Send(new GetUserByIdQuery { Id = userId });
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index", "GroupItems");
            }

            var successRate = await GetSuccessrate(userId);

            var userModel = new ProfileUserViewModel()
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                SuccessRate = successRate,
            };

            return View(userModel);
        }

        private async Task<int> GetSuccessrate(int userId)
        {
            var userGroup = await _mediator.Send(new GetAllGroupItemsByUserIdQuery { UserId = userId });

            var allTodoItems = userGroup
                .Where(g => g.RepetedGroups != null)
                .SelectMany(g => g.RepetedGroups ?? new List<RepetedGroup>())
                .Where(r => r.TodoItems != null)
                .SelectMany(r => r.TodoItems ?? new List<TodoItem>())
                .ToList();

            int totalCount = allTodoItems.Count;

            if (totalCount == 0)
                return 0;

            int completedCount = allTodoItems.Count(t => t.IsComplete);
            return (completedCount * 100) / totalCount;
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

       
    }
}