using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Daemon
{
    public class TaskHandler
    {
        public List<DbTask> Tasks { get; set; }
        private List<Timer> timers = new List<Timer>();

        public void CreateTimers(bool clear = true)
        {
            timers.Clear();
            foreach (var task in Tasks)
            {
                foreach (var taskLocation in task.taskLocations)
                {
                    foreach (var time in taskLocation.times)
                    {
                        var timer = new System.Threading.Timer((e) =>
                        {
                            var action = DecideAction();
                            action();
                        }, null, time.startTime.TimeOfDay, new TimeSpan(0,0,0,(int)time.interval,0));
                    }
                }
                
            }
            
        }

        //TODO : Finish
        private Action DecideAction()
        {
            return PlaceHolderMethod;
        }
        //TODO : Finish
        private void PlaceHolderMethod()
        {
            Console.WriteLine(DateTime.Now+": PLACE HOLDER 11234786321");
        }

    }
}
