using Application.GroupItems.UseCases.Queries;
using Application.RepetitionGroups.UseCases.Commands;
using Application.RepetitionGroups.UseCases.Dtos;
using Application.RepetitionGroups.UseCases.Queries;
using Application.TodoItems.UseCases.Commands;
using Application.TodoItems.UseCases.Queries;
using Application.Users.UseCases.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UiWeb.Models;

namespace UiWeb.Controllers
{
    public class TodoItemsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TodoItemsController> _logger;
        public TodoItemsController(IMediator mediator, ILogger<TodoItemsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("TodoItems/GroupTodos/{groupId}")]
        public async Task<IActionResult> GroupTodos(int groupId)
        {
            var userId = GetCurrentUserId();

            var group = await _mediator.Send(new GetGroupItemsByIdQuery { Id = groupId });
            if (group == null || group.UserId != userId)
            {
                return NotFound();
            }

            var repetitions = await _mediator.Send(new GetAllRepetitionByGroupQuery { GroupId = groupId });

            var viewModel = new GroupTodosViewModel
            {
                GroupId = group.Id,
                GroupTitle = group.Title ?? string.Empty,
                RepetitionType = group.RepetitionType.ToString(),
                CreatedAt = group.CreatedAt?.ToString("yyyy/MM/dd"),
                Repetitions = repetitions.Select(r => new RepetitionViewModel
                {
                    Id = r.Id,
                    RepetitionDate = r.RepetitionDate.ToString("yyyy/MM/dd"),
                    IsCompleted = r.IsCompleted,
                    CompletedAt = r.CompletedAt,
                    TodoItems = r.TodoItems?.Select(t => new TodoItemViewModel
                    {
                        Id = t.Id,
                        Title = t.Title ?? string.Empty,
                        Description = t.Description,
                        Priority = t.Priority.ToString(),
                        IsComplete = t.IsComplete,
                        CreatedAt = t.Created?.ToString("yyyy/MM/dd")
                    }).ToList() ?? new()
                }).ToList()
            };
            viewModel.RateOfSuccesses = await rateOfSuccess(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int taskId)
        {
            var todo = await _mediator.Send(new GetTodoByIdQuery { Id = taskId });
            if (todo == null)
            {
                TempData["ErrorMessage"] = "Task not found.";
                return RedirectToAction("Index", "GroupItems");
            }
            var repetition = await _mediator.Send(new GetRepitedGroupByIdQuery { Id = (int)todo.RepitedGroupId });

            var result = await _mediator.Send(new CompleteTodoItemRepetitionCommand { TodoId = taskId });
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to update task status.";
                return RedirectToAction("GroupTodos", new { groupId = todo.GroupItemId });
            }

            TempData["SuccessMessage"] = "Task status updated successfully!";
            return Redirect($"/TodoItems/GroupTodos/{repetition.GroupId}");
        }

        [HttpGet("TodoItems/AddTodoItem/{groupId}")]
        public async Task<IActionResult> AddTodoItem(int groupId)
        {
            
            var userId = GetCurrentUserId();
            var group = await _mediator.Send(new GetGroupItemsByIdQuery { Id = groupId });

            if (group == null || group.UserId != userId)
            {
                TempData["ErrorMessage"] = "Group not found.";
                return RedirectToAction("Index", "GroupItems");
            }

            var viewModel = new TodoItemViewModel
            {
                GroupId = groupId,
                PriorityOptions = GetPriorityOptions()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTodoItem(TodoItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.PriorityOptions = GetPriorityOptions();
                return View(viewModel);
            }

            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Users");
            }

            if (!int.TryParse(viewModel.Priority, out int priority))
            {
                TempData["ErrorMessage"] = "Invalid priority value.";
                return RedirectToAction("GroupTodos", new { groupId = viewModel.GroupId });
            }

            var todoDto = new TodoItemDto
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                Priority = priority,
                IsComplete = false,
                Created = DateTime.Now,
                UserId = userId,
                GroupItemId = viewModel.GroupId,
            };

            var result = await _mediator.Send(new AddTodoItemToRepetitionCommand
            {
                GroupId = viewModel.GroupId,
                TodoItem = todoDto
            });

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to add new task";
                return RedirectToAction("GroupTodos", new { groupId = viewModel.GroupId });
            }

            TempData["SuccessMessage"] = "Task added successfully to all repetitions!";
            return RedirectToAction("GroupTodos", new { groupId = viewModel.GroupId });
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(int  taskId,string Title,string Description,int Priority)
        {
            var todo = await _mediator.Send(new GetTodoByIdQuery { Id = taskId });
            if(todo == null)
                TempData["ErrorMessage"] = "Failed to edit task";
            var result = await _mediator.Send(new UpdateTodoCommand
            {
                Id = taskId,
                Title = Title,
                Description = Description,
                Priority = Priority,
                GroupItemId = (int)todo.GroupItemId
            });
            if (!result)
                TempData["ErrorMessage"] = "Failed to edit task";

            return RedirectToAction("GroupTodos", new { groupId = todo.GroupItemId });
        }

        [HttpPost("TodoItems/DeleteTask/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var todo = await _mediator.Send(new GetTodoByIdQuery { Id = taskId });
            if (todo == null)
            {
                TempData["ErrorMessage"] = "Failed to delete task";
                _logger.LogError("fail to find Task");
                return RedirectToAction("Index","GroupItems");
            }
            var result = await _mediator.Send(new DeleteTodoCommand 
            {
                GroupId = todo.GroupItemId,
                Title = todo.Title,
            });

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to delete task";
                _logger.LogError($"fail to Delete {todo.Title}");
                return RedirectToAction("GroupTodos", new { groupId = todo.GroupItemId });
            }
            TempData["SuccessMessage"] = "delete was successfully";
            _logger.LogInformation($"delet {todo.Title} was successfully");
            return RedirectToAction("GroupTodos",new {groupId = todo.GroupItemId});
        }

        private List<SelectListItem> GetPriorityOptions()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Value = "0", Text = "Low" },
        new SelectListItem { Value = "1", Text = "Medium" },
        new SelectListItem { Value = "2", Text = "High" },
        new SelectListItem { Value = "3", Text = "Critical" }
    };
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        private async Task<List<RateOfSuccess>> rateOfSuccess(GroupTodosViewModel group)
        {
            var repetitions = await _mediator.Send(new GetAllRepetitionByGroupQuery { GroupId = group.GroupId });

            var result = new List<RateOfSuccess>();
            foreach (var repetition in repetitions)
            {
                var count = repetition.TodoItems.Count;
                int competed = 0;
                foreach (var task in repetition.TodoItems)
                {
                    if(task.IsComplete)
                        competed++;
                }
                var rate = (int)Math.Round((double)competed / count * 100);
                var rateOfSucces = new RateOfSuccess()
                {
                    Rate = rate,
                    Successed = repetition.RepetitionDate
                };

                result.Add(rateOfSucces);
            }

            return result;
        }

    }
}
