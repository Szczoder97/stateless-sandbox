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
        public StateMachine<State, Trigger> machine;
        public List<UnitTask> Tasks { get; set; }
        public StateMachine<State, Trigger>.TriggerWithParameters<string> AssingReportTrigger;
        public StateMachine<State, Trigger>.TriggerWithParameters<string> AddTaskTrigger;

        public Report()
        {
            Tasks = new List<UnitTask>();
            machine = new StateMachine<State, Trigger>(State.Open);
            AssingReportTrigger = machine.SetTriggerParameters<string>(Trigger.Assign);
            AddTaskTrigger = machine.SetTriggerParameters<string>(Trigger.AddTask);

            machine.Configure(State.Open)
                .Permit(Trigger.Assign, State.Assigned);
            machine.Configure(State.Assigned)
                .OnEntryFrom(AssingReportTrigger, assignee => OnAssing(assignee))
                .InternalTransition<string>(AddTaskTrigger, (text, t) => OnAddTask(text))
                .PermitIf(Trigger.Verify, State.Verified, () => TasksCompletionCheck());
        }

        public void Assign(string assignee)
        {
            machine.Fire(AssingReportTrigger, assignee);
        }

        public void AddTask(string text)
        {
            machine.Fire(AddTaskTrigger, text);
        }

        public void Verify()
        {
            machine.Fire(Trigger.Verify);
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
            return Tasks.All(t => t.machine.IsInState(Task.State.Completed));
        }

    }
}
