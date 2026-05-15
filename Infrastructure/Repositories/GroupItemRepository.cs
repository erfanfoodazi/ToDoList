using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories
{
    public class GroupItemRepository : IGroupItemRepository
    {
        private readonly AppDbContext _context;
        public GroupItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<GroupItem> AddGroupItemAsync(GroupItem groupItem)
        {
            if (groupItem == null) 
                throw new ArgumentNullException(nameof(groupItem));

            await _context.GroupItems.AddAsync(groupItem);
            await _context.SaveChangesAsync();
            return groupItem;
        }

        public async Task<bool> DeleteGroupItemAsync(int id)
        {
            var groupItem = await _context.GroupItems
                .Include(x => x.TodoItems)
                .Include(x => x.RepetedGroups)
             .FirstOrDefaultAsync(x => x.Id == id);

            if (groupItem == null)
                return false;

            if (groupItem.TodoItems != null && groupItem.TodoItems.Any())
            {
                _context.TodoItems.RemoveRange(groupItem.TodoItems);
            }

            if (groupItem.RepetedGroups != null && groupItem.RepetedGroups.Any())
            {
                _context.RepetedGroups.RemoveRange(groupItem.RepetedGroups);
            }

            _context.GroupItems.Remove(groupItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<GroupItem>> GetAllGroupItemsByUserId(int userId)
        {
            var groups = await _context.GroupItems
                .Include(g  => g.RepetedGroups)
                .Include(x => x.TodoItems)
                .Where(g => g.UserId == userId)
                .ToListAsync();
            return groups;

        }

        public async Task<List<GroupItem>> GetAllGroupItemsAsync()
        {
            var allGroup = await _context.GroupItems
                .Include(x => x.RepetedGroups)
                .Include(c => c.TodoItems)
                .ToListAsync();
            return allGroup;
        }

        public async Task<GroupItem?> GetGroupItemByIdAsync(int id)
        {
            var groupItem = await _context.GroupItems
                .Include(c => c.TodoItems)
                .FirstOrDefaultAsync(x => x.Id == id);
            return groupItem;
        }

        public async Task<bool> UpdateGroupItemAsync(GroupItem groupItem)
        {

            var oldgroupItem = await _context.GroupItems
                .FirstOrDefaultAsync(c => c.Id == groupItem.Id);

            if (oldgroupItem == null)
                return false;
            oldgroupItem.Title = groupItem.Title;
            oldgroupItem.EndRepit = groupItem.EndRepit;
            oldgroupItem.RepetitionType = groupItem.RepetitionType;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
