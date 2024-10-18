namespace YourBlazorProject.Models
{
    public class ScheduleSymbols
    {
        public string holidaySymbol { get; set; } = "#";
        public string notScheduledSymbol { get; set; } = "x";
        public string dayOffSymbol { get; set; } = "/";

        public void ResetToDefault()
        {
            holidaySymbol = "#";
            notScheduledSymbol = "x";
            dayOffSymbol = "/";
        }
    }
}