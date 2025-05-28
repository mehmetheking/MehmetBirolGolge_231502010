using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MehmetBirolGolge_231502010.Models
{
	public class TaskCategory
	{
		[Key, Column(Order = 0)]
		[ForeignKey("Task")]
		public int TaskID { get; set; }

		[Key, Column(Order = 1)]
		[ForeignKey("Category")]
		public int CategoryID { get; set; }

		// Navigation Properties
		public virtual Task Task { get; set; }
		public virtual Category Category { get; set; }
	}
}