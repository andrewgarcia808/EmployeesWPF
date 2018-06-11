using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Employees;

namespace Employees
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
        /******************************************************************/
        /*                          Get Paychecks                         */
        /******************************************************************/
        private void RunPayChecks_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> AllEmployees = getAllEmployees();
            getPayChecks(AllEmployees);
        }
        static void getPayChecks(List<Employee> AllEmployees)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EmployeeID", Type.GetType("System.String"));
            dt.Columns.Add("FirstName", Type.GetType("System.String"));
            dt.Columns.Add("LastName", Type.GetType("System.String"));
            dt.Columns.Add("GrossPay", Type.GetType("System.Double"));
            dt.Columns.Add("FederalTax", Type.GetType("System.Double"));
            dt.Columns.Add("StateTax", Type.GetType("System.Double"));
            dt.Columns.Add("NetPay", Type.GetType("System.Double"));

            foreach (Employee employee in AllEmployees)
            {
                DataRow dtRow = dt.NewRow();
                dtRow["EmployeeID"] = employee.EmployeeID;
                dtRow["FirstName"] = employee.FirstName;
                dtRow["LastName"] = employee.LastName;
                dtRow["GrossPay"] = employee.getGrossPay();
                dtRow["FederalTax"] = employee.getFederalTax();
                dtRow["StateTax"] = employee.getStateTax();
                dtRow["NetPay"] = employee.getNetPay();
                dt.Rows.Add(dtRow);
            }
            dt.DefaultView.Sort = "GrossPay DESC";
            dt = dt.DefaultView.ToTable();
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/PayChecks.txt"))
            {
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    StringBuilder output = new StringBuilder();
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        if (col < 3)
                        {
                            output.Append(dt.Rows[row][col].ToString());
                        }
                        else
                        {
                            output.Append(((double)dt.Rows[row][col]).ToString("$#,##0.00"));
                        }
                        if (col != dt.Columns.Count - 1)
                        {
                            output.Append(", ");
                        }
                    }
                    sw.WriteLine(output.ToString());
                    output.Clear();
                }
            }
        }
        /******************************************************************/
        /*                      Get Top 15 Percent                        */
        /******************************************************************/
        private void RunTop15Percent_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> AllEmployees = getAllEmployees();
            getTop15Percent(AllEmployees);
            ReportStatus.Text = "Report is Saved on Your Desktop!";
        }
        static void getTop15Percent(List<Employee> AllEmployees)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FirstName", Type.GetType("System.String"));
            dt.Columns.Add("LastName", Type.GetType("System.String"));
            dt.Columns.Add("NumberOfYearsWorked", Type.GetType("System.Int32"));
            dt.Columns.Add("GrossPay", Type.GetType("System.Double"));

            foreach (Employee employee in AllEmployees)
            {
                DataRow dtRow = dt.NewRow();
                dtRow["FirstName"] = employee.FirstName;
                dtRow["LastName"] = employee.LastName;
                dtRow["NumberOfYearsWorked"] = employee.getNumberOfYearsWorked();
                dtRow["GrossPay"] = employee.getGrossPay();
                dt.Rows.Add(dtRow);
            }
            dt.DefaultView.Sort = "GrossPay DESC";
            dt = dt.DefaultView.ToTable();

            DataTable Top15Percentdt = new DataTable();
            Top15Percentdt.Columns.Add("FirstName", Type.GetType("System.String"));
            Top15Percentdt.Columns.Add("LastName", Type.GetType("System.String"));
            Top15Percentdt.Columns.Add("NumberOfYearsWorked", Type.GetType("System.Int32"));
            Top15Percentdt.Columns.Add("GrossPay", Type.GetType("System.Double"));
            for (int i = 0; i < (dt.Rows.Count * .15); i++)
            {
                DataRow dtRow = Top15Percentdt.NewRow();
                dtRow["FirstName"] = dt.Rows[i][0];
                dtRow["LastName"] = dt.Rows[i][1];
                dtRow["NumberOfYearsWorked"] = dt.Rows[i][2];
                dtRow["GrossPay"] = dt.Rows[i][3];
                Top15Percentdt.Rows.Add(dtRow);
            }
            Top15Percentdt.DefaultView.Sort = "NumberOfYearsWorked DESC, LastName ASC, FirstName ASC";
            Top15Percentdt = Top15Percentdt.DefaultView.ToTable();
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Top15Percent.txt"))
            {
                for (int row = 0; row < Top15Percentdt.Rows.Count; row++)
                {
                    StringBuilder output = new StringBuilder();
                    for (int col = 0; col < Top15Percentdt.Columns.Count; col++)
                    {
                        if (col < 3)
                        {
                            output.Append(Top15Percentdt.Rows[row][col].ToString());
                        }
                        else
                        {
                            output.Append(((double)Top15Percentdt.Rows[row][col]).ToString("$#,##0.00"));
                        }
                        if (col != Top15Percentdt.Columns.Count - 1)
                        {
                            output.Append(", ");
                        }
                    }
                    sw.WriteLine(output.ToString());
                    output.Clear();
                }
            }
        }
        /******************************************************************/
        /*                         Get State Info                         */
        /******************************************************************/
        private void RunStateInfo_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> AllEmployees = getAllEmployees();
            getStateInfo(AllEmployees);
            ReportStatus.Text = "Report is Saved on Your Desktop!";
        }
        static void getStateInfo(List<Employee> AllEmployees)
        {
            List<string> States = getStates(AllEmployees);
            States = States.OrderBy(x => x).ToList();
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/StateInfo.txt"))
            {
                foreach (string State in States)
                {
                    StringBuilder output = new StringBuilder();
                    List<Employee> EmployeesInState = AllEmployees.Where(x => x.State == State).ToList();
                    double MedianTimeWorked = getMedianTimeWorked(EmployeesInState.OrderBy(x => x.Hours).ToList());
                    double MedianNetPay = getMedianNetPay(EmployeesInState.OrderBy(x => x.getNetPay()).ToList());
                    double StateTaxes = getStateTaxes(EmployeesInState);
                    output.Append(State + ", " + MedianTimeWorked.ToString("#,##0.00") + ", " + MedianNetPay.ToString("$#,##0.00") + ", " + StateTaxes.ToString("$#,##0.00"));
                    sw.WriteLine(output.ToString());
                    output.Clear();
                }
            }
        }
        static double getMedianTimeWorked(List<Employee> EmployeesInState)
        {
            double MedianTimeWorked = 0;
            List<double> TimeWorked = new List<double>();
            foreach (Employee employee in EmployeesInState)
            {
                TimeWorked.Add(employee.Hours);
            }
            MedianTimeWorked = getMedian(TimeWorked);
            return MedianTimeWorked;
        }
        static double getMedianNetPay(List<Employee> EmployeesInState)
        {
            double MedianNetPay = 0;
            List<double> NetPay = new List<double>();
            foreach (Employee employee in EmployeesInState)
            {
                NetPay.Add(employee.Hours);
            }
            MedianNetPay = getMedian(NetPay);
            return MedianNetPay;
        }
        static double getStateTaxes(List<Employee> EmployeesInState)
        {
            double StateTaxes = 0;
            foreach (Employee employee in EmployeesInState)
            {
                StateTaxes += employee.getStateTax();
            }
            return StateTaxes;
        }
        static double getMedian(List<double> values)
        {
            double Median = 0;
            if (values.Count % 2 == 0)
            {
                Median = (values[(values.Count / 2)] + values[(values.Count / 2) - 1]) / 2;
            }
            else
            {
                Median = values[(values.Count / 2)];
            }
            return Median;
        }
        static List<string> getStates(List<Employee> AllEmployees)
        {
            List<string> States = new List<string>();
            foreach (Employee employee in AllEmployees)
            {
                string State = employee.State;
                if (!StateExists(States, State))
                {
                    States.Add(State);
                }
            }
            return States;
        }
        static bool StateExists(List<string> States, string State)
        {
            foreach (string state in States)
            {
                if (state == State)
                {
                    return true;
                }
            }
            return false;
        }
        /******************************************************************/
        /*                        Get All Reports                         */
        /******************************************************************/
        private void RunAllReports_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> AllEmployees = getAllEmployees();
            getPayChecks(AllEmployees);
            getTop15Percent(AllEmployees);
            getStateInfo(AllEmployees);
            ReportStatus.Text = "Reports are Saved on Your Desktop!";
        }
        /******************************************************************/
        /*                       Get Employee Info                        */
        /******************************************************************/
        private void GetEmployeeInfo_Click(object sender, RoutedEventArgs e)
        {
            GridEmployeeInfo.Visibility = Visibility.Hidden;
            string id = txtboxEmployeeID.Text;
            Employee employee = getEmployeeByID(id);
            if (employee.EmployeeID != null)
            {
                GridEmployeeInfo.Visibility = Visibility.Visible;
                EmployeeInfoStatus.Text = "";
                GridEmployeeID.Text = employee.EmployeeID;
                GridFirstName.Text = employee.FirstName;
                GridLastName.Text = employee.LastName;
                GridPayType.Text = employee.PayType;
                GridSalary.Text = employee.Salary.ToString("$#,##0.00");
                GridStartDate.Text = employee.StartDate;
                GridState.Text = employee.State;
                GridHours.Text = employee.Hours.ToString("###0.00");
                GridGrossPay.Text = employee.getGrossPay().ToString("$#,##0.00");
                GridFederalTaxes.Text = employee.getFederalTax().ToString("$#,##0.00");
                GridStateTaxes.Text = employee.getStateTax().ToString("$#,##0.00");
                GridNetPay.Text = employee.getNetPay().ToString("$#,##0.00");
            }
            else
            {
                EmployeeInfoStatus.Text = "That Employee Does Not Exists!";
            }
        }
        static Employee getEmployeeByID(string EmployeeID)
        {
            Employee employee = new Employee(EmployeeID);
            return employee;
        }
        /******************************************************************/
        /*                        Global Functions                        */
        /******************************************************************/




        static List<Employee> getAllEmployees()
        {
            string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetFullPath("../../Files/Employees.txt"));
            List<Employee> AllEmployees = new List<Employee>();
            foreach (string line in lines)
            {
                string[] emp = line.Split(',');
                string EmployeeID = emp[0];
                string FirstName = emp[1];
                string LastName = emp[2];
                string PayType = emp[3];
                double Salary = Convert.ToDouble(emp[4]);
                string StartDate = emp[5];
                string State = emp[6];
                double Hours = Convert.ToDouble(emp[7]);

                AllEmployees.Add(new Employee(EmployeeID, FirstName, LastName, PayType, Salary, StartDate, State, Hours));
            }
            return AllEmployees;
        }


    }
}
