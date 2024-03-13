using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBeforeWriting.Res.Clases
{
    internal class Employee
    {
        public string name { get; set; }
        public string typeOfContract { get; set; }
        public int minWorkHours { get; set; }

        public Employee(string name, string typeOfContract, int minWorkHours)
        {
            this.name = name;
            this.typeOfContract = typeOfContract;
            this.minWorkHours = minWorkHours;
        }
    }
}
