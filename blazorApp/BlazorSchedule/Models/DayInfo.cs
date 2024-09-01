namespace YourBlazorProject.Models
{
    public class DayInfo
    {
        public required string Day { get; set; }
        public required string WorkingHours { get; set; }
        public required List<string> Positions { get; set; }
    }
}