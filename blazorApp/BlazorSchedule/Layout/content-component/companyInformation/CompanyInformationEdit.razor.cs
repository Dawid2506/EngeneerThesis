using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using YourBlazorProject.Models;


namespace BlazorSchedule.Layout.content_component.companyInformation
{
    public partial class CompanyInformationEdit
    {
        [Parameter]
        public string? name { get; set; }
        [Parameter]
        public ShiftType shift { get; set; } = ShiftType.First;

        [Parameter]
        public List<string> workingDaysList { get; set; } = new List<string>();

        [Parameter]
        public List<string> positions { get; set; } = new List<string>();
        private string? position;
        private int mintHours;
        private string? errorMessage;

        protected override void OnInitialized()
        {
            if (appState.CompanyInstance.name != null)
            {
                name = appState.CompanyInstance.name;
                workingDaysList = appState.CompanyInstance.workingDays;
                positions = appState.CompanyInstance.positionsList;
            }
        }

        private void AddDay(string day)
        {
            if (string.IsNullOrEmpty(day))
            {
                return;
            }

            if (workingDaysList.Contains(day))
            {
                workingDaysList.Remove(day);
            }
            else
            {
                workingDaysList.Add(day);
            }
        }

        private void AddPosition()
        {
            if (string.IsNullOrEmpty(position))
            {
                errorMessage = "Position cannot be empty";
                return;
            }

            if (positions.Contains(position))
            {
                errorMessage = "Position already exists";
                return;
            }

            positions.Add(position);
            position = string.Empty;
        }

        private void NextPage()
        {
            if (!AreParametersNotEmpty() || name == null)
            {
                errorMessage = "Fill in all fields";
                return;
            }

            string encodedPositions = Uri.EscapeDataString(JsonSerializer.Serialize(positions));
            string encodedWorkingDaysList = Uri.EscapeDataString(JsonSerializer.Serialize(workingDaysList));
            string encodedName = Uri.EscapeDataString(name);
            NavigationManager.NavigateTo($"/company-information-edit-next/{encodedPositions}/{encodedWorkingDaysList}/{encodedName}");
        }

        private bool AreParametersNotEmpty()
        {
            return !string.IsNullOrEmpty(name) && workingDaysList.Count > 0 && positions.Count > 0;
        }

        private void GoBack()
        {
            NavigationManager.NavigateTo("/add-employee-menu");
        }
    }
}