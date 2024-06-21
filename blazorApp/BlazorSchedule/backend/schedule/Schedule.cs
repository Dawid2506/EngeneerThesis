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
            Console.WriteLine("-------------------------------");
            foreach (var kvp in company.positionsPerDay)
            {
                Console.WriteLine("Day: " + kvp.Key);
                foreach (var position in kvp.Value)
                {
                    Console.WriteLine("Position: " + position);
                }
            }
            Console.WriteLine("-------------------------------");
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
            Console.WriteLine("iiiiiiiiiiiiiiiiiiiiiiiiiiii");
            foreach (var kvp in company.positionsPerDay)
            {
                Console.WriteLine("Day: " + kvp.Key);
                foreach (var position in kvp.Value)
                {
                    Console.WriteLine("Position: " + position);
                }
            }
            Console.WriteLine("iiiiiiiiiiiiiiiiiiiiiiiiiiii");

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
        positions = new List<string>(company.positionsPerDay[day]); // ***Użycie kopii listy***
        Console.WriteLine($"Created copy of positions for day {day}: {string.Join(", ", positions)}");

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
                if (employees[randomEmployee].minHoursUsed >= workingHours)
                {
                    employees[randomEmployee].minHoursUsed -= workingHours;
                    schedule[i, randomEmployee + 1] = positions[x]; // assign position to employee
                    randomEmployeesUsed.Add(randomEmployee); // add randomEmployee to used list
                    // ***Dodano logowanie przed usunięciem pozycji***
                    Console.WriteLine($"Removing position {positions[x]} for employee {employees[randomEmployee].name} on day {actualDate} ({day})");
                    positions.RemoveAt(x); // remove the position from the list
                }
            }
            tryCount++;
            if (tryCount > 20)
            {
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
    List<int> brokenDaysCopy = new List<int>(brokenDays);
    Random random = new Random();

    for (int j = 0; j < brokenDays.Count; j++)
    {
        string dayName = "";
        int day = brokenDays[j] - 1; // Zmniejsz o 1, aby dopasować do indeksu

        if (day < 0 || day >= schedule.GetLength(0))
        {
            Console.WriteLine("Error: day out of range: " + day);
            continue;
        }

        List<Employee> freeEmployees = new List<Employee>(employees); // Kopia listy employees

        int weekDayIndex = day % 7;
        dayName = GetDayOfWeekName(weekDayIndex);

        if (!company.positionsPerDay.ContainsKey(dayName))
        {
            Console.WriteLine($"Key '{dayName}' does not exist in the dictionary.");
            continue;
        }

        List<string> positions = new List<string>(company.positionsPerDay[dayName]); // ***Użycie kopii listy***
        Console.WriteLine($"Created copy of positions for day {dayName}: {string.Join(", ", positions)}");

        Console.WriteLine("Positions for " + dayName + ": " + string.Join(", ", positions));

        int tryCount = 0;
        while (positions.Count > 0)
        {
            int maxMinHoursUsed = 0;
            int employeeWithMaxMinHoursUsed = -1; // Zmieniona wartość początkowa na -1

            for (int i = 0; i < freeEmployees.Count; i++)
            {
                if (freeEmployees[i].minHoursUsed > maxMinHoursUsed)
                {
                    maxMinHoursUsed = freeEmployees[i].minHoursUsed;
                    employeeWithMaxMinHoursUsed = i;
                }
            }

            if (employeeWithMaxMinHoursUsed == -1)
            {
                Console.WriteLine("No suitable employee found for position.");
                break;
            }

            int x = random.Next(0, positions.Count);

            // Logowanie i sprawdzenie zakresu indeksów
            if (x < 0 || x >= positions.Count)
            {
                Console.WriteLine($"Index out of range for positions: {x}");
                continue;
            }

            if (employees[employeeWithMaxMinHoursUsed].positions.Contains(positions[x]) && schedule[day, employeeWithMaxMinHoursUsed + 1] != "/")
            {
                int workingHours = company.CountWorkingHours(dayName);

                // Logowanie i sprawdzenie zakresu indeksów
                if (employeeWithMaxMinHoursUsed < 0 || employeeWithMaxMinHoursUsed >= freeEmployees.Count)
                {
                    Console.WriteLine($"Index out of range for freeEmployees: {employeeWithMaxMinHoursUsed}");
                    continue;
                }

                if (freeEmployees[employeeWithMaxMinHoursUsed].minHoursUsed >= workingHours)
                {
                    schedule[day, employeeWithMaxMinHoursUsed + 1] = positions[x];
                    freeEmployees[employeeWithMaxMinHoursUsed].minHoursUsed -= workingHours;

                    if (employeeWithMaxMinHoursUsed >= 0 && employeeWithMaxMinHoursUsed < freeEmployees.Count)
                    {
                        freeEmployees.RemoveAt(employeeWithMaxMinHoursUsed);
                    }

                    if (x >= 0 && x < positions.Count)
                    {
                        // ***Dodano logowanie przed usunięciem pozycji***
                        Console.WriteLine($"Removing position {positions[x]} for employee {employees[employeeWithMaxMinHoursUsed].name} on day {day + 1} ({dayName})");
                        positions.RemoveAt(x);
                    }

                    // Logowanie przypisania pracownika
                    //Console.WriteLine($"Przypisano {employees[employeeWithMaxMinHoursUsed].name} do {positions[x]} na dzień {day + 1} ({dayName}). Pozostałe godziny: {employees[employeeWithMaxMinHoursUsed].minHoursUsed}");
                }
            }

            tryCount++;
            if (tryCount > 20)
            {
                Console.WriteLine("Error: too many tries at day: " + (day + 1));
                break;
            }
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