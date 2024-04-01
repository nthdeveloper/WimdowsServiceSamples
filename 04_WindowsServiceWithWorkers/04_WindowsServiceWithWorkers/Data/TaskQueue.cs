using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceWithWorkers.Data
{
    internal class TaskQueue
    {
        ConcurrentQueue<TaskItem> _queue;

        public TaskQueue()
        {
            _queue = new ConcurrentQueue<TaskItem>();
        }

        public TaskItem AddTask(string name)
        {
            var task = new TaskItem()
            {
                Id = Guid.NewGuid(),
                Name = name                
            };

            _queue.Enqueue(task);

            return task;
        }        

        public TaskItem GetNextTask()
        {
            if(_queue.TryDequeue(out var task))
                return task;

            return null;
        }
    }
}
