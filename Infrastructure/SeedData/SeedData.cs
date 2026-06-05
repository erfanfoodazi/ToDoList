//using Domain.Entities;
//using Domain.Enums;
//using Infrastructure.DBContext;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Infrastructure
//{
//    public static class SeedData
//    {
//        public static async Task InitializeAsync(IServiceProvider serviceProvider)
//        {
//            using var scope = serviceProvider.CreateScope();
//            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//            // Ensure database is created
//            await context.Database.EnsureCreatedAsync();

//            // If data already exists, exit
//            if (context.GroupItems.Any())
//                return;

//            // ========== Create test groups ==========
//            var groups = new[]
//            {
//                new GroupItem("Daily Tasks", (int)RepetitionType.Daily,DateTime.Now.AddMonths(2), 1),
//                new GroupItem("Important Project", (int)RepetitionType.None, DateTime.Now.AddMonths(2),1),
//                new GroupItem("Entertainment", (int)RepetitionType.Weekly,DateTime.Now.AddMonths(2), 1),
//                new GroupItem("Learning Programming", (int)RepetitionType.Daily, DateTime.Now.AddMonths(2), 1)
//            };

//            await context.GroupItems.AddRangeAsync(groups);
//            await context.SaveChangesAsync();

//            // ========== Create TodoItems ==========
//            var todos = new[]
//            {
//                new TodoItem("Buy groceries", "From the nearby supermarket", (int)Priority.Medium, groups[0].Id),
//                new TodoItem("Call the professor", "About the project", (int)Priority.High, groups[0].Id),
//                new TodoItem("Exercise", "30 minutes running", (int)Priority.Low, groups[0].Id),

//                new TodoItem("Review documentation", "Study EF Core documentation", (int)Priority.High, groups[1].Id),
//                new TodoItem("Design database", "Complete the diagram", (int)Priority.High, groups[1].Id),
//                new TodoItem("Develop API", "Create controllers", (int)Priority.High, groups[1].Id),

//                new TodoItem("Watch a movie", "New Nolan film", (int)Priority.Medium, groups[2].Id),
//                new TodoItem("Play video games", "1 hour", (int)Priority.Low, groups[2].Id),

//                new TodoItem("Watch tutorial", "C# educational video", (int)Priority.High, groups[3].Id),
//                new TodoItem("Practice coding", "Implement practical project", (int)Priority.Critical, groups[3].Id),
//                new TodoItem("Read a book", "Clean Code - Chapter 3", (int)Priority.Medium, groups[3].Id)
//            };

//            await context.TodoItems.AddRangeAsync(todos);
//            await context.SaveChangesAsync();
//        }
//    }
//}