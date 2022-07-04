using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandbox
{
    class Report
    {
        public string Assignee;
        public enum State { Open, Assigned, Verified };
        public enum Trigger { Assign, AddTask, Verify};
        public StateMachine<State, Trigger> Machine;
        public List<UnitTask> Tasks { get; set; }
        public StateMachine<State, Trigger>.TriggerWithParameters<string> AssingReportTrigger;
        public StateMachine<State, Trigger>.TriggerWithParameters<string> AddTaskTrigger;

        public Report()
        {
            Tasks = new List<UnitTask>();
            Machine = new StateMachine<State, Trigger>(State.Open);
            AssingReportTrigger = Machine.SetTriggerParameters<string>(Trigger.Assign);
            AddTaskTrigger = Machine.SetTriggerParameters<string>(Trigger.AddTask);

            Machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);
            Machine.Configure(State.Assigned)
                .OnEntryFrom(AssingReportTrigger, assignee => OnAssing(assignee))
                .InternalTransition<string>(AddTaskTrigger, (text, t) => OnAddTask(text))
                .PermitIf(Trigger.Verify, State.Verified, () => TasksCompletionCheck());
        }

        public void Assign(string assignee)
        {
            Machine.Fire(AssingReportTrigger, assignee);
        }

        public void AddTask(string text)
        {
            Machine.Fire(AddTaskTrigger, text);
        }

        public void Verify()
        {
            Machine.Fire(Trigger.Verify);
        }

        private void OnAssing(string assignee)
        {
            Assignee = assignee;
        }
        private void OnAddTask(string text)
        {
            Tasks.Add(new UnitTask(text));
        }

        private bool TasksCompletionCheck()
        {
            return Tasks.All(t => t.Machine.IsInState(Task.State.Completed));
        }

    }
}
