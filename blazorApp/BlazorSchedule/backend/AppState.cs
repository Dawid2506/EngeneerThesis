namespace BlazorSchedule
{
    public class AppState
    {
        public Company CompanyInstance { get; set; }
        public EmployeesRepository EmployeesRepository { get; set; }

        public AppState()
        {
            CompanyInstance = Company.Instance;
            EmployeesRepository = new EmployeesRepository();
        }
    }
}