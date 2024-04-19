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

    public void InitializeSchedule(int numberOfDays, List<int> workingDaysInt, int firstDayOfMonth)
    {
        schedule = new bool[numberOfDays,5];
        //Console.WriteLine(schedule.GetLength(0));
        
        int actualDay = firstDayOfMonth;
        for(int i = 0; i < numberOfDays; i++)
        {
            for(int j = 0; j < workingDaysInt.Count; j++)
            {
                if(workingDaysInt[j] == actualDay)
                {
                    schedule[i,0] = true;
                }
            }
            if(actualDay == 6)
            {
                actualDay = 0;
            }
            else
            {
                actualDay++;
            }
        }

        
    }

    public void PrintZeroLayerOfSchedule(){
        for(int i = 0; i < schedule.GetLength(0); i++)
        {
            Console.WriteLine(schedule[i,0]);
        }
    }
}