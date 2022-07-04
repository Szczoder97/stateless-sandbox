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

            Machine = new StateMachine<State, Trigger>(State.Open);

            SetAsigneeTrigger = Machine.SetTriggerParameters<string>(Trigger.Assign);
            AddTaskTrigger = Machine.SetTriggerParameters<string>(Trigger.AddTask);

            Machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);
            Machine.Configure(State.Assigned)
                .OnEntryFrom(SetAsigneeTrigger, assignee => OnAssign(assignee))
                .InternalTransition<string>(AddTaskTrigger, (text, t) => OnAddTask(text))
                .PermitIf(Trigger.Accept, State.Accepted, () => DataCompleteCheck())
                .PermitIf(Trigger.Reject, State.Rejected, () => DataCompleteCheck());
            Machine.Configure(State.Accepted)
                .SubstateOf(State.Completed);
            Machine.Configure(State.Rejected)
                .SubstateOf(State.Completed);
        }

        public void Assing(string assignee)
        {
            Machine.Fire(SetAsigneeTrigger, assignee);
        }

        public void Accept()
        {
            Machine.Fire(Trigger.Accept);
        }

        public void Reject()
        {
            Machine.Fire(Trigger.Reject);
        }

        public void AddTask(string text)
        {
            Machine.Fire(AddTaskTrigger, text);
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
            return SubTasks.All(t => t.Machine.IsInState(State.Completed));
        }

    }
}
