namespace BlazorSchedule{
    internal interface IEmployeesRepository
    {
        List<Employee> GetEmployees();
        Employee GetEmployeeByName(string name);
        void AddEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
    }
}