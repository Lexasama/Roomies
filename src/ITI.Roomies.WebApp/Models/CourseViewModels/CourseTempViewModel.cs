using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.CourseTempViewModels
{
        public class CourseTempViewModel
        {
            public int CourseId { get; set; }

            [Required]
            public string CourseName { get; set; }

            [Required]
            public int CoursePrice { get; set; }

            [Required]
            public int CollocId { get; set; }
        }
}
