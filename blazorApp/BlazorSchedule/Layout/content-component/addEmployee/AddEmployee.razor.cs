using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBlazorProject.Models;

namespace BlazorSchedule.Layout.content_component.addEmployee
{
    public partial class AddEmployee
    {
        private string? minHours;
        private AgreementType? typeOfAgreement;
        private string? name;
        private int mintHours;
        private string? errorMessage;
        private List<Employee> employees = new List<Employee>();
        private List<string> positions = new List<string>();
        private string? position;

        [Parameter]
        public string? editByName { get; set; }

        protected override void OnInitialized()
        {
            positions = new List<string>();
            position = appState.CompanyInstance.positionsList[0];
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (editByName == null) return;

            name = editByName;
            List<Employee> employees = appState.EmployeesRepository.GetEmployees();
            Employee? employee = employees.FirstOrDefault(e => e.name == editByName);
            typeOfAgreement = employee?.typeOfAgreement;
            minHours = employee?.minHours.ToString();
            positions = employee?.positions ?? new List<string>();

        }

        private void AddEmployeeMethod()
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(minHours))
            {
                errorMessage = "Please fill all fields";
                return;
            }

            try
            {
                mintHours = int.Parse(minHours);

                if (typeOfAgreement != AgreementType.contract)
                {
                    typeOfAgreement = AgreementType.mandate;
                }

                if (appState.EmployeesRepository.GetEmployees().Any(e => e.name == name))
                {
                    Employee particularEmployee = appState.EmployeesRepository.GetEmployeeByName(name);
                    particularEmployee.typeOfAgreement = (AgreementType)typeOfAgreement;
                    particularEmployee.minHours = mintHours;
                    particularEmployee.positions = positions;
                    particularEmployee.name = name;
                }
                else
                {
                    Employee employee = new Employee(name, (AgreementType)typeOfAgreement, mintHours, positions);

                    appState.EmployeesRepository.AddEmployee(employee);

                    employees = appState.EmployeesRepository.employees;
                }

                errorMessage = null;

                NavigationManager.NavigateTo("/add-employee-menu");
            }
            catch (Exception e)
            {
                errorMessage = $"Invalid data entered! {e.Message}";
            }
        }

        private void AddPosition()
        {
            if (position != null && positions.Contains(position))
            {
                errorMessage = "Position already exists";
                return;
            }
            else
            {
                positions.Add(position ?? string.Empty);
                position = string.Empty;
                errorMessage = null;
            }
        }

        private void GoBack()
        {
            NavigationManager.NavigateTo("/add-employee-menu");
        }
    }
}