namespace BlazorSchedule{
    public class Company
    {
        private static Company instance;
        private static readonly object lockObject = new object();

        public string name { get; set; }
        public List<string> workingDays { get; set; }
        public Dictionary<string, Dictionary<string, string>> workingHoursDay { get; set; }
        public Dictionary<string, List<string>> positionsPerDay { get; set; }
        public List<string> positionsList { get; set; }
        public bool isCheckboxChecked { get; set; } = false;

        private Company()
        {
            workingDays = new List<string>();
        }

        public void SetWorkingHoursDay(Dictionary<string, Dictionary<string, string>> workingHoursDay)
        {
            this.workingHoursDay = workingHoursDay;
        }

        public static Company Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new Company();
                        }
                    }
                }
                return instance;
            }
        }

        public int CountWorkingHours(string day)
        {
            int workingHours = 0;
            Dictionary<string, string> hours = workingHoursDay[day];
            DateTime startHourForAll = DateTime.Parse(workingHoursDay.Values.First().Keys.First());
            DateTime endHourForAll = DateTime.Parse(workingHoursDay.Values.First().Values.First());

            TimeSpan duration = endHourForAll - startHourForAll;
            workingHours = (int)duration.TotalHours;

            return workingHours;
        }
    }
}