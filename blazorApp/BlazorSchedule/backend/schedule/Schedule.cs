public class Schedule
{
    private static Schedule instance;
    private static readonly object lockObject = new object();

    private Schedule()
    {
        schedule = new bool[32,5];
        schedule[1, 2] = true;
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
}