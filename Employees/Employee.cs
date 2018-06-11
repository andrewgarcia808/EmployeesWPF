using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees
{
    class Employee
    {
        /******************************************************************/
        /*                         Constructor                            */
        /******************************************************************/
        public Employee(string employeeID, string firstName, string lastName, string payType, double salary, string startDate, string state, double hours)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            PayType = payType;
            Salary = salary;
            StartDate = startDate;
            State = state;
            Hours = hours;
        }
        public Employee(string employeeID)
        {
            string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetFullPath("../../Files/Employees.txt"));
            foreach (string line in lines)
            {
                string[] emp = line.Split(',');
                if (emp[0] == employeeID)
                {
                    EmployeeID = emp[0];
                    FirstName = emp[1];
                    LastName = emp[2];
                    PayType = emp[3];
                    Salary = Convert.ToDouble(emp[4]);
                    StartDate = emp[5];
                    State = emp[6];
                    Hours = Convert.ToDouble(emp[7]);
                    break;
                }
            }
        }
        /******************************************************************/
        /*                          Attributes                            */
        /******************************************************************/
        public string EmployeeID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PayType { get; }
        public double Salary { get; }
        public string StartDate { get; }
        public string State { get; }
        public double Hours { get; }
        /******************************************************************/
        /*                          Functions                             */
        /******************************************************************/
        public double getGrossPay()
        {
            double GrossPay = 0;

            if (PayType == "S")
            {
                GrossPay = Salary / 26;
            }
            else if (PayType == "H")
            {
                double ExtraOverTime = 0;
                double OverTime = 0;
                double Regular = 0;
                if (Hours > 90)
                {
                    ExtraOverTime = Hours - 90;
                    OverTime = Hours - ExtraOverTime - 80;
                    Regular = Hours - ExtraOverTime - OverTime;
                }
                else if (Hours > 80 && Hours <= 90)
                {
                    OverTime = Hours - 80;
                    Regular = Hours - OverTime;
                }
                else
                {
                    Regular = Hours;
                }

                GrossPay = (ExtraOverTime * Salary) + (OverTime * Salary) + (Regular * Salary);
            }
            return GrossPay;
        }
        public double getFederalTax()
        {
            double GrossPay = getGrossPay();
            double FederalTaxRate = 0.15;
            double FederalTax = 0;

            FederalTax = GrossPay * FederalTaxRate;

            return FederalTax;
        }
        public double getStateTax()
        {
            double GrossPay = getGrossPay();
            double StateTaxRate = 0;
            double StateTax = 0;

            if (State == "UT" || State == "WY" || State == "NV")
            {
                StateTaxRate = 0.05;
            }
            else if (State == "CO" || State == "ID" || State == "AZ" || State == "OR")
            {
                StateTaxRate = 0.065;
            }
            else if (State == "WA" || State == "NM" || State == "TX")
            {
                StateTaxRate = 0.07;
            }

            StateTax = GrossPay * StateTaxRate;

            return StateTax;
        }
        public double getNetPay()
        {
            double NetPay = 0;
            double GrossPay = getGrossPay();
            double FederalTax = getFederalTax();
            double StateTax = getStateTax();

            NetPay = GrossPay - FederalTax - StateTax;
            return NetPay;
        }
        public int getNumberOfYearsWorked()
        {
            int NumberOfYearsWorked = 0;
            DateTime start = Convert.ToDateTime(StartDate);
            DateTime current = start;
            while (current <= DateTime.Now)
            {
                current = current.AddYears(1);
                if (current <= DateTime.Now)
                {
                    NumberOfYearsWorked++;
                }
            }
            return NumberOfYearsWorked;
        }
    }
}
