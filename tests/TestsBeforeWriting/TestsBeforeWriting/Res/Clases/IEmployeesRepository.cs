using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBeforeWriting.Res.Clases
{
    internal interface IEmployeesRepository
    {
        List<Employee> GetEmployees();
        Employee GetEmployeeByName(string name);
        void AddEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
    }
}
