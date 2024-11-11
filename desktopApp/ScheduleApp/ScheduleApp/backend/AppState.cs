using YourBlazorProject.Models;

namespace BlazorSchedule
{
    public class AppState
    {
        public Company CompanyInstance { get; set; }
        public Company CompanySecondShift { get; set; }
        public EmployeesRepository EmployeesRepository { get; set; }
        public Schedule schedule { get; set;}
        public ScheduleSymbols scheduleSymbols { get; set; }

        public AppState()
        {
            CompanyInstance = new Company();
            EmployeesRepository = new EmployeesRepository();
            schedule = Schedule.Instance;
            CompanySecondShift = new Company();
            scheduleSymbols = new ScheduleSymbols();
        }
    }
}