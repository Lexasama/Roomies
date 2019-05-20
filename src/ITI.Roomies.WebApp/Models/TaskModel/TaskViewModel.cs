using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Roomies.WebApp.Models.TaskModel
{
    public class TaskViewModel
    {
        [Required]
        public string TaskName { get; set; }

        public string TaskDes { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime TaskDate { get; set; }
        [Required]
        public int collocId { get; set; }

        public int[] roomiesId { get; set; }
    }
}
