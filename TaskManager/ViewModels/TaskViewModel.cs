using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TaskViewModel : Screen
    {
        public BindableCollection<Task> taskList { get; set; }
        public string ContentNew { get; set; }
        private string filePath = "taskmanager.json"; // file path where list of tasks will be saved

        /// <summary>
        /// Gets the current day of the month
        /// </summary>
        public string DayInt
        {
            get
            {
               return DateTime.Today.Day.ToString();
            }
        }

        /// <summary>
        /// Gets the current day of the week
        /// </summary>
        public string DayNameString
        {
            get
            {
                return DateTime.Today.DayOfWeek.ToString();
            }
        }

        /// <summary>
        ///  Gets the current month and year
        /// </summary>
        public string MonthAndYearString
        {
            get
            {
                var month = DateTime.Now.ToString("MMMM");
                var year = DateTime.Now.Year.ToString();

                return string.Format("{0}, {1}", month, year);
            }
        }

        public TaskViewModel()
        {
            // create an empty task list
            taskList = new BindableCollection<Task>();
            // read json file from json file
            ReadFile();
        }

        /// <summary>
        /// Adds new task to task list and saves to file
        /// </summary>
        /// <param name="contentNew">Content of new task</param>
        public void AddNewTask(string contentNew)
        {
            // check if empty
            if (string.IsNullOrWhiteSpace(ContentNew))
            {
                // display error
                MessageBox.Show("Please enter a name for the new task", "Error");
                return;
            }

            // create new task object
            var newTask = new Task(ContentNew);
            // check if list is empty to assign id to new task
            if (taskList.Count != 0) // list is not empty, current id = last id + 1
            {
                newTask.Id = taskList.Max(x => x.Id) + 1;
            }
            else // if list is empty start with id = 1
            {
                newTask.Id = 1;
            }

            // add to list
            taskList.Add(newTask);
            // update ui
            NotifyOfPropertyChange(() => taskList);
            SaveToFile();
        }

        /// <summary>
        /// Deletes task from the list
        /// </summary>
        /// <param name="task">Task object to be deleted</param>
        public void DeleteTask(Task task)
        {
            // check
            if (task == null)
            {
                return;
            }
            // remove task from list
            taskList.Remove(task);
            // update ui
            NotifyOfPropertyChange(() => taskList);
            SaveToFile();
        }

        /// <summary>
        /// Saves list of tasks to File by first Converting to JSON object
        /// </summary>
        public void SaveToFile()
        {
            // check
            if (taskList == null)
            {
                return;
            }
            // convert to json object
            string newTaskJson = JsonConvert.SerializeObject(taskList);
            // write to file - if file does not exist, create new. if exists, overwrite.
            File.WriteAllText(filePath, newTaskJson);
        }

        /// <summary>
        /// Reads File From JSON file by deserializing JSON from the file to a list
        /// iterate through the objects from the list and add to the task list
        /// </summary>
        private void ReadFile()
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // read file
                string tasksObject = reader.ReadToEnd();
                // check if empty
                if (string.IsNullOrWhiteSpace(tasksObject))
                {
                    return;
                }
                // conver json object to list of task objects
                var tasks = JsonConvert.DeserializeObject<List<Task>>(tasksObject);
                // add each task to task list
                foreach (var task in tasks)
                {
                    taskList.Add(task);
                }
            }
        }





    }
}
