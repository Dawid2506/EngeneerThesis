namespace BlazorSchedule
{
    public class EmployeesRepository : IEmployeesRepository
    {
        public List<Employee> employees { get; set; }

        public EmployeesRepository()
        {
            employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        public Employee GetEmployeeByName(string name)
        {
            return employees.Find(e => e.name == name);
        }

        public List<Employee> GetEmployees()
        {
            return employees;
        }

        public void RemoveEmployee(Employee employee)
        {
            employees.Remove(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            Employee employeeToUpdate = employees.Find(e => e.name == employee.name);
            employees.Remove(employeeToUpdate);
            employees.Add(employee);
        }
    }
}
