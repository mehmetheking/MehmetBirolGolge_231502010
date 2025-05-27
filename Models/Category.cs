using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MehmetBirolGolge_231502010.Models
{
	public class Category
	{
		[Key]
		public int CategoryID { get; set; }

		[Required]
		[StringLength(50)]
		public string CategoryName { get; set; }

		[StringLength(7)]
		public string Color { get; set; } = "#000000";

		// Navigation Property
		public virtual ICollection<TaskCategory> TaskCategories { get; set; }
	}
}