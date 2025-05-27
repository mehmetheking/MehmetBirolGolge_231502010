using System.Data.Entity;
using MehmetBirolGolge_231502010.Models;

namespace MehmetBirolGolge_231502010.Data
{
	public class TodoContext : DbContext
	{
		public TodoContext() : base("name=TodoConnection")
		{
			// Database yoksa oluştur
			Database.SetInitializer(new CreateDatabaseIfNotExists<TodoContext>());
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Task> Tasks { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<TaskCategory> TaskCategories { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// TaskCategory için composite key tanımı
			modelBuilder.Entity<TaskCategory>()
				.HasKey(tc => new { tc.TaskID, tc.CategoryID });

			base.OnModelCreating(modelBuilder);
		}
	}
}