using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tweet.Models
{
    public class Tweet
    {
        public int Id { get; set; }

        [Display(Name= "User Name")]
        public string? UserName { get; set; }

        [Required]
        public string?  Title { get; set; }

        public string? Content { get; set; }

        [Display(Name = "Time Stamp")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime TimeStamp { get; set; }  
        
    }
}