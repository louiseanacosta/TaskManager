using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsComplete { get; set; }
        public DateTime DueDate { get; set; }

        public Task(int id, string content, bool isComplete, DateTime dueDate)
        {
            Id = id;
            Content = content;
            IsComplete = isComplete;
            DueDate = dueDate;
        }

        public Task(string content)
        {
            Content = content;
        }

    }
}
