using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSchedule.Layout.content_component.employeePreference
{
    public partial class EmployeePreference
    {
        private string? errorMessage { get; set; }
        private List<Employee> employees { get; set; } = new List<Employee>();
        private IEmployeesRepository? employeesRepository { get; set; }

        protected override void OnInitialized()
        {
            employeesRepository = appState.EmployeesRepository;
            employees = employeesRepository.GetEmployees();
        }

        private void AddDate(Employee employee)
        {
            if (employee.SelectedDayOff <= 0 || employee.SelectedDayOff >= 32)
            {
                errorMessage = "Invalid date format";
                return;
            }

            errorMessage = null;
            employee.daysOff.Add(employee.SelectedDayOff);
        }

        private void SubtractDay(Employee employee, int index)
        {
            if (employee.daysOff == null || index < 0 || index >= employee.daysOff.Count)
            {
                return;
            }

            employee.daysOff.RemoveAt(index);
        }

        private void OnEnterKeyPress(KeyboardEventArgs e, Employee employee)
        {
            if (e.Key == "Enter")
            {
                AddDate(employee);
            }
        }
    }
}