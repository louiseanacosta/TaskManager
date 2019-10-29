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
                // remove task from list
                taskList.Remove(task);
                SaveToFile();
            }
        }
        #endregion

        #region File Access Methods
        /// <summary>
        /// Saves list of tasks to file as json object
        /// </summary>
        public void SaveToFile()
        {
          
            if (taskList != null) {
                var serializedString = SerializeJson(taskList);
                // write to file - if file does not exist, create new. if exists, overwrite.
                File.WriteAllText(filePath, serializedString);
            }
            
        }

        /// <summary>
        /// Reads File From JSON file 
        /// by deserializing JSON from the file to a list
        /// iterate through the objects from the list and add to the task list
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
                var deserializedList = DeserializeJson(jsonString);
                // add each task to task list
                if (deserializedList.Count > 0)
                {
                    foreach (var task in deserializedList)
                    {
                        taskList.Add(task);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Serializes task list to json object
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        #region Helper Methods
        private static string SerializeJson(BindableCollection<Task> lists)
        {
            var serializedString = string.Empty;

            try
            {
                // convert to json object
                serializedString = JsonConvert.SerializeObject(lists, Formatting.Indented);
                
            }
            catch (Exception)
            {
                MessageBox.Show("Failed To Save", "Error");
            }
            return serializedString;
        }

        /// <summary>
        /// Deserializes JSON string
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns>List<Task></returns>
        private static List<Task> DeserializeJson(string jsonString)
        {
            var deserializedList = new List<Task>();
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    // convert json object to list of task objects
                    deserializedList = JsonConvert.DeserializeObject<List<Task>>(jsonString);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured in JSON Deserialization", "Error");
            }
            return deserializedList;
        }

        #endregion
    }
}
