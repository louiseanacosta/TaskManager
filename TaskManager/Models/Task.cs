/* Task Manager
 * Author: Louise Acosta
 * Date: October 28, 2019
 */

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
        public DateTime? DueDate { get; set; }

        public Task() { }

        public Task(string content)
        {
            Content = content;
        }

        public Task(int id, string content, bool isComplete, DateTime dueDate)
        {
            Id = id;
            Content = content;
            IsComplete = isComplete;
            DueDate = dueDate;
        }
    }
}
