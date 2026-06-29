using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly AppDbContext _context;
        public TodoItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<TodoItem> AddTodoItemAsync(TodoItem item)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteTodoItemByTitleAsync(string title, int? groupId)
        {
            var items = await _context.TodoItems
                .Where(t => t.Title == title && t.GroupItemId == groupId)
                .ToListAsync();
            if(items == null || items.Count == 0)
                return false;

            _context.TodoItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TodoItem>> GetAllTodoItemsAsync()
        {
           var allitems = await _context.TodoItems.ToListAsync();
           return allitems;

        }

        public async Task<List<TodoItem>> GetTodoItemByGroupIdAsync(int? groupId)
        {
            return await _context.TodoItems
                .Where(t => t.GroupItemId == groupId)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetTodoItemByIdAsync(int id)
        {
            var item = await _context.TodoItems
                .Include(t => t.GroupItem)
                .Include(t => t.RepitedGroup)
                .FirstOrDefaultAsync(t => t.Id == id);
            return item;
        }

        public async Task<List<TodoItem>> GetAllTodoItemByUserIdAsync(int userId)
        {
            return await _context.TodoItems
                .Include(t => t.GroupItem)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<TodoItem>> GetTodoItemsByGroupIdAsync(int groupId)
        {
            var items = await _context.TodoItems
                .Where(i => i.GroupItemId == groupId)
                .ToListAsync();
            return items;
        }

        public async Task<bool> IsCompleteTodoItemAsync(int id)
        {
            var item = await _context.TodoItems
                .FirstOrDefaultAsync(i => i.Id == id); 
            if(item == null)
                return false;
            item.IsComplete = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTodoItemAsync(TodoItem item)
        {
            var oldItem = await _context.TodoItems
                .FirstOrDefaultAsync(i => i.Id == item.Id);
            if(oldItem == null) 
                return false;
            oldItem.Title = item.Title;
            oldItem.Description = item.Description;
            oldItem.IsComplete = item.IsComplete;
            oldItem.Priority = item.Priority;
            oldItem.GroupItemId = item.GroupItemId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTodoItemByTodoIdAsync(int todoId)
        {
            var item = await _context.TodoItems.FindAsync(todoId);
            if(item == null) return false;

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
