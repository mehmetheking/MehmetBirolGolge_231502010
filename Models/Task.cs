using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MehmetBirolGolge_231502010.Models
{
	public class Task
	{
		[Key]
		public int TaskID { get; set; }

		[ForeignKey("User")]
		public int UserID { get; set; }

		[Required]
		[StringLength(200)]
		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime? DueDate { get; set; }

		public bool IsCompleted { get; set; } = false;

		[StringLength(20)]
		public string Priority { get; set; } = "Orta";

		public string AIDescription { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.Now;

		// Navigation Properties
		public virtual User User { get; set; }
		public virtual ICollection<TaskCategory> TaskCategories { get; set; }
	}
}