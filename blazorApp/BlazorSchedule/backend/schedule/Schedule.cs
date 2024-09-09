using System;
using System.Collections.Generic;
using Blazorise.Extensions;
using YourBlazorProject.Models;

namespace BlazorSchedule
{
    public class Schedule
    {
        private static Schedule instance { get; set; }
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

        public string[,] schedule { get; set; }
        public List<int> brokenDays { get; set; }
        public Dictionary<int, List<string>> brokenDaysPositions { get; set; }
        public string month { get; set; }
        public string year { get; set; }

        public void InitializeSchedule(int numberOfDays, List<int> workingDaysInt, int firstDayOfMonth, List<Employee> employees, Company company)
        {
            foreach (var employee in employees)
            {
                employee.minHoursUsed = employee.minHours;
            }
            brokenDays = new List<int>();
            brokenDaysPositions = new Dictionary<int, List<string>>();
            int numberOfEmployees = employees.Count;
            schedule = new string[numberOfDays, numberOfEmployees + 2];

            int actualDay = firstDayOfMonth;

            for (int i = 0; i < schedule.GetLength(0); i++)
            {
                for (int j = 0; j < schedule.GetLength(1); j++)
                {
                    schedule[i, j] = ScheduleCellContent.holiday.ToSymbol();
                }
            }

            // Making first layer of schedule with information about working days
            FirstLayerOfSchedule(numberOfDays, workingDaysInt, actualDay);

            // Making second layer of schedule with information about employees
            SecondLayerOfSchedule(numberOfEmployees, employees, numberOfDays);

            // Making third layer of schedule with information about positions
            //ThirdLayerOfSchedule(numberOfDays, firstDayOfMonth, employees, company);
            ThirdLayerOfScheduleEmployee(numberOfDays, firstDayOfMonth, employees, company, numberOfEmployees);

            // Making fourth layer of schedule witch adjust the broken days
            //FourthLayerOfSchedule(company, employees);
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
                    if (schedule[j, 0] == ScheduleCellContent.holiday.ToSymbol())
                    {
                        schedule[j, i + 2] = ScheduleCellContent.holiday.ToSymbol();
                    }
                    else if (employees[i].daysOff.Contains(actualDate))
                    {
                        schedule[j, i + 2] = ScheduleCellContent.dayOff.ToSymbol();
                    }
                    else
                    {
                        schedule[j, i + 2] = ScheduleCellContent.notScheduled.ToSymbol();
                    }
                    actualDate++;
                }
                actualDate = 1;
            }
        }

        private void ThirdLayerOfScheduleEmployee(int numberOfDays, int firstDayOfMonth, List<Employee> employees, Company company, int numberOfEmployees)
        {

            // Sort employees by typeOfAgreement
            List<Employee> employeesWithAgreement = employees.Where(employee => employee.typeOfAgreement == AgreementType.mandate).ToList();
            List<Employee> employeesWithoutAgreement = employees.Where(employee => employee.typeOfAgreement != AgreementType.contract).ToList();
            List<Employee> sortedEmployees = employeesWithAgreement.Concat(employeesWithoutAgreement).ToList();

            // foreach (var employee in sortedEmployees)
            // {
            //     Console.WriteLine("Employee: " + employee.name + " " + employee.typeOfAgreement);
            // }

            //<number of day of month, mon of day of week>
            Dictionary<int, int> daysOfWeek = new Dictionary<int, int>();

            //generate dictionary daysOfWeek
            int dayOfWeek = firstDayOfMonth;
            for (int i = 1; i <= numberOfDays; i++)
            {
                //add the day of the week string
                schedule[i - 1, 1] = GetDayOfWeekName(dayOfWeek);
                daysOfWeek.Add(i, dayOfWeek);

                if (dayOfWeek == 6)
                {
                    dayOfWeek = 0;
                }
                else
                {
                    dayOfWeek++;
                }
            }

            // // Console write whole daysofweek
            // foreach (var kvp in daysOfWeek)
            // {
            //     Console.WriteLine("Day: " + kvp.Key + " Day of Week: " + kvp.Value);
            // }

            //copy positions per day
            Dictionary<int, List<string>> positionsPerDay = new Dictionary<int, List<string>>();
            // foreach (var entry in company.positionsPerDay)
            // {
            //     positionsPerDay[entry.Key] = new List<string>(entry.Value);
            // }

            for (int i = 1; i <= numberOfDays; i++)
            {
                positionsPerDay[i] = new List<string>();
            }

            dayOfWeek = firstDayOfMonth;
            for (int i = 1; i <= numberOfDays; i++)
            {

                string DayOfWeekString = GetDayOfWeekName(dayOfWeek);
                positionsPerDay[i].AddRange(company.positionsPerDay[DayOfWeekString]);

                if (dayOfWeek == 6)
                {
                    dayOfWeek = 0;
                }
                else
                {
                    dayOfWeek++;
                }
            }

            foreach (var employee in sortedEmployees)
            {
                List<int> randomDaysUsed = new List<int>();
                int attemptCounter = 0;
                int maxAttempts = 500;

                while (employee.minHours > employee.realHoursUsed() && attemptCounter < maxAttempts)
                {
                    attemptCounter++;
                    //Console.WriteLine("Employee: " + employee.name + " " + employee.minHoursUsed);

                    Random random = new Random();

                    int randomDay = random.Next(1, numberOfDays + 1);
                    int randomDayOfWeekInt = daysOfWeek[randomDay];
                    string randomDayOfWeek = GetDayOfWeekName(randomDayOfWeekInt);

                    if (randomDaysUsed.Contains(randomDay))
                    {
                        continue;
                    }
                    if (!positionsPerDay.ContainsKey(randomDay) || positionsPerDay[randomDay].Count == 0)
                    {
                        continue; // Skip if no positions available for the day
                    }
                    randomDaysUsed.Add(randomDay);

                    List<string> positions = positionsPerDay[randomDay];

                    int employeeIdx = EmployeeIndexByName(employee.name, employees);

                    //Console.WriteLine("positionPerDay: " + positionsPerDay["poniedziałek"][0]);

                    //Console.WriteLine("Employee: " + employee.name + "employeeIdx " + employeeIdx + "employee.positions.Count " + employee.positions.Count + "string.Join( , employee.positions) " + string.Join(", ", employee.positions) + "string.Join(, positionsPerDay[randomDayOfWeek]) " + string.Join(", ", positionsPerDay[randomDayOfWeek]) + "randomDay " + randomDay + "randomDayOfWeek " + randomDayOfWeek + "randomDayOfWeekInt " + randomDayOfWeekInt + "schedule[randomDay, employeeIdx + 1] " + schedule[randomDay - 1, employeeIdx + 1] + "employee.daysOff.Contains(randomDay) " + employee.daysOff.Contains(randomDay) + "employee.positions.Intersect(positionsPerDay[randomDayOfWeek]).Any() " + employee.positions.Intersect(positionsPerDay[randomDayOfWeek]).Any() + "employee.minHoursUsed " + employee.minHoursUsed + " " + company.CountWorkingHours(randomDayOfWeek) + " " + employee.minHours + " " + employee.realHoursUsed());

                    if (schedule[randomDay - 1, employeeIdx + 2] == ScheduleCellContent.notScheduled.ToSymbol() && !employee.daysOff.Contains(randomDay) && employee.positions.Intersect(positionsPerDay[randomDay]).Any())
                    {
                        int workingHours = company.CountWorkingHours(randomDayOfWeek);
                        employee.minHoursUsed -= workingHours;
                        foreach (var employeePosition in employee.positions)
                        {
                            if (positionsPerDay[randomDay].Contains(employeePosition))
                            {
                                schedule[randomDay - 1, employeeIdx + 2] = employeePosition;
                                //teraz usunac ta pozycje z listy i dodac break
                                positionsPerDay[randomDay].Remove(employeePosition);
                                break;
                            }
                        }
                    }

                    if (attemptCounter >= maxAttempts)
                    {
                        Console.WriteLine("Employee: " + employee.name);
                    }
                }
            }

            //broken day
            for (int i = 1; i <= numberOfDays; i++)
            {
                if (!positionsPerDay[i].IsNullOrEmpty())
                {
                    brokenDays.Add(i);
                    brokenDaysPositions.Add(i, positionsPerDay[i]);
                }
            }
        }

        private int EmployeeIndexByName(string name, List<Employee> employees)
        {
            int index = employees.FindIndex(e => e.name == name);
            return index;
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
            if (!(schedule[i, 0] == ScheduleCellContent.holiday.ToSymbol()))  //if day is working day
            {
                string day = GetDayOfWeekName(actualDay);
                positions = new List<string>(company.positionsPerDay[day]);
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
                    if (employees[randomEmployee].positions.Contains(positions[x]) && schedule[i, randomEmployee + 1] == ScheduleCellContent.notScheduled.ToSymbol()) // if employee has this position and is working
                    {
                        int workingHours = company.CountWorkingHours(day);
                        if (employees[randomEmployee].minHoursUsed >= (workingHours - 10))
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

            foreach (var brokenDay in brokenDays)
            {
                List<string> positions = brokenDaysPositions[brokenDay];
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
            DaysOfWeek day = dayOfWeek switch
            {
                0 => DaysOfWeek.Sunday,
                1 => DaysOfWeek.Monday,
                2 => DaysOfWeek.Tuesday,
                3 => DaysOfWeek.Wednesday,
                4 => DaysOfWeek.Thursday,
                5 => DaysOfWeek.Friday,
                6 => DaysOfWeek.Saturday,
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Invalid day of the week")
            };

            return day.ToFriendlyString();
        }
    }
}