using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TaskViewModel : Screen
    {

        private DateTime currentDate = DateTime.Now;
        public BindableCollection<Task> taskList { get; set; }
        public string ContentNew { get; set; }
        private Task selectedTask;
        private string filePath = "taskmanager.json";

        public DateTime CurrentDate
        {
            get { return currentDate; }
        }


        public Task SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
            }
        }

        public TaskViewModel()
        {
            taskList = new BindableCollection<Task>();
            ReadFile();
            
        }

        private void ReadFile()
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string tasksObject = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(tasksObject))
                {
                    return;
                }

                var tasks = JsonConvert.DeserializeObject<List<Task>>(tasksObject);

                foreach (var task in tasks)
                {
                    taskList.Add(task);
                }

            }

        }
        


        public void AddNewTask(string contentNew)
        {
            // check if empty
            if (string.IsNullOrWhiteSpace(ContentNew))
            {
                MessageBox.Show("Please enter a name for the new task","Error");
                return;
            }

           
            var newTask = new Task(ContentNew);
            if(taskList.Count != 0)
            {
                newTask.Id = taskList.Max(x => x.Id) + 1;
            } 
            else
            {
                newTask.Id = 1;
            }
           
            taskList.Add(newTask);
            NotifyOfPropertyChange(() => taskList);
            SaveToFile();

        }
        public void DeleteTask(Task task)
        {
            if (task == null)
            {
                return;
            }
            taskList.Remove(task);
            NotifyOfPropertyChange(() => taskList);
            SaveToFile();
        }

        public void SaveToFile()
        {
            if (taskList == null)
            {
                return;
            }
            string newTaskJson = JsonConvert.SerializeObject(taskList);
            File.WriteAllText(filePath, newTaskJson);
        }

        


    }
}
