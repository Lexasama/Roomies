using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.CourseViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required]
        public  string CourseName{get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CourseDate{get; set; }

        [Required]
        public int CoursePrice {get; set; }

        [Required]
        public int CollocId { get; set; }

        //[Required]
        //public bool IsTemplate { get; set; }
    }
}
