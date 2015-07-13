#region

using System.Collections.Generic;
using SharedCode.Tasks.Models;

#endregion

namespace SharedCode.Tasks
{
    public class TasksQueue<Out> where Out : TaskOutput
    {
        private readonly List<TaskPair> tasks;

        public TasksQueue()
        {
            tasks = new List<TaskPair>();
        }

        public void add(ITask<Out> task, TaskInput taskInput)
        {
            tasks.Add(new TaskPair(task,taskInput));
        }
        public void performAll(Out outputToFill)
        {
            foreach (TaskPair tuple in tasks)
            {
                ITask<Out> task = tuple.task;
                TaskInput input = tuple.taskInput;
                task.Execute(input, outputToFill);
            }
        }

        private class TaskPair
        {
            public ITask<Out> task { get; private set; }
            public TaskInput taskInput { get; private set; }

            public TaskPair(ITask<Out> task, TaskInput taskInput)
            {
                this.task = task;
                this.taskInput = taskInput;
            }
        }
    }
}
