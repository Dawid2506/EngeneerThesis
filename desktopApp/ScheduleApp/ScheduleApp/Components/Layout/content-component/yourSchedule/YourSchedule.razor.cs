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
using ClosedXML.Excel;
using System.IO;


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
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Arkusz1");

                    // Pobieranie danych
                    string[,] schedule = appState.schedule.schedule;
                    List<Employee> employees = appState.EmployeesRepository.employees;

                    string year = appState.schedule.year;
                    string month = appState.schedule.month;
                    string monthYear = $"{month}.{year}";
                    worksheet.Cell(1, 2).Value = monthYear;

                    // Nagłówki kolumn z nazwami pracowników
                    for (int i = 0; i < employees.Count; i++)
                    {
                        worksheet.Cell(1, i + 3).Value = employees[i].name;
                    }

                    // Wypełnianie tabeli z numerami dni
                    for (int i = 0; i < schedule.GetLength(0); i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = i + 1; // Numer dnia
                        for (int j = 1; j < schedule.GetLength(1); j++)
                        {
                            worksheet.Cell(i + 2, j + 1).Value = schedule[i, j];
                        }
                    }

                    // Dodatkowe informacje o pracownikach (nazwa, godziny)
                    int employeeInfoPlaceX = schedule.GetLength(1) + 2;
                    for (int i = 0; i < employees.Count; i++)
                    {
                        worksheet.Cell(i + 2, employeeInfoPlaceX).Value = employees[i].name;
                        worksheet.Cell(i + 2, employeeInfoPlaceX + 1).Value = employees[i].realHoursUsed();
                        worksheet.Cell(i + 2, employeeInfoPlaceX + 2).Value = employees[i].minHours;
                    }

                    // Obliczenia godzin dla każdego dnia
                    int hoursColumnIndex = schedule.GetLength(1) + 1;
                    worksheet.Cell(1, hoursColumnIndex + 1).Value = "Godziny na dzień";

                    for (int i = 0; i < schedule.GetLength(0); i++)
                    {
                        string day = worksheet.Cell(i + 2, 2).GetString();
                        int hours = appState.CompanyInstance.CountWorkingHours(day);
                        worksheet.Cell(i + 2, hoursColumnIndex + 1).Value = hours;
                    }

                    // Podsumowanie godzin dla pracowników
                    for (int j = 0; j < employees.Count; j++)
                    {
                        string employeeScheduleColumn = $"{(char)('A' + j + 2)}";
                        string totalHoursCell = $"{(char)('A' + j + 2)}{schedule.GetLength(0) + 3}";

                        worksheet.Cell(schedule.GetLength(0) + 3, j + 3).FormulaA1 =
                            $"SUMIF({employeeScheduleColumn}2:{employeeScheduleColumn}{schedule.GetLength(0) + 1}, \"<>x\", " +
                            $"{(char)('A' + hoursColumnIndex)}2:{(char)('A' + hoursColumnIndex)}{schedule.GetLength(0) + 1})";
                    }

                    // Zapis pliku Excel w lokalnym systemie plików
                    var filePath = Path.Combine(FileSystem.AppDataDirectory, "GeneratedSchedule.xlsx");
                    Console.WriteLine($"Próba zapisu pliku w: {filePath}");

                    // Zapis pliku do systemu plików
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.SaveAs(stream);
                    }

                    await JSRuntime.InvokeVoidAsync("console.log", $"Plik Excel został zapisany pomyślnie w: {filePath}");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("console.error", $"Błąd podczas generowania pliku Excel: {ex.Message}");
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