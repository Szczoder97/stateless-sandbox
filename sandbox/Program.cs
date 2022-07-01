using System;

namespace sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var report = new Report();
            // assigning worker to the report and setting tasks for subunits
            report.Assign("Pracownik LBa");
            report.AddTask("Zadanie 1");
            report.AddTask("Zadanie 2");
            var zad1 = report.Tasks[0];
            var zad2 = report.Tasks[1];
            // operations on first task
            zad1.Assing("Odział");
            zad1.AddTask("zadanie A");
            zad1.AddTask("zadanie B");
            var zadA = zad1.SubTasks[0];
            var zadB = zad1.SubTasks[1];
            zadA.Assing("pracownik komórki meryotrycznej");
            zadA.Complete("zrobiłem robotę");
            zadB.Assing("inny pracownik");
            zadB.Complete("brak wyników");
            zad1.Complete(true); // possible only if all subtasks are completed
            // another task for different unit 
            zad2.Assing("Spółka");
            zad2.AddTask("Zadanie C");
            var zadC = zad2.SubTasks[0];
            zadC.Assing("Pracownik spółki");
            zadC.Complete("złe wyniki");
            zad2.Complete(false);
            // passing whole report to verification
            report.Verify();
        }
    }
}
