using Microsoft.EntityFrameworkCore;
using Plank.Core.Data;
using Todo.Api.Data.Models;

namespace Todo.Api.Data
{
    public class TodoContext : PlankDbContext
    {
        private readonly string _csvFilePath;

        public TodoContext(DbContextOptions<TodoContext> options, string csvFilePath) : base(options)
        {
            _csvFilePath = csvFilePath;
        }

        public DbSet<TodoModel> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Load data from CSV file
            var todoItems = LoadTodoItemsFromCsv(_csvFilePath);
            modelBuilder.Entity<TodoModel>().HasData(todoItems);
        }

        private List<TodoModel> LoadTodoItemsFromCsv(string csvFilePath)
        {
            var todoItems = new List<TodoModel>();
            var lines = File.ReadAllLines(csvFilePath);

            foreach (var line in lines.Skip(1)) // Skip header
            {
                var values = line.Split(',');
                var todoItem = new TodoModel
                {
                    Id = int.Parse(values[0]),
                    GlobalId = Guid.Parse(values[1]),
                    DateCreated = DateTime.Parse(values[2]),
                    DateLastModified = DateTime.Parse(values[3]),
                    IsCompleted = bool.Parse(values[4]),
                    Title = values[5]
                };
                todoItems.Add(todoItem);
            }

            return todoItems;
        }
    }
}