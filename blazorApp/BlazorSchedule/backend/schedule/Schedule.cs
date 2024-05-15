using System;
using System.Collections.Generic;

namespace BlazorSchedule
{
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

        public string[,] schedule;
        public List<int> brokenDays;

        public void InitializeSchedule(int numberOfDays, List<int> workingDaysInt, int firstDayOfMonth, List<Employee> employees, Company company)
        {
            brokenDays = new List<int>();
            int numberOfEmployees = employees.Count;
            schedule = new string[numberOfDays, numberOfEmployees + 1];

            int actualDay = firstDayOfMonth;

            for (int i = 0; i < schedule.GetLength(0); i++)
            {
                for (int j = 0; j < schedule.GetLength(1); j++)
                {
                    schedule[i, j] = "#";
                }
            }

            // Making first layer of schedule with information about working days
            for (int i = 0; i < numberOfDays; i++)
            {
                for (int j = 0; j < workingDaysInt.Count; j++)
                {
                    if (workingDaysInt[j] == actualDay)
                    {
                        schedule[i, 0] = "true";
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
            // Making second layer of schedule with information about employees
            for (int i = 0; i < numberOfEmployees; i++)
            {
                for (int j = 0; j < numberOfDays; j++)
                {
                    if (schedule[j, 0] == "#")
                    {
                        schedule[j, i + 1] = "#";
                    }
                    else if (employees[i].daysOff.Contains(actualDate))
                    {
                        schedule[j, i + 1] = "/";
                    }
                    else
                    {
                        schedule[j, i + 1] = "x";
                    }
                    actualDate++;
                }
                actualDate = 1;
            }

            actualDay = firstDayOfMonth;
            actualDate = 1;
            List<string> positions;
            for (int i = 0; i < numberOfDays; i++)
            {
                if (!(schedule[i, 0] == "#"))  //if day is working day
                {
                    string day = GetDayOfWeekName(actualDay);
                    positions = new List<string>(company.positionsPerDay[day]); // get positions for this day
                    Console.WriteLine("day" + day);
                    List<int> randomEmployeesUsed = new List<int>();

                    int tryCount = 0;
                    while (positions.Count > 0)
                    {
                        int randomEmployee;
                        do
                        {
                            randomEmployee = new Random().Next(0, employees.Count); // get random employee int
                        } while (randomEmployeesUsed.Contains(randomEmployee)); // check if randomEmployee is already used

                        int x;
                        x = new Random().Next(0, positions.Count);
                        if (employees[randomEmployee].positions.Contains(positions[x]) && schedule[i, randomEmployee + 1] == "x") // if employee has this position and is working
                        {
                            schedule[i, randomEmployee + 1] = positions[x]; // assign position to employee
                            randomEmployeesUsed.Add(randomEmployee); // add randomEmployee to used list
                            Console.WriteLine("randomEmployee: " + randomEmployee + " position: " + positions[x]);
                            positions.Remove(positions[x]); // remove the position from the list
                        }
                        tryCount++;
                        if (tryCount > 20)
                        {
                            Console.WriteLine("Error: too many tries at day: " + actualDate);
                            brokenDays.Add(actualDate);
                            break;
                        }
                    }

                    positions.Clear();
                    randomEmployeesUsed.Clear();
                }
                actualDate++;
                if (actualDay == 6)
                {
                    actualDay = 0;
                }
                else
                {
                    actualDay++;
                }
            }
        }

        public void PrintAllSchedule()
        {
            for (int i = 0; i < schedule.GetLength(0); i++)
            {
                Console.Write("Day " + (i + 1) + " ");
                for (int j = 0; j < schedule.GetLength(1); j++)
                {
                    Console.Write(schedule[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public string GetDayOfWeekName(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 0:
                    return "niedziela";
                case 1:
                    return "poniedziałek";
                case 2:
                    return "wtorek";
                case 3:
                    return "środa";
                case 4:
                    return "czwartek";
                case 5:
                    return "piątek";
                case 6:
                    return "sobota";
                default:
                    return "error";
            }
        }
    }
}