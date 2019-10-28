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
    public class ShellViewModel : Screen
    {

        private DateTime currentDate = DateTime.Now;
        public BindableCollection<TaskModel> taskList { get; set; }
        public string ContentNew { get; set; }
        private TaskModel selectedTask;
        private string filePath = "taskmanager.json";

        public DateTime CurrentDate
        {
            get { return currentDate; }
        }


        public TaskModel SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
            }
        }

        public ShellViewModel()
        {
            Load();
        }

        private void Load()
        {

            taskList = new BindableCollection<TaskModel>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string tasksObject = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(tasksObject))
                {
                    return;
                }

                var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(tasksObject);

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

           
            var newTask = new TaskModel(ContentNew);
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
            SaveToFile(taskList);

        }
        public void DeleteTask(TaskModel task)
        {
            if (task == null)
            {
                return;
            }
            taskList.Remove(task);
            NotifyOfPropertyChange(() => taskList);
            SaveToFile(taskList);
        }

        private void SaveToFile(BindableCollection<TaskModel> list)
        {
            string newTaskJson = JsonConvert.SerializeObject(list);
            File.WriteAllText(filePath, newTaskJson);
        }


    }
}
