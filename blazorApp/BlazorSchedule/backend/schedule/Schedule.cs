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

        public string[,] schedule; // Zmieniamy na pole

        public void InitializeSchedule(int numberOfDays, List<int> workingDaysInt, int firstDayOfMonth, List<Employee> employees)
        {
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

            // Tworzenie pierwszej warstwy harmonogramu z dniami roboczymi
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
            // Tworzenie drugiej warstwy harmonogramu z informacją czy pracownik może pracować czy nie
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
    }
}
