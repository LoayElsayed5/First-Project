﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Restaurantopia.Entities.Models
{
	public class Item
	{
		[Key]
		public int Id { get; set; }
		[StringLength ( 100 )]
		public string ItemTitle { get; set; }
		[StringLength ( 120 )]
		public string? ItemDescription { get; set; }
		[StringLength ( 500 )]
		public string ItemImage { get; set; }
		public decimal ItemPrice { get; set; }
		

		[ForeignKey ( "Category" )]
		public int CategoryId { get; set; }
		public Category Category { get; set; }
        
        public ICollection<OrderDetails> OrderDetails { get; set; }
        [NotMapped]
        public List<Category> categoryList { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
