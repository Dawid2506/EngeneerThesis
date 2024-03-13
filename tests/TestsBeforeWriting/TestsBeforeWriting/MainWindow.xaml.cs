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
using TestsBeforeWriting.Res.Pages;

namespace TestsBeforeWriting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addEmployeePage_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee();
            MainFrame.Content = addEmployee;
        }

        private void employeePreferencePage_Click(object sender, RoutedEventArgs e)
        {
            EmployeePreference employeePreference = new EmployeePreference();
            MainFrame.Content = employeePreference;
        }
    }
}
