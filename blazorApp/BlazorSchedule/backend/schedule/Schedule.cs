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
            foreach (var employee in employees)
            {
                employee.minHoursUsed = employee.minHours;
            }
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
            FirstLayerOfSchedule(numberOfDays, workingDaysInt, actualDay);

            // Making second layer of schedule with information about employees
            SecondLayerOfSchedule(numberOfEmployees, employees, numberOfDays);

            // Making third layer of schedule with information about positions
            ThirdLayerOfSchedule(numberOfDays, firstDayOfMonth, employees, company);

            // Making fourth layer of schedule witch adjust the broken days
            FourthLayerOfSchedule(company, employees);

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

        private void FirstLayerOfSchedule(int numberOfDays, List<int> workingDaysInt, int actualDay)
        {
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
        }

        private void SecondLayerOfSchedule(int numberOfEmployees, List<Employee> employees, int numberOfDays)
        {
            int actualDate = 1;
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

        private void ThirdLayerOfSchedule(int numberOfDays, int firstDayOfMonth, List<Employee> employees, Company company)
        {
            int actualDay = firstDayOfMonth;
            int actualDate = 1;
            List<string> positions;
            for (int i = 0; i < numberOfDays; i++)
            {
                ThirdLayerEngine(numberOfDays, firstDayOfMonth, employees, company, i, actualDay, actualDate);
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

            foreach (var employee in employees)
            {
                Console.WriteLine(employee.name);
                Console.WriteLine(employee.minHoursUsed);
            }


            // here i was trying to fix the problem with broken days

            // int fuse = 0;
            // while (brokenDays.Count > 0)
            // {
            //     if (fuse > 50)
            //     {
            //         Console.WriteLine("fuse");
            //         break;
            //     }
            //     foreach (int day in brokenDays)
            //     {
            //         ThirdLayerEngine(numberOfDays, firstDayOfMonth, employees, company, day, actualDay, actualDate);
            //     }
            //     fuse++;
            // }
        }

        private void ThirdLayerEngine(int numberOfDays, int firstDayOfMonth, List<Employee> employees, Company company, int i, int actualDay, int actualDate)
        {
            List<string> positions;
            if (!(schedule[i, 0] == "#"))  //if day is working day
            {
                string day = GetDayOfWeekName(actualDay);
                positions = new List<string>(company.positionsPerDay[day]); // get positions for this day
                //Console.WriteLine("day" + day);
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
                        //becouse of this code sometimes site has crashed
                        employees[randomEmployee].minHoursUsed -= company.CountWorkingHours(GetDayOfWeekName(actualDay)); // decrease minHours of employee
                        Console.WriteLine("employee:" + employees[randomEmployee].name + "minHours: " + company.CountWorkingHours(GetDayOfWeekName(actualDay)));
                        if (employees[randomEmployee].minHoursUsed <= -10)
                        {
                            employees[randomEmployee].minHoursUsed += company.CountWorkingHours(GetDayOfWeekName(actualDay));
                            continue;
                        }

                        schedule[i, randomEmployee + 1] = positions[x]; // assign position to employee
                        randomEmployeesUsed.Add(randomEmployee); // add randomEmployee to used list
                        //Console.WriteLine("randomEmployee: " + randomEmployee + " position: " + positions[x]);
                        positions.Remove(positions[x]); // remove the position from the list
                    }
                    tryCount++;
                    if (tryCount > 20)
                    {
                        //Console.WriteLine("Error: too many tries at day: " + actualDate);
                        brokenDays.Add(actualDate);
                        break;
                    }
                }

                positions.Clear();
                randomEmployeesUsed.Clear();
            }
        }

        private void FourthLayerOfSchedule(Company company, List<Employee> employees)
{
    List<string> positions = new List<string>();
    List<int> brokenDaysCopy = new List<int>(brokenDays);
    for (int j = 0; j < brokenDays.Count; j++)
    {
        string dayName = "";
        int day = brokenDays[j];
        List<Employee> freeEmployees = employees;
        //take day of the week in string
        if (day <= 6 && day >= 0)
        {
            dayName = GetDayOfWeekName(day);
        }
        else if (day <= 13 && day >= 7)
        {
            dayName = GetDayOfWeekName(day - 7);
        }
        else if (day <= 20 && day >= 14)
        {
            dayName = GetDayOfWeekName(day - 14);
        }
        else if (day <= 27 && day >= 21)
        {
            dayName = GetDayOfWeekName(day - 21);
        }
        else if (day <= 34 && day >= 28)
        {
            dayName = GetDayOfWeekName(day - 28);
        }
        else
        {
            Console.WriteLine("Error: day out of range: " + day);
            continue; // Skip processing this day and move to the next iteration
        }

        positions.Clear();
        Console.WriteLine("broken string: " + dayName + "day number" + day);
        
        foreach(var dayy in brokenDaysCopy)
        {
            Console.WriteLine("broken day: " + dayy);
        }

        // Check if the index 'day' is within the bounds of the employees list
        if (day >= 0 && day < employees.Count)
        {
            //taking the position list for the day
            if (company.positionsPerDay.ContainsKey(dayName))
            {
                positions = company.positionsPerDay[dayName]; // get positions for this day
            }
            else
            {
                Console.WriteLine($"Key '{dayName}' does not exist in the dictionary.");
                continue; // Skip processing this day and move to the next iteration
            }

            //started to assign positions to employees
            int tryCount = 0;
            while (positions.Count > 0)
            {
                //finded employee with max minHoursUsed
                int maxMinHoursUsed = 0;
                int employeeWithMaxMinHoursUsed = 0;
                for (int i = 0; i < freeEmployees.Count; i++)
                {
                    if (freeEmployees[i].minHoursUsed > maxMinHoursUsed)
                    {
                        maxMinHoursUsed = freeEmployees[i].minHoursUsed;
                        employeeWithMaxMinHoursUsed = i;
                    }
                }

                int x;
                x = new Random().Next(0, positions.Count);
                // Check if the index 'day' is within the bounds of the schedule array
                if (day >= 0 && day < schedule.GetLength(0))
                {
                    if (employees[employeeWithMaxMinHoursUsed].positions.Contains(positions[x]) && schedule[day, employeeWithMaxMinHoursUsed + 1] != "/")
                    {
                        schedule[day, employeeWithMaxMinHoursUsed + 1] = positions[x];
                        freeEmployees.RemoveAt(employeeWithMaxMinHoursUsed);
                        positions.Remove(positions[x]);
                    }
                }
                else
                {
                    Console.WriteLine("Error: index out of range in schedule array.");
                    break;
                }

                tryCount++;
                if (tryCount > 20)
                {
                    Console.WriteLine("Error: too many tries at day: " + day);
                    break;
                }
            }
        }
        else
        {
            Console.WriteLine("Error: index out of range in employees list.");
        }
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