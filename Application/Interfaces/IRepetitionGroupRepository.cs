using Application.RepetitionGroups.UseCases.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepetitionGroupRepository
    {
        Task<List<RepetedGroup>> GetAllRepetationsByGroupIdAsync(int groupId);
        Task<List<RepetedGroup>> GetAllRepetitionsByGroupIdAsync(int  groupId);
        Task<RepetedGroup?> GetRepetitionGroupByIdAsync(int id);
        Task<RepetedGroup> CreateRepetationGroupAsync(RepetedGroup repitedGroup);
        Task<bool> AddTodoItemToAllRepetitionsAsync(int groupId, TodoItemDto todoItemDto);
        Task<bool> CompleteRepetitionAsync(int repetitionId);
        Task<bool> CompleteTodoItemRepetitionAsync(int todoItemId);
        Task<List<RepetedGroup>> GetPendingRepetitionsByDateAsync(DateTime date);
        Task<bool> UpdateTodoItemInAllRepetitionsAsync(int groupId, string oldTitle, TodoItem updatedTodoItem);
        Task<bool> RemoveTodoItemFromAllRepetitionsAsync(int groupId, string todoTitle);
        

    }
}
