public class Schedule
{
    private static Schedule instance;
    private static readonly object lockObject = new object();

    private Schedule()
    {
    }

    public static Schedule Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Schedule();
                    }
                }
            }
            return instance;
        }
    }

    public bool[,] schedule { get; set; }

    public void InitializeSchedule(int numberOfDays)
    {
        schedule = new bool[numberOfDays,5];
        Console.WriteLine(schedule.GetLength(0));
    }
}