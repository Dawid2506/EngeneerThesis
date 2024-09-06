using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using YourBlazorProject.Models;


namespace BlazorSchedule.Layout.content_component.companyInformation
{
    public partial class CompanyInformationEditNext
    {
        [Parameter]
        public string? name { get; set; }
        [Parameter]
        public ShiftType shift { get; set; }
        public List<string> positions { get; set; } = new List<string>();
        public List<string> workingDaysList { get; set; } = new List<string>();

        [Parameter]
        public string? encodedPositions { get; set; }

        [Parameter]
        public string? encodedWorkingDaysList { get; set; }

        private bool isCheckboxChecked = true;
        private List<DateTime> startHours { get; set; } = new List<DateTime>();
        private List<DateTime> endHours { get; set; } = new List<DateTime>();
        private Dictionary<DateTime, DateTime> workingHours { get; set; } = new Dictionary<DateTime, DateTime>();
        private Dictionary<string, List<string>> positionsPerDay { get; set; } = new Dictionary<string, List<string>>();
        private string? position { get; set; }
        public DateTime startHourForAll { get; set; }
        public DateTime endHourForAll { get; set; }
        public Company? companyInstance;

        protected override void OnInitialized()
        {
            if (shift == ShiftType.First)
            {
                companyInstance = appState.CompanyInstance;
            }
            else if (shift == ShiftType.Second)
            {
                companyInstance = appState.CompanySecondShift;
            }

            positions = JsonSerializer.Deserialize<List<string>>(encodedPositions ?? "[]") ?? new List<string>();
            workingDaysList = JsonSerializer.Deserialize<List<string>>(encodedWorkingDaysList ?? "[]") ?? new List<string>();
            position = positions.FirstOrDefault();

            positionsPerDay = new Dictionary<string, List<string>>();

            for (int i = 0; i < workingDaysList.Count; i++)
            {
                startHours.Add(DateTime.MinValue);
                endHours.Add(DateTime.MinValue);
                positionsPerDay.Add(workingDaysList[i], new List<string>());
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (companyInstance?.workingHoursDay == null) return;

            positionsPerDay = companyInstance.positionsPerDay;

            if (companyInstance.isCheckboxChecked)
            {
                startHourForAll = DateTime.Parse(companyInstance.workingHoursDay.Values.First().Keys.First());
                endHourForAll = DateTime.Parse(companyInstance.workingHoursDay.Values.First().Values.First());
            }
            else
            {
                foreach (var day in companyInstance.workingHoursDay)
                {
                    var index = workingDaysList.IndexOf(day.Key);

                    startHours[index] = DateTime.Parse(day.Value.Keys.First());
                    endHours[index] = DateTime.Parse(day.Value.Values.First());
                }
            }
        }

        private void ToggleCheckbox()
        {
            isCheckboxChecked = !isCheckboxChecked;
        }

        private void UpdateWorkingHours(string day, DateTime startHour, DateTime endHour, bool isStartTime)
        {
            if (isStartTime)
            {
                startHours[workingDaysList.IndexOf(day)] = startHour;
            }
            else
            {
                endHours[workingDaysList.IndexOf(day)] = endHour;
            }
        }

        private void AddPosition(int iIndex)
        {
            if (position == null) return;

            positionsPerDay[workingDaysList[iIndex]].Add(position);
        }

        private void RemovePosition(int iIndex)
        {
            if (position == null) return;
            positionsPerDay[workingDaysList[iIndex]].Remove(position);
        }

        private void AddShift()
        {
            SaveData();

            string encodedPositions = Uri.EscapeDataString(JsonSerializer.Serialize(positions ?? new List<string>()));
            string encodedWorkingDaysList = Uri.EscapeDataString(JsonSerializer.Serialize(workingDaysList ?? new List<string>()));
            string encodedName = Uri.EscapeDataString(name ?? string.Empty);
            shift = ShiftType.Second;
            NavigationManager.NavigateTo($"/company-information-edit-next/{encodedPositions}/{encodedWorkingDaysList}/{encodedName}/{shift}");
        }

        private void SaveData()
        {
            // Save the working hours per day
            Dictionary<string, Dictionary<string, string>> myWorkingHoursDay = new Dictionary<string, Dictionary<string, string>>();

            if (isCheckboxChecked)
            {
                string startHour = startHourForAll.TimeOfDay.ToString();
                string endHour = endHourForAll.TimeOfDay.ToString();

                foreach (var day in workingDaysList)
                {
                    Dictionary<string, string> myWorkingHours = new Dictionary<string, string>();
                    myWorkingHours.Add(startHour, endHour);
                    myWorkingHoursDay.Add(day, myWorkingHours);
                }
            }
            else
            {
                for (int i = 0; i < workingDaysList.Count; i++)
                {
                    string startHour = startHours[i].TimeOfDay.ToString();
                    string endHour = endHours[i].TimeOfDay.ToString();

                    Dictionary<string, string> myWorkingHours = new Dictionary<string, string>();
                    myWorkingHours.Add(startHour, endHour);
                    myWorkingHoursDay.Add(workingDaysList[i], myWorkingHours);
                }
            }

            if (companyInstance == null || name == null) return;

            companyInstance.workingDays = workingDaysList;
            companyInstance.workingHoursDay = myWorkingHoursDay;
            companyInstance.positionsPerDay = positionsPerDay;
            companyInstance.name = name;
            companyInstance.positionsList = positions;
            companyInstance.isCheckboxChecked = isCheckboxChecked;

            if (shift == ShiftType.First)
            {
                appState.CompanyInstance = companyInstance;
            }
            else if (shift == ShiftType.Second)
            {
                appState.CompanySecondShift = companyInstance;
            }

            NavigationManager.NavigateTo("/company-information");
        }
    }
}