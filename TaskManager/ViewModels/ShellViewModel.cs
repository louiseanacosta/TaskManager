using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class ShellViewModel : Screen
    {
        private DateTime currentDate = DateTime.Now;
        public BindableCollection<Task> taskList { get; set; }
        public string ContentNew { get; set; }
        public ShellViewModel()
        {
            taskList = new BindableCollection<Task>
            {

                new Task (1, "Test", false, Convert.ToDateTime("2019-12-12")),
                new Task (2, "Test1", true, Convert.ToDateTime("2019-12-13")),
                new Task (3, "Test2", true, Convert.ToDateTime("2019-12-13")),
                new Task (4, "Test3", false, Convert.ToDateTime("2019-12-13")),
                new Task (5, "Test4", true, Convert.ToDateTime("2019-12-13"))

            };

            //
            //var filePath = "taskmanager.json";
            //// Read existing json data
            //var jsonData = System.IO.File.ReadAllText(filePath);
            //// De-serialize to object or create new list
            //var TaskList = JsonConvert.DeserializeObject<List<Task>>(jsonData)
            //                      ?? new List<Task>();

            //// Add any new employees
            //TaskList.Add(
            //   new Task(1, "Test", false, Convert.ToDateTime("2019-12-12")),
            //   new Task(2, "Test1", true, Convert.ToDateTime("2019-12-13"))
            //   );


            //// Update json data string
            //jsonData = JsonConvert.SerializeObject(TaskList);
            //System.IO.File.WriteAllText(filePath, jsonData);
        }

        public DateTime CurrentDate
        {
            get { return currentDate; }
        }


        public void AddNewTask(string contentNew)
        {
            //taskList.Add(new Task(3, "Added", false, Convert.ToDateTime("2020-5-5"), false));
            if(string.IsNullOrWhiteSpace(ContentNew))
            {
                return;
            }

            taskList.Add(new Task(ContentNew));
            NotifyOfPropertyChange(() => taskList);
        }

        public void DeleteTask()
        {
            //if (SelectedTask == null)
            //{
            //    return;
            //}
            //taskList.Remove(SelectedTask);
            //NotifyOfPropertyChange(() => taskList);

            MessageBox.Show("Working");
        }
    }
}
