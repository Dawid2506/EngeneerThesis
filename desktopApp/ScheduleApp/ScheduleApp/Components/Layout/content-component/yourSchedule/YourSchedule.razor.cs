using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBlazorProject.Models;
using OfficeOpenXml;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using BlazorSchedule;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;


namespace ScheduleApp.Components.Layout.content_component.yourSchedule
{
    public partial class YourSchedule
    {
        private int holiday { get; set; }
        //private List<int> holidays { get; set; } = new List<int>();
        private string? errorMessage { get; set; }
        private int month { get; set; }
        private int year { get; set; }
        private Dictionary<string, int> workingDaysToInt { get; set; } = new Dictionary<string, int>();
        private bool scheduleIsVisible { get; set; } = false;
        private int NumOfDays { get; set; }
        private List<int> daysToColor { get; set; } = new List<int>();

        protected override void OnInitialized()
        {
            //holidays = appState.CompanyInstance.holidays;
            month = 4;
            year = 2024;
        }

        private void AddHoliday(int holiday)
        {
            if (holiday <= 0 || holiday >= 32) return;

            appState.CompanyInstance.holidays.Add(holiday);
            holiday = 0;
        }

        // private void MakeScheduleGenerator()
        // {
        //     DateTime date = new DateTime(year, month, 1);
        //     int NumOfDaysInApril = DateTime.DaysInMonth(date.Year, date.Month);
        //     List<Employee> employees = appState.EmployeesRepository.employees;

        //     int tryCount = 0;
        //     int maxTryCount = 1;
        //     int minBrokenDays = 0;
        //     string[,] bestSchedule = new string[NumOfDaysInApril, employees.Count + 2];
        //     // while (tryCount < maxTryCount)
        //     // {
        //     //     tryCount++;
        //     //     MakeSchedule(NumOfDaysInApril, employees);
        //     //     if (minBrokenDays < appState.schedule.brokenDays.Count())
        //     //     {
        //     //         minBrokenDays = appState.schedule.brokenDays.Count();
        //     //         bestSchedule = appState.schedule.schedule;
        //     //     }
        //     // }
        //     MakeSchedule(NumOfDaysInApril, employees);
        //     appState.schedule.schedule = appState.schedule.schedule;
        // }
        private void MakeSchedule()
        {
            if (month == 0 || year == 0)
            {
                errorMessage = "Provide month and year";
                return;
            }
            else
            {
                errorMessage = null;
            }

            DateTime date = new DateTime(year, month, 1);
            int NumOfDaysInApril = DateTime.DaysInMonth(date.Year, date.Month);
            List<Employee> employees = appState.EmployeesRepository.employees;
            DateTime firstDayOfApril = new DateTime(date.Year, date.Month, 1);
            DayOfWeek startDayOfWeek = firstDayOfApril.DayOfWeek;
            int FirstDayOfMonth = (int)startDayOfWeek;

            List<int> workingDaysInt = MakeNumberOfDaysDictionary();
            Company company = appState.CompanyInstance;
            ScheduleSymbols scheduleSymbols = appState.scheduleSymbols;

            appState.schedule.InitializeSchedule(NumOfDaysInApril, workingDaysInt, FirstDayOfMonth, employees, company, scheduleSymbols);

            appState.schedule.month = month.ToString();
            appState.schedule.year = year.ToString();

            scheduleIsVisible = true;
            NumOfDays = NumOfDaysInApril;

            daysToColor = appState.schedule.brokenDays;
        }

        private string GetListAsString(int key, Dictionary<int, List<string>> brokenDaysPositions)
        {
            if (brokenDaysPositions.TryGetValue(key, out var list))
            {
                return string.Join(", ", list);
            }
            return string.Empty;
        }

        private List<int> MakeNumberOfDaysDictionary()
        {
            List<int> workingDaysInt = new List<int>();

            workingDaysToInt = new Dictionary<string, int>();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                workingDaysToInt.Add(day.ToString(), (int)day);
            }

            foreach (string day in appState.CompanyInstance.workingDays)
            {
                workingDaysInt.Add(workingDaysToInt[day]);
            }

            return workingDaysInt;
        }

        private async Task GenerateExcel()
        {
            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Arkusz1");

                    // Pobieranie danych
                    string[,] schedule = appState.schedule.schedule;
                    List<Employee> employees = appState.EmployeesRepository.employees;

                    string year = appState.schedule.year;
                    string month = appState.schedule.month;
                    string monthYear = $"{month}.{year}";
                    worksheet.Cells["B1"].Value = monthYear;

                    // Nagłówki kolumn z nazwami pracowników
                    for (int i = 0; i < employees.Count; i++)
                    {
                        string cellAddress = $"{(char)('A' + i + 2)}1";
                        worksheet.Cells[cellAddress].Value = employees[i].name;
                    }

                    // Wypełnianie tabeli danymi
                    for (int i = 0; i < schedule.GetLength(0); i++)
                    {
                        worksheet.Cells[$"A{i + 2}"].Value = i + 1; // Numer dnia

                        for (int j = 1; j < schedule.GetLength(1); j++)
                        {
                            string cellAddress = $"{(char)('A' + j)}{i + 2}";
                            worksheet.Cells[cellAddress].Value = schedule[i, j];
                        }
                    }

                    // Zapis pliku Excel w lokalnym systemie plików
                    var filePath = Path.Combine(FileSystem.AppDataDirectory, "GeneratedSchedule.xlsx");
                    await File.WriteAllBytesAsync(filePath, package.GetAsByteArray());

                    // Powiadomienie użytkownika o zapisaniu pliku
                    Console.WriteLine($"Plik Excel został zapisany: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas generowania pliku Excel: {ex.Message}");
            }
        }

        private Modal modalRef;

        private void ResetSymbols()
        {
            // Przywróć domyślne wartości symboli
            appState.scheduleSymbols.ResetToDefault();
        }

        private void OnShowModalClick()
        {
            modalRef.Show();
        }

        private void OnHideModalClick()
        {
            modalRef.Hide();
        }

        private void OnEnterKeyPress(KeyboardEventArgs e, int holiday)
        {
            if (e.Key == "Enter")
            {
                AddHoliday(holiday);
            }
        }
    }
}