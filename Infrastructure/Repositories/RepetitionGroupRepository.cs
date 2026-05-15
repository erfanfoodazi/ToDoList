using Application.Interfaces;
using Application.RepetitionGroups.UseCases.Dtos;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RepetitionGroupRepository : IRepetitionGroupRepository
    {
        private readonly AppDbContext _context;
        public RepetitionGroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CompleteRepetitionAsync(int repetitionId)
        {
            var repetition = await _context.RepetedGroups
              .Include(r => r.TodoItems)
              .FirstOrDefaultAsync(r => r.Id == repetitionId);

            if (repetition == null)
                return false;

            repetition.IsCompleted = true;
            repetition.CompletedAt = DateTime.Now;

            if (repetition.TodoItems != null)
            {
                foreach (var todo in repetition.TodoItems)
                {
                    todo.IsComplete = true;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteTodoItemRepetitionAsync(int todoItemId)
        {
            var todoItem = await _context.TodoItems
                .Include(t => t.RepitedGroup)
                .ThenInclude(r => r.TodoItems)
                .FirstOrDefaultAsync(t => t.Id == todoItemId);

            if (todoItem == null)
                return false;

            todoItem.IsComplete = !todoItem.IsComplete;

            if (todoItem.RepitedGroup != null)
            {
                var allTodosComplete = todoItem.RepitedGroup.TodoItems.All(t => t.IsComplete);

                if (allTodosComplete && !todoItem.RepitedGroup.IsCompleted)
                {
                    todoItem.RepitedGroup.IsCompleted = true;
                    todoItem.RepitedGroup.CompletedAt = DateTime.Now;
                }
                else if (!allTodosComplete && todoItem.RepitedGroup.IsCompleted)
                {
                    todoItem.RepitedGroup.IsCompleted = false;
                    todoItem.RepitedGroup.CompletedAt = null;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RepetedGroup> CreateRepetationGroupAsync(RepetedGroup repitedGroup)
        {
            if (repitedGroup == null)
                throw new ArgumentNullException(nameof(repitedGroup));

            await _context.RepetedGroups.AddAsync(repitedGroup);
            await _context.SaveChangesAsync();
            return repitedGroup;
        }

        public async Task<List<RepetedGroup>> GetAllRepetationsByGroupIdAsync(int groupId)
        {
            return await _context.RepetedGroups
                .Include(r => r.TodoItems)
                .Where(r => r.GroupId == groupId).ToListAsync();
        }

        public async Task<List<RepetedGroup>> GetAllRepetitionsByGroupIdAsync(int groupId)
        {
            return await _context.RepetedGroups
                .Include(r => r.TodoItems)
                .Include(r => r.GroupItem)
                .Where(r => r.GroupId == groupId).ToListAsync();
        }

        public async Task<List<RepetedGroup>> GetPendingRepetitionsByDateAsync(DateTime date)
        {
            return await _context.RepetedGroups
               .Include(r => r.TodoItems)
               .Include(r => r.GroupItem)
               .Where(r => r.RepetitionDate.Date == date.Date && !r.IsCompleted)
               .ToListAsync();
        }

        public async Task<RepetedGroup?> GetRepetitionGroupByIdAsync(int id)
        {
            return await _context.RepetedGroups
                .Include(r => r.TodoItems)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> AddTodoItemToAllRepetitionsAsync(int groupId, TodoItemDto todoItemDto)
        {
            var allRepetitions = await _context.RepetedGroups
                .Where(r => r.GroupId == groupId)
                .ToListAsync();

            if (!allRepetitions.Any())
            {
                var group = await _context.GroupItems
                    .Include(g => g.TodoItems)
                    .FirstOrDefaultAsync(g => g.Id == groupId);

                if (group == null)
                    return false;

                var newRepetition = new RepetedGroup
                {
                    GroupId = groupId,
                    RepetitionDate = DateTime.Today,
                    IsCompleted = false,
                    TodoItems = new List<TodoItem>()
                };

                var newTodo = new TodoItem
                {
                    Title = todoItemDto.Title,
                    Description = todoItemDto.Description,
                    Priority = (Priority)todoItemDto.Priority,
                    IsComplete = false,
                    Created = DateTime.Now,
                    UserId = todoItemDto.UserId,
                    GroupItemId = groupId,
                    RepitedGroupId = newRepetition.Id
                };

                newRepetition.TodoItems.Add(newTodo);

                if (group.TodoItems != null)
                {
                    foreach (var existingTodo in group.TodoItems)
                    {
                        newRepetition.TodoItems.Add(new TodoItem
                        {
                            Title = existingTodo.Title,
                            Description = existingTodo.Description,
                            Priority = existingTodo.Priority,
                            IsComplete = false,
                            Created = DateTime.Now,
                            UserId = todoItemDto.UserId,
                            GroupItemId = groupId,
                            RepitedGroupId = newRepetition.Id
                        });
                    }
                }

                await _context.RepetedGroups.AddAsync(newRepetition);
                await _context.SaveChangesAsync();
                return true;
            }

            foreach (var repetition in allRepetitions)
            {
                var newTodo = new TodoItem
                {
                    Title = todoItemDto.Title,
                    Description = todoItemDto.Description,
                    Priority = (Priority)todoItemDto.Priority,
                    IsComplete = false,
                    Created = DateTime.Now,
                    UserId = todoItemDto.UserId,
                    GroupItemId = groupId,
                    RepitedGroupId = repetition.Id
                };

                await _context.TodoItems.AddAsync(newTodo);
            }

            var mainGroup = await _context.GroupItems.FindAsync(groupId);
            if (mainGroup != null)
            {
                var newTodoForGroup = new TodoItem
                {
                    Title = todoItemDto.Title,
                    Description = todoItemDto.Description,
                    Priority = (Priority)todoItemDto.Priority,
                    IsComplete = false,
                    Created = DateTime.Now,
                    UserId = todoItemDto.UserId,
                    GroupItemId = groupId
                };

                if (mainGroup.TodoItems == null)
                    mainGroup.TodoItems = new List<TodoItem>();

                mainGroup.TodoItems.Add(newTodoForGroup);
            }

            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> UpdateTodoItemInAllRepetitionsAsync(int groupId, string oldTitle, TodoItem updatedTodoItem)
        {
            var allRepetitions = await _context.RepetedGroups
                .Include(r => r.TodoItems)
                .Where(r => r.GroupId == groupId)
                .ToListAsync();

            if (!allRepetitions.Any())
                return false;

            foreach (var repetition in allRepetitions)
            {
                var todo = repetition.TodoItems.FirstOrDefault(t => t.Title == oldTitle);
                if (todo != null)
                {
                    todo.Title = updatedTodoItem.Title;
                    todo.Description = updatedTodoItem.Description;
                    todo.Priority = updatedTodoItem.Priority;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveTodoItemFromAllRepetitionsAsync(int groupId, string todoTitle)
        {
            var allRepetitions = await _context.RepetedGroups
                .Include(r => r.TodoItems)
                .Where(r => r.GroupId == groupId)
                .ToListAsync();

            if (!allRepetitions.Any())
                return false;

            foreach (var repetition in allRepetitions)
            {
                var todoToRemove = repetition.TodoItems.FirstOrDefault(t => t.Title == todoTitle);
                if (todoToRemove != null)
                {
                    repetition.TodoItems.Remove(todoToRemove);
                    _context.TodoItems.Remove(todoToRemove);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
