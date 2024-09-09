namespace YourBlazorProject.Models
{
    public static class ScheduleCellContentExtensions
    {
        public static string ToSymbol(this ScheduleCellContent content)
        {
            return content switch
            {
                ScheduleCellContent.holiday => "#",
                ScheduleCellContent.notScheduled => "x",
                ScheduleCellContent.dayOff => "/",
                _ => throw new ArgumentOutOfRangeException(nameof(content), content, null)
            };
        }
    }
}