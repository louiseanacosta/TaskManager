using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? DueDate { get; set; }

        public TaskModel() { }

        public TaskModel(int id, string content, bool isComplete, DateTime dueDate)
        {
            Id = id;
            Content = content;
            IsComplete = isComplete;
            DueDate = dueDate;
        }

        public TaskModel(string content)
        {
            Content = content;
        }

    }
}
