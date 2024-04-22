using BlazorSchedule;

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

    public void InitializeSchedule(int numberOfDays, List<int> workingDaysInt, int firstDayOfMonth, List<Employee> employees)
    {
        int numberOfEmployees = employees.Count;
        schedule = new bool[numberOfDays, numberOfEmployees + 1];
        //Console.WriteLine(schedule.GetLength(0));

        int actualDay = firstDayOfMonth;

        //making the first layer of the schedule with the working days
        for (int i = 0; i < numberOfDays; i++)
        {
            for (int j = 0; j < workingDaysInt.Count; j++)
            {
                if (workingDaysInt[j] == actualDay)
                {
                    schedule[i, 0] = true;
                }
            }
            if (actualDay == 6)
            {
                actualDay = 0;
            }
            else
            {
                actualDay++;
            }
        }

        int actualDate = 1;
        //making the second layer of the schedule with informtation wether the employee can working or not
        for (int i = 0; i < numberOfEmployees; i++)
        {
            for (int j = 0; j < numberOfDays; j++)
            {
                //the numer of employee on the second dimension is equel to the number of the employee from the list + 1
                if (employees[i].daysOff.Contains(actualDate))
                {
                    schedule[j, i + 1] = false;
                }
                else
                {
                    schedule[j, i + 1] = true;
                }
                actualDate++;
            }
            actualDate = 1;
        }


    }

    public void PrintAllSchedule()
    {
        for (int i = 0; i < schedule.GetLength(0); i++)
        {
            Console.Write("Day " + (i + 1));
            for (int j = 0; j < schedule.GetLength(1); j++)
            {
                Console.Write(schedule[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}