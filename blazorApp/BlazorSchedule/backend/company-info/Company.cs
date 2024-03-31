namespace BlazorSchedule{
    public class Company
    {
        private static Company instance;
        private static readonly object lockObject = new object();

        public string name { get; set; }
        public List<string> workingDays { get; set; }
        public List<string> positions { get; set; }
        public Dictionary<int, int> workingHours { get; set; }
        public Dictionary<string, Dictionary<int, int>> workingHoursDay { get; set; }

        private Company()
        {
            // Private constructor to prevent instantiation
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
    }
}