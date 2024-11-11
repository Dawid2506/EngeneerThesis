namespace YourBlazorProject.Models
{
    public static class ScheduleCellContentExtensions
    {
        public static string ToSymbol(this ScheduleCellContent content, ScheduleSymbols symbols)
        {
            return content switch
            {
                ScheduleCellContent.holiday => symbols.holidaySymbol,
                ScheduleCellContent.notScheduled => symbols.notScheduledSymbol,
                ScheduleCellContent.dayOff => symbols.dayOffSymbol,
                _ => throw new ArgumentOutOfRangeException(nameof(content), content, null)
            };
        }
    }
}