using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGroupItemRepository
    {
        Task<List<GroupItem>> GetAllGroupItemsAsync();
        Task<GroupItem?> GetGroupItemByIdAsync(int id);
        Task<bool> DeleteGroupItemAsync(int id);
        Task<bool> UpdateGroupItemAsync(GroupItem groupItem);
        Task<GroupItem> AddGroupItemAsync(GroupItem groupItem);
        Task<List<GroupItem>> GetAllGroupItemsByUserId(int userId);
    }
}
