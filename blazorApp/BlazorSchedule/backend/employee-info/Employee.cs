namespace BlazorSchedule{
    public class Employee
    {
        public string name { get; set; }
        public string typeOfAgreement { get; set; }
        public int minHours { get; set; }

        public Employee(string name, string typeOfAgreement, int minHours)
        {
            this.name = name;
            this.typeOfAgreement = typeOfAgreement;
            this.minHours = minHours;
        }
    }
}