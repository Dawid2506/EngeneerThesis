namespace BlazorSchedule
{
    internal class EmployeesRepository : IEmployeesRepository
    {
        private static EmployeesRepository instance;
        public List<Employee> employees { get; set; }

        private EmployeesRepository()
        {
            employees = new List<Employee>();
        }

        public static EmployeesRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new EmployeesRepository();
            }
            return instance;
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
