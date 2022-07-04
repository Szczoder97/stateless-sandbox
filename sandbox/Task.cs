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
       public enum State { Open, Assigned, Completed, Accepted, Rejected };
       public enum Trigger { Assign, Complete, Verify, AddTask, Accept, Reject };
       public StateMachine <State, Trigger> Machine;
       public StateMachine<State, Trigger>.TriggerWithParameters<string> SetAsigneeTrigger;
       public string Text { get; set; }
       public string Assignee { get; set; }
    }
}
