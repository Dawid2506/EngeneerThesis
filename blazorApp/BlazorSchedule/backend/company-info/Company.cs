namespace BlazorSchedule{
    public class Company
    {

        public string shift { get; set; }
        public string name { get; set; }
        public List<string> workingDays { get; set; }
        public Dictionary<string, Dictionary<string, string>> workingHoursDay { get; set; }
        public Dictionary<string, List<string>> positionsPerDay { get; set; }
        public List<string> positionsList { get; set; }
        public List<int> holidays { get; set; }
        public bool isCheckboxChecked { get; set; } = false;

        public Company()
        {
            workingDays = new List<string>();
            positionsPerDay = new Dictionary<string, List<string>>();
            shift = string.Empty;
            name = string.Empty;
            workingHoursDay = new Dictionary<string, Dictionary<string, string>>();
            positionsList = new List<string>();
            holidays = new List<int>();
        }

        public void SetWorkingHoursDay(Dictionary<string, Dictionary<string, string>> workingHoursDay)
        {
            this.workingHoursDay = workingHoursDay;
        }

        public int CountWorkingHours(string day)
        {
            int workingHours = 0;
            Dictionary<string, string> hours = workingHoursDay[day];

            int startHour = int.Parse(hours.Keys.First().Substring(0, 2));
            int endHour = int.Parse(hours.Values.First().Substring(0, 2));

            workingHours = endHour - startHour;

            return workingHours;
        }
    }
}