namespace BlazorSchedule
{
    public class AppState
    {
        public Company CompanyInstance { get; set; }
        public Company CompanySecondShift { get; set; }
        public EmployeesRepository EmployeesRepository { get; set; }
        public Schedule schedule { get; set;}

        public AppState()
        {
            CompanyInstance = Company.Instance;
            EmployeesRepository = new EmployeesRepository();
            schedule = Schedule.Instance;
            CompanySecondShift = Company.Instance;
        }
    }
}