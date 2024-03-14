using System;
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
    /// Interaction logic for EmployeePreference.xaml
    /// </summary>
    public partial class EmployeePreference : Page
    {
        private static IEmployeesRepository employeesRepository;
        private List<Employee> employees;
        public EmployeePreference()
        {
            InitializeComponent();
            employeesRepository = EmployeesRepository.GetInstance();
            employees = employeesRepository.GetEmployees();
        }

        private void showClick_Click(object sender, RoutedEventArgs e)
        {
            show.Content = "cos:" + employees[1].name;
            foreach (Employee employee in employees)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = employee.name;
                newItem.Name = employee.name;
                employeesList.Items.Add(newItem);
            }
        }

        private void onLoadPreferencePage(object sender, RoutedEventArgs e)
        {
            foreach (Employee employee in employees)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = employee.name;
                newItem.Name = employee.name;
                employeesList.Items.Add(newItem);
            }
        }



    }
}
