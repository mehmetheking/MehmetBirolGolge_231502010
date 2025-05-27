using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MehmetBirolGolge_231502010.Models
{
	public class User
	{
		[Key]
		public int UserID { get; set; }

		[Required]
		[StringLength(50)]
		public string Username { get; set; }

		[Required]
		[StringLength(100)]
		public string Password { get; set; }

		[Required]
		[StringLength(100)]
		public string Email { get; set; }

		[StringLength(20)]
		public string Role { get; set; } = "User"; // Default olarak User

		public DateTime CreatedDate { get; set; } = DateTime.Now;

		// Navigation Property
		public virtual ICollection<Task> Tasks { get; set; }
	}
}