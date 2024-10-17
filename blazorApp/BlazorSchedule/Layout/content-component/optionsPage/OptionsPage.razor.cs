using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlazorSchedule.Layout.content_component.optionsPage
{
    public partial class OptionsPage
    {
        string? employeeData { get; set; }
        string? companyData { get; set; }
        Company? companyInstance { get; set; } = new Company();
        EmployeesRepository? employeesRepository { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            companyInstance = appState.CompanyInstance;
            employeesRepository = appState.EmployeesRepository;

            companyData="{\"shift\":null,\"name\":\"Soprano\",\"workingDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"workingHoursDay\":{\"Monday\":{\"12:00:00\":\"22:00:00\"},\"Tuesday\":{\"12:00:00\":\"22:00:00\"},\"Wednesday\":{\"12:00:00\":\"22:00:00\"},\"Thursday\":{\"12:00:00\":\"22:00:00\"},\"Friday\":{\"12:00:00\":\"23:00:00\"},\"Saturday\":{\"12:00:00\":\"23:00:00\"},\"Sunday\":{\"15:00:00\":\"22:00:00\"}},\"positionsPerDay\":{\"Monday\":[\"Kierowca\",\"Kuchnia\",\"Bar\"],\"Tuesday\":[\"Kierowca\",\"Kuchnia\",\"Bar\",\"Kuchnia\"],\"Wednesday\":[\"Kierowca\",\"Kuchnia\",\"Bar\",\"Kuchnia\"],\"Thursday\":[\"Kierowca\",\"Kuchnia\",\"Bar\"],\"Friday\":[\"Kierowca\",\"Kuchnia\",\"Kuchnia\",\"Bar\"],\"Saturday\":[\"Kierowca\",\"Kierowca\",\"Kuchnia\",\"Kuchnia\",\"Bar\"],\"Sunday\":[\"Kierowca\",\"Kierowca\",\"Kuchnia\",\"Kuchnia\",\"Bar\"]},\"positionsList\":[\"Kierowca\",\"Kuchnia\",\"Bar\"],\"isCheckboxChecked\":false}";


            employeeData="{\"employees\":[{\"name\":\"Brygida\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":-2,\"daysOff\":[],\"SelectedDayOff\":13,\"positions\":[\"Kuchnia\"]},{\"name\":\"Magda\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":0,\"daysOff\":[],\"SelectedDayOff\":15,\"positions\":[\"Kuchnia\"]},{\"name\":\"Aska\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":-3,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Kuchnia\"]},{\"name\":\"Julia\",\"typeOfAgreement\":1,\"minHours\":130,\"minHoursUsed\":-1,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Bar\"]},{\"name\":\"Beniamin\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":-8,\"daysOff\":[],\"SelectedDayOff\":29,\"positions\":[\"Kierowca\"]},{\"name\":\"Denis\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":-4,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Kierowca\"]},{\"name\":\"Laura\",\"typeOfAgreement\":1,\"minHours\":130,\"minHoursUsed\":-6,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Kierowca\",\"Bar\"]},{\"name\":\"Dawid\",\"typeOfAgreement\":1,\"minHours\":140,\"minHoursUsed\":129,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Kierowca\"]},{\"name\":\"Iwona\",\"typeOfAgreement\":1,\"minHours\":40,\"minHoursUsed\":-4,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Bar\"]}]}";


            // employeeData =
            // "{\"employees\":[{\"name\":\"Brygida\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":25,\"daysOff\":[26,21,18,1,2,3,4,5,6,7,8,9,10,11,12,13],\"SelectedDayOff\":13,\"positions\":[\"Kuchnia\"]},{\"name\":\"Magda\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":-5,\"daysOff\":[3,11,12,14,15],\"SelectedDayOff\":15,\"positions\":[\"Kuchnia\"]},{\"name\":\"Aska\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":44,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Kuchnia\"]},{\"name\":\"Julia\",\"typeOfAgreement\":1,\"minHours\":140,\"minHoursUsed\":-9,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Bar\"]},{\"name\":\"Beniamin\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":11,\"daysOff\":[2,7,8,16,17,18,19,20,22,23,24,25,26,27,29],\"SelectedDayOff\":29,\"positions\":[\"Kierowca\"]},{\"name\":\"Denis\",\"typeOfAgreement\":0,\"minHours\":160,\"minHoursUsed\":-3,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Kierowca\"]},{\"name\":\"Laura\",\"typeOfAgreement\":1,\"minHours\":150,\"minHoursUsed\":-9,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Kierowca\",\"Bar\"]},{\"name\":\"Dawid\",\"typeOfAgreement\":1,\"minHours\":140,\"minHoursUsed\":130,\"daysOff\":[],\"SelectedDayOff\":0,\"positions\":[\"Kierowca\"]}]}";
            // companyData =
            // "{\"shift\":null,\"name\":\"Soprano\",\"workingDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"workingHoursDay\":{\"Monday\":{\"12:00:00\":\"22:00:00\"},\"Tuesday\":{\"12:00:00\":\"22:00:00\"},\"Wednesday\":{\"12:00:00\":\"22:00:00\"},\"Thursday\":{\"12:00:00\":\"22:00:00\"},\"Friday\":{\"12:00:00\":\"23:00:00\"},\"Saturday\":{\"12:00:00\":\"23:00:00\"},\"Sunday\":{\"15:00:00\":\"22:00:00\"}},\"positionsPerDay\":{\"Monday\":[\"Kierowca\",\"Kuchnia\",\"Bar\"],\"Tuesday\":[\"Kierowca\",\"Kuchnia\",\"Bar\"],\"Wednesday\":[\"Kierowca\",\"Kuchnia\",\"Bar\"],\"Thursday\":[\"Kierowca\",\"Kuchnia\",\"Bar\"],\"Friday\":[\"Kierowca\",\"Kuchnia\",\"Kuchnia\",\"Bar\"],\"Saturday\":[\"Kierowca\",\"Kierowca\",\"Kuchnia\",\"Kuchnia\",\"Bar\"],\"Sunday\":[\"Kierowca\",\"Kierowca\",\"Kuchnia\",\"Kuchnia\",\"Bar\"]},\"positionsList\":[\"Kierowca\",\"Kuchnia\",\"Bar\"],\"isCheckboxChecked\":false}";
        }
        private void ReadData()
        {
            if(companyInstance == null) return;
            Console.WriteLine(companyInstance.name);
        }

        private void SavaAllData()
        {
            string employeeData = JsonConvert.SerializeObject(employeesRepository);
            Console.WriteLine(employeeData);

            string companyData = JsonConvert.SerializeObject(companyInstance);
            Console.WriteLine(companyData);
        }

        private void LoadAllData()
        {
            if(employeeData == null || companyData == null) return;
            companyInstance = JsonConvert.DeserializeObject<Company>(companyData);
            employeesRepository = JsonConvert.DeserializeObject<EmployeesRepository>(employeeData);

            if(companyInstance == null || employeesRepository == null) return;
            appState.CompanyInstance = companyInstance;
            appState.EmployeesRepository = employeesRepository;
        }
    }
}
