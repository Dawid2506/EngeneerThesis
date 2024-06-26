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
        public Dictionary<int, List<string>> brokenDaysPositions;

        public void InitializeSchedule(int numberOfDays, List<int> workingDaysInt, int firstDayOfMonth, List<Employee> employees, Company company)
        {
            // Console.WriteLine("-------------------------------");
            // foreach (var kvp in company.positionsPerDay)
            // {
            //     Console.WriteLine("Day: " + kvp.Key);
            //     foreach (var position in kvp.Value)
            //     {
            //         Console.WriteLine("Position: " + position);
            //     }
            // }
            // Console.WriteLine("-------------------------------");
            foreach (var employee in employees)
            {
                employee.minHoursUsed = employee.minHours;
            }
            brokenDays = new List<int>();
            brokenDaysPositions = new Dictionary<int, List<string>>();
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


            // Console.WriteLine("iiiiiiiiiiiiiiiiiiiiiiiiiiii");
            // foreach (var kvp in company.positionsPerDay)
            // {
            //     Console.WriteLine("Day: " + kvp.Key);
            //     foreach (var position in kvp.Value) 
            //     {
            //         Console.WriteLine("Position: " + position);
            //     }
            // }
            // Console.WriteLine("iiiiiiiiiiiiiiiiiiiiiiiiiiii");

        }

        public void PrintAllSchedule()
        {
            // for (int i = 0; i < schedule.GetLength(0); i++)
            // {
            //     Console.Write("Day " + (i + 1) + " ");
            //     for (int j = 0; j < schedule.GetLength(1); j++)
            //     {
            //         Console.Write(schedule[i, j] + " ");
            //     }
            //     Console.WriteLine();
            // }
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
        }

        private void ThirdLayerEngine(int numberOfDays, int firstDayOfMonth, List<Employee> employees, Company company, int i, int actualDay, int actualDate)
        {
            List<string> positions;
            if (!(schedule[i, 0] == "#"))  //if day is working day
            {
                string day = GetDayOfWeekName(actualDay);
                positions = new List<string>(company.positionsPerDay[day]); // ***Użycie kopii listy***
                //Console.WriteLine($"Created copy of positions for day {day}: {string.Join(", ", positions)}");

                List<int> randomEmployeesUsed = new List<int>();
                Random random = new Random();

                int tryCount = 0;
                while (positions.Count > 0)
                {
                    int randomEmployee;
                    do
                    {
                        randomEmployee = random.Next(0, employees.Count); // get random employee int
                    } while (randomEmployeesUsed.Contains(randomEmployee)); // check if randomEmployee is already used

                    int x;
                    x = random.Next(0, positions.Count);
                    if (employees[randomEmployee].positions.Contains(positions[x]) && schedule[i, randomEmployee + 1] == "x") // if employee has this position and is working
                    {
                        int workingHours = company.CountWorkingHours(day);
                        Console.WriteLine("Working hours: " + workingHours + " for " + day);
                        if (employees[randomEmployee].minHoursUsed >= (workingHours - 1))
                        {
                            employees[randomEmployee].minHoursUsed -= workingHours;
                            schedule[i, randomEmployee + 1] = positions[x]; // assign position to employee
                            randomEmployeesUsed.Add(randomEmployee); // add randomEmployee to used list
                            //Console.WriteLine($"Removing position {positions[x]} for employee {employees[randomEmployee].name} on day {actualDate} ({day})");
                            positions.RemoveAt(x); // remove the position from the list
                        }
                    }
                    tryCount++;
                    if (tryCount > 20)
                    {
                        brokenDays.Add(actualDate);
                        brokenDaysPositions.Add(actualDate, positions);
                        break;
                    }
                }

                positions.Clear();
                randomEmployeesUsed.Clear();
            }
        }

        private void FourthLayerOfSchedule(Company company, List<Employee> employees)
        {
            Employee employeeWithBiggestHoursDifference = SearchEmployeeWithBiggestDiffrence(employees);
            Console.WriteLine("Employee with biggest hours difference: " + employeeWithBiggestHoursDifference.name);

            foreach (var brokenDay in brokenDays)
            {
                Console.WriteLine("Broken day: " + brokenDay);
                List<string> positions = brokenDaysPositions[brokenDay];
                Console.WriteLine("Positions: " + string.Join(", ", positions));
                // List<int> randomEmployeesUsed = new List<int>();
                // Random random = new Random();

                // int tryCount = 0;
                // while (positions.Count > 0)
                // {
                //     int randomEmployee;
                //     do
                //     {
                //         randomEmployee = random.Next(0, employees.Count); // get random employee int
                //     } while (randomEmployeesUsed.Contains(randomEmployee)); // check if randomEmployee is already used

                //     int x;
                //     x = random.Next(0, positions.Count);
                //     if (employees[randomEmployee].positions.Contains(positions[x]) && schedule[brokenDay - 1, randomEmployee + 1] == "x") // if employee has this position and is working
                //     {
                //         int workingHours = company.CountWorkingHours(GetDayOfWeekName(brokenDay - 1));
                //         if (employees[randomEmployee].minHoursUsed >= (workingHours - 1))
                //         {
                //             employees[randomEmployee].minHoursUsed -= workingHours;
                //             schedule[brokenDay - 1, randomEmployee + 1] = positions[x]; // assign position to employee
                //             randomEmployeesUsed.Add(randomEmployee); // add randomEmployee to used list
                //             //Console.WriteLine($"Removing position {positions[x]} for employee {employees[randomEmployee].name} on day {brokenDay}");
                //             positions.RemoveAt(x); // remove the position from the list
                //         }
                //     }
                //     tryCount++;
                //     if (tryCount > 20)
                //     {
                //         break;
                //     }
                // }

                // positions.Clear();
                // randomEmployeesUsed.Clear();
            }
        }

        private Employee SearchEmployeeWithBiggestDiffrence(List<Employee> employees)
        {
            int biggestHourDiffrence = 0;
            Employee employeeWithBiggestHoursDifference = null;
            foreach (var employee in employees)
            {
                if ((employee.minHours - employee.realHoursUsed()) > biggestHourDiffrence)
                {
                    biggestHourDiffrence = employee.minHours - employee.realHoursUsed();
                    employeeWithBiggestHoursDifference = employee;
                }
            }
            return employeeWithBiggestHoursDifference;
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