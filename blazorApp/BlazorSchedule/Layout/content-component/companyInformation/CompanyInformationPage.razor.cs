using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBlazorProject.Models;

namespace BlazorSchedule.Layout.content_component.companyInformation
{
    public partial class CompanyInformationPage
    {
        private Company companyInstance = new Company();
        private string? companyName { get; set; }
        private Dictionary<string, Dictionary<string, string>> workingHoursDay = new();
        private Dictionary<string, List<string>> positionsPerDay = new();

        protected override void OnInitialized()
        {
            companyInstance = appState.CompanyInstance;
            companyName = companyInstance.name;
            workingHoursDay = companyInstance.workingHoursDay;
            positionsPerDay = companyInstance.positionsPerDay;
        }

        private void EditInformation()
        {
            NavigationManager.NavigateTo("/company-information-edit");
        }

        private bool IsNotNullCompanyInfo()
        {
            return !string.IsNullOrEmpty(companyName) && workingHoursDay != null && positionsPerDay != null;
        }

        private List<DayInfo> GetWorkingHoursWithPositions()
        {
            List<DayInfo> dayInfoList = new List<DayInfo>();

            foreach (KeyValuePair<string, Dictionary<string, string>> workingHoursEntry in workingHoursDay)
            {
                string day = workingHoursEntry.Key;
                string workingHours = $"{workingHoursEntry.Value.First().Key} - {workingHoursEntry.Value.First().Value}";
                List<string> positions = positionsPerDay.ContainsKey(day) ? positionsPerDay[day] : new List<string>();

                dayInfoList.Add(new DayInfo
                {
                    Day = day,
                    WorkingHours = workingHours,
                    Positions = positions
                });
            }

            return dayInfoList;
        }
    }
}