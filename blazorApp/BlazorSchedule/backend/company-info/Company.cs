namespace BlazorSchedule{
    public class Company
    {
        public string name { get; set; }
        public string[] workingDays { get; set; }
        public string[] positions { get; set; }
        public Dictionary<int, int> workingHours { get; set; }
        public Dictionary<string, Dictionary<int, int>> workingHoursDay { get; set; }
        public Dictionary<string, int> helpEmployees { get; set; }
         
    }
}