using YourBlazorProject.Models;

namespace BlazorSchedule
{
    public class Employee
    {
        public string name { get; set; }
        public AgreementType typeOfAgreement { get; set; }
        public int minHours { get; set; }
        public int minHoursUsed { get; set; }
        public List<int> daysOff { get; set; }
        public int SelectedDayOff { get; set; }
        public List<string> positions { get; set; }

        public int realHoursUsed()
        {
            return minHours - minHoursUsed;
        }

        public bool IsAvailableOn(int date)
        {
            return !daysOff.Contains(date);
        }

        public Employee(string name, AgreementType typeOfAgreement, int minHours, List<string> positions)
        {
            this.name = name;
            this.typeOfAgreement = typeOfAgreement;
            this.minHours = minHours;
            this.positions = positions;
            daysOff = new List<int>();
        }
    }
}