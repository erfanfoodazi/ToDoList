// UiWeb/Controllers/GroupItemsController.cs
using Application.GroupItems.UseCases.Commands;
using Application.GroupItems.UseCases.Queries;
using Application.RepetitionGroups.UseCases.Queries;
using Application.Users.UseCases.Queries;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using UiWeb.Models;

namespace UiWeb.Controllers
{
    [Authorize]
    public class GroupItemsController : Controller
    {
        private readonly IMediator _mediator;

        public GroupItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var groups = await _mediator.Send(new GetAllGroupItemsByUserIdQuery { UserId = userId });
            var viewModels = groups.Select(g => new GroupItemViewModel
            {
                Id = g.Id,
                Title = g.Title ?? string.Empty,
                RepetitionType = g.RepetitionType.ToString(),
                RepetitionTypeValue = (int)g.RepetitionType,
                CreatedAt = g.CreatedAt?.ToString("yyyy/MM/dd HH:mm"),
                EndDate = g.EndRepit?.ToString("yyyy/MM/dd"),
                TodoItemsCount = g.RepetedGroups?.Count ?? 0,
                TodoItems = g.TodoItems?.Select(t => new TodoItemViewModel
                {
                    Id = t.Id,
                    Title = t.Title ?? string.Empty,
                    Description = t.Description,
                    Priority = t.Priority.ToString(),
                    IsComplete = t.IsComplete,
                    CreatedAt = t.Created?.ToString("yyyy/MM/dd HH:mm")
                }).ToList()
            }).ToList();

            return View(viewModels);
        }

        [HttpGet("creategroup")]
        public IActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost("creategroup")]
        public async Task<IActionResult> CreateGroup(GroupItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Users");
            }

            DateTime? endDate = null;
            if (!string.IsNullOrWhiteSpace(model.EndDate))
            {
                if (DateTime.TryParse(model.EndDate, out DateTime parsedDate))
                {
                    endDate = parsedDate;
                }
                else
                {
                    ModelState.AddModelError("EndDate", "Invalid date format");
                    return View(model);
                }
            }

            var result = await _mediator.Send(new CreateGroupItemCommand
            {
                Title = model.Title,
                RepetitionType = model.RepetitionTypeValue,
                EndDate = endDate,
                UserId = userId,
            });
            if (result != null)
            {
                TempData["SuccessMessage"] = "Group created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create group.";
                return View(model);
            }

        }

        [HttpGet("editgroup/{id}")]
        public async Task<IActionResult> EditGroup(int id)
        {
            var group = await _mediator.Send(new GetGroupItemsByIdQuery { Id = id });
            var model = new GroupItemViewModel
            {
                Id = id,
                Title = group.Title,
                EndDate = group.EndRepit.ToString(),
                CreatedAt = group.CreatedAt.ToString(),
                RepetitionTypeValue = (int)group.RepetitionType,
                RepetitionType = group.RepetitionType.ToString(),
                
            };
            return View(group);
        }

        [HttpPost("editgroup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(int id, string title, int repetitionTypeValue, DateTime? endDate)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Users");
            }

            var result = await _mediator.Send(new UpdateGroupItemCommand
            {
                Id = id,
                Title = title,
                RepetitionType = repetitionTypeValue,
                EndDate = endDate
            });

            if (result)
            {
                TempData["SuccessMessage"] = "Group updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update group.";
            }

            return RedirectToAction("Index");
        }


        [HttpPost("GroupItems/DeleteGroup")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var result = await _mediator.Send(new DeleteGroupItemCommand { Id = id });
            if (result)
            {
                TempData["SuccessMessage"] = "Group deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete group. Please try again.";
            }

            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> StatisticsUser()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                TempData["ErrorMessage"] = "Failed to find user. please login.";
                return RedirectToAction("Login","Users");
            }
            var user = await _mediator.Send(new GetUserByIdQuery { Id = userId });
            if (user == null)
            {
                TempData["ErrorMessage"] = "Failed to find user. please login.";
                return RedirectToAction("Login", "Users");
            }

            var lowPriorityRate = await GetSuccessrateWithPriority(userId , 0);
            var meduimPriorityRate = await GetSuccessrateWithPriority(userId , 1);
            var highPriorityRate = await GetSuccessrateWithPriority(userId , 2);
            var criticalPriorityRate = await GetSuccessrateWithPriority(userId , 3);

            var model = new StatisticsUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                LowPriority = lowPriorityRate,
                MeduimPriority = meduimPriorityRate,
                HighPriority = highPriorityRate,
                CriticalPriority = criticalPriorityRate
            };

            return View(model);
        }
        private async Task<List<RateOfSuccess>> GetSuccessrateWithPriority(int userId, int priority)
        {
            var userGroup = await _mediator.Send(new GetAllGroupItemsByUserIdQuery { UserId = userId });

            var Rate = userGroup
                .Where(g => g.RepetedGroups != null)
                .SelectMany(g => g.RepetedGroups ?? new List<RepetedGroup>())
                .Where(r => r.TodoItems != null)
                .SelectMany(r => r.TodoItems ?? new List<TodoItem>())
                .Where(r => (int)r.Priority == priority && r.IsComplete == true) 
                .GroupBy(r => r.Created.Value.Date) 
                .Select(g => new RateOfSuccess
                {
                    Rate = g.Count(),    
                    Successed = g.Key
                })
                .OrderBy(r => r.Successed)     
                .ToList();

            return Rate;
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }
}