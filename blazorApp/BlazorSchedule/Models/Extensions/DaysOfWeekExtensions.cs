namespace YourBlazorProject.Models
{
    public static class DaysOfWeekExtensions
    {
        public static string ToFriendlyString(this DaysOfWeek day)
        {
            return day.ToString();
        }
    }
}