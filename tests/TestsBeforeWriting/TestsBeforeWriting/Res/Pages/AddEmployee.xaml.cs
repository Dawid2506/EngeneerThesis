﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestsBeforeWriting.Res.Clases;

namespace TestsBeforeWriting.Res.Pages
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Page
    {
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            IEmployeesRepository employeesRepository = EmployeesRepository.GetInstance();
            string name = this.name.Text;
            string contractType = "Zlecenie";
            int minWorkHours = int.Parse(this.minWorkHours.Text);
            Employee employee = new Employee(name, contractType, minWorkHours);
            employeesRepository.AddEmployee(employee);
            this.name.Text = "";
            this.minWorkHours.Text = "";

        }
    }
}
