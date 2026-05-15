using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITodoItemRepository  
    {
        Task<List<TodoItem>> GetAllTodoItemsAsync();
        Task<TodoItem?> GetTodoItemByIdAsync(int id);  
        Task<List<TodoItem>> GetAllTodoItemByUserIdAsync(int id);
        Task<List<TodoItem>> GetTodoItemByGroupIdAsync(int groupId);
        Task<TodoItem> AddTodoItemAsync(TodoItem item);
        Task<bool> UpdateTodoItemAsync(TodoItem item); 
        Task<bool> DeleteTodoItemByIdAsync(int id); 
        Task<bool> IsCompleteTodoItemAsync(int id);
        Task<List<TodoItem>> GetTodoItemsByGroupIdAsync(int groupId);
    }
}