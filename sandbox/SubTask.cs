using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandbox
{
    class SubTask : Task
    {
        public string Data { get; set; }
        public StateMachine<State, Trigger>.TriggerWithParameters<string> setDataTrigger;

        public SubTask(string text)
        {
            Text = text;
            machine = new StateMachine<State, Trigger>(State.Open);
            setAsigneeTrigger = machine.SetTriggerParameters<string>(Trigger.Assign);
            setDataTrigger = machine.SetTriggerParameters<string>(Trigger.Complete);

            machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);
            machine.Configure(State.Assigned)
                .OnEntryFrom(setAsigneeTrigger, assignee => OnAssign(assignee))
                .Permit(Trigger.Complete, State.Completed);
            machine.Configure(State.Completed)
                .OnEntryFrom(setDataTrigger, data => OnComplete(data));
        }

        public void Assing(string assignee)
        {
            machine.Fire(setAsigneeTrigger, assignee);
        }

        public void Complete(string data)
        {
            machine.Fire(setDataTrigger, data);
        }
        private void OnAssign(string assignee)
        {
            Assignee = assignee;
        }

        private void OnComplete(string data)
        {
            Data = data;
        }
        

    }
}
