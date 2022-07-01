using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandbox
{
    abstract class Task
    {
       public enum State { Open, Assigned, Completed, Rejected };
       public enum Trigger { Assign, Complete, Verify, AddTask };
       public StateMachine <State, Trigger> machine;
       public StateMachine<State, Trigger>.TriggerWithParameters<string> setAsigneeTrigger;
       public string Text { get; set; }
       public string Assignee { get; set; }
    }
}
