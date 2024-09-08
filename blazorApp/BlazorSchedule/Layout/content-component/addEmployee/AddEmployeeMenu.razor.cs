using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSchedule.Layout.content_component.addEmployee
{
    public partial class AddEmployeeMenu
    {
        private List<Employee> employeeList { get; set; } = new List<Employee>();
        private bool isPersonInfoVisible { get; set; } = false;
        private Employee? actualEmployee { get; set; }
        private EmployeesRepository? employeesRepository { get; set; }

        [Parameter]
        public string? editByName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            employeesRepository = appState.EmployeesRepository;
            employeeList = employeesRepository.GetEmployees();
        }

        private void TogglePersonInfoContainer(string name)
        {
            if(employeesRepository == null) return;
            isPersonInfoVisible = !isPersonInfoVisible;
            actualEmployee = employeesRepository.GetEmployeeByName(name);
        }

        private void ChangeToAddEmployee()
        {
            NavigationManager.NavigateTo("/add-component");
        }

        private void EditPerson()
        {
            if(actualEmployee == null) return;
            editByName = actualEmployee.name;
            NavigationManager.NavigateTo($"/add-component/{editByName}");
        }

        private void DeletePerson()
        {
            if(actualEmployee == null || employeesRepository == null) return;
            employeesRepository.DeleteEmployee(actualEmployee.name);
            employeeList = employeesRepository.GetEmployees();
            isPersonInfoVisible = false;
        }
    }
}