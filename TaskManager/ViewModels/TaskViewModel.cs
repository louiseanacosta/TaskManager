/* Task Manager
 * Author: Louise Acosta
 * Date: October 28, 2019
 */

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
        private BindableCollection<Task> taskList; 
        private string contentNew; 
        private string filePath = "taskmanager.json"; // file path where list of tasks will be saved

        // list of tasks getter and setter
        public BindableCollection<Task> TaskList
        {
            get { return taskList; }
            set
            {
                if (taskList != value)
                {
                    taskList = value;
                }
                NotifyOfPropertyChange(() => taskList);

            }
        }

        // content for new task getter and setter
        public string ContentNew
        {
            get { return contentNew; }
            set
            {
                if (contentNew != value)
                {
                    contentNew = value;
                }
                NotifyOfPropertyChange(() => contentNew);
            }
        }

        // constructor
        public TaskViewModel()
        {
            // create an empty task list
            taskList = new BindableCollection<Task>();
            // read json file from json file
            ReadFile();
        }


        /// <summary>
        /// Get current date
        /// </summary>
        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        
        #region Data Manipulation Methods
        /// <summary>
        /// Adds new task to task list and saves to file
        /// </summary>
        /// <param name="contentNew">Content of new task</param>
        public void AddNewTask(string contentNew)
        {
            // check if empty
            if (string.IsNullOrWhiteSpace(ContentNew))
            {
                // display error message
                MessageBox.Show("Please enter a name for the new task", "Error");
                return;
            }

            // create new task object
            var newTask = new Task(ContentNew);
            // assign id to new task
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
            SaveToFile();
            ContentNew = string.Empty;
        }

        /// <summary>
        /// Deletes task from the list
        /// </summary>
        /// <param name="task">Task object to be deleted</param>
        public void DeleteTask(Task task)
        {
            if(task != null)
            {
                try
                {
                    // remove task from list
                    taskList.Remove(task);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error deleting task", "Error");
                }
                SaveToFile();
            }
        }
        #endregion

        #region File Access Methods
        /// <summary>
        /// Saves list of tasks to File as JSON object
        /// </summary>
        public void SaveToFile()
        {
            if (taskList != null)
            {
                try
                {
                    // convert to json object
                    string serializedString = JsonConvert.SerializeObject(taskList, Formatting.Indented);
                    // write to file - if file does not exist, create new. if exists, overwrite.
                    File.WriteAllText(filePath, serializedString);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed To Save", "Error");
                }
            }
            
        }

        /// <summary>
        /// Reads list of tasks from JSON file
        /// </summary>
        private void ReadFile()
        {
            // check if file exists
            if (!File.Exists(filePath))
            {
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                // read file
                string jsonString = reader.ReadToEnd();
                // check if empty
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    return;
                }

                try
                {
                    // convert json object to list of task objects
                    var deserializedList = JsonConvert.DeserializeObject<List<Task>>(jsonString);
                    // add each task to task list
                    foreach (var task in deserializedList)
                    {
                        taskList.Add(task);
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Error Reading File", "Error");
                    return;
                }
            }
        }
        #endregion
    }
}
