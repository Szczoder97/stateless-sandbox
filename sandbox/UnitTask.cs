using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandbox
{
    class UnitTask : Task
    {
        public List<SubTask> SubTasks { get; set; }
        public bool DataAccepted { get; set; }
        public StateMachine<State, Trigger>.TriggerWithParameters<string> AddTaskTrigger;

        public UnitTask(string text)
        {
            Text = text;
            SubTasks = new List<SubTask>();

            machine = new StateMachine<State, Trigger>(State.Open);

            setAsigneeTrigger = machine.SetTriggerParameters<string>(Trigger.Assign);
            AddTaskTrigger = machine.SetTriggerParameters<string>(Trigger.AddTask);

            machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);
            machine.Configure(State.Assigned)
                .OnEntryFrom(setAsigneeTrigger, assignee => OnAssign(assignee))
                .InternalTransition<string>(AddTaskTrigger, (text, t) => OnAddTask(text))
                .PermitIf(Trigger.Accept, State.Accepted, () => DataCompleteCheck())
                .PermitIf(Trigger.Reject, State.Rejected, () => DataCompleteCheck());
            machine.Configure(State.Accepted)
                .SubstateOf(State.Completed);
            machine.Configure(State.Rejected)
                .SubstateOf(State.Completed);
        }

        public void Assing(string assignee)
        {
            machine.Fire(setAsigneeTrigger, assignee);
        }

        public void Accept()
        {
            machine.Fire(Trigger.Accept);
        }

        public void Reject()
        {
            machine.Fire(Trigger.Reject);
        }

        public void AddTask(string text)
        {
            machine.Fire(AddTaskTrigger, text);
        }
        private void OnAssign(string assignee)
        {
            Assignee = assignee;
        }

        private void OnAddTask(string text)
        {
            SubTasks.Add(new SubTask(text));
        }

        private bool DataCompleteCheck()
        {
            return SubTasks.All(t => t.machine.IsInState(State.Completed));
        }

    }
}
