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
            using (StreamReader reader = new StreamReader("taskmanager.json"))
            {
                string json = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(json))
                {
                    return;
                }

                var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(json);

                foreach (var task in tasks)
                {
                    taskList.Add(task);
                }

            }
        }
        

        public DateTime CurrentDate
        {
            get { return currentDate; }
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
            string newTaskJson = JsonConvert.SerializeObject(taskList);
            //write to file
            File.WriteAllText("taskmanager.json", newTaskJson);

        }

        public void DeleteTask()
        {
            //if (task == null)
            //{ 
            //    return;
            //}
            //taskList.Remove(task);
            //NotifyOfPropertyChange(() => taskList);

            MessageBox.Show("Working");
        }


    }
}
