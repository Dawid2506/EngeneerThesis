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
        public EmployeePreference()
        {
            InitializeComponent();
        }

        private void showClick_Click(object sender, RoutedEventArgs e)
        {
            IEmployeesRepository employeesRepository = EmployeesRepository.GetInstance();
            List<Employee> employees = employeesRepository.GetEmployees();
            show.Content = "cos:" + employees[1].name;
        }
    }
}
