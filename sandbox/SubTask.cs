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
        public StateMachine<State, Trigger>.TriggerWithParameters<string> SetDataTrigger;

        public SubTask(string text)
        {
            Text = text;
            Machine = new StateMachine<State, Trigger>(State.Open);
            SetAsigneeTrigger = Machine.SetTriggerParameters<string>(Trigger.Assign);
            SetDataTrigger = Machine.SetTriggerParameters<string>(Trigger.Complete);

            Machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);
            Machine.Configure(State.Assigned)
                .OnEntryFrom(SetAsigneeTrigger, assignee => OnAssign(assignee))
                .Permit(Trigger.Complete, State.Completed);
            Machine.Configure(State.Completed)
                .OnEntryFrom(SetDataTrigger, data => OnComplete(data));
        }

        public void Assing(string assignee)
        {
            Machine.Fire(SetAsigneeTrigger, assignee);
        }

        public void Complete(string data)
        {
            Machine.Fire(SetDataTrigger, data);
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
