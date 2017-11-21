using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CalculatePaycheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //CONSTANTS
            const double SOCIAL_SEC_TAX = 0.06;
            const double FEDERAL_INC_TAX = 0.14;
            const double PROVINCIAL_TAX = 0.05;
            const float UNION_FEES = 25F;
            const float BASE_WAGE_PROG = 35.50F;
            const float BASE_WAGE_ENG = 40.25F;
            const float BASE_WAGE_TECH = 37.50F;
            const float BASE_DEPENDENT = 50.00F;
            const float PLUS3_DEPENDENT = 15.00F;
            const double FULL_TIME = 40;
            const double OVERTIME_MODIFIER = 1.5;
            const double OVERTIME_BONUS = 100;

            //VALIDATE:
            try
            {
                if (textBox1.Text == null || textBox1.Text == "")
                {
                    MessageBox.Show("You must enter a name!");
                    return;
                }
            }
            catch (FormatException)
            {

                MessageBox.Show("Invalid name!");
                return;
            }
            try
            {
                int numberEntered = Convert.ToInt32(textBox2.Text);
                if (numberEntered < 1)
                {
                    MessageBox.Show("You must enter a valid Work ID greater than zero!");
                    return;
                }
            }
            catch (FormatException)
            {

                MessageBox.Show("Please enter an integer Work ID!");
                return;
            }
            try
            {
                double hoursEntered = Convert.ToDouble(textBox3.Text);
                if (hoursEntered < 0)
                {
                    MessageBox.Show("You cannot enter negative hours!");
                    return;
                }
            }
            catch (FormatException)
            {

                MessageBox.Show("You need to enter a double value for hours!");
                return;
            }
            try
            {
                int numberEntered = Convert.ToInt32(textBox4.Text);
                if (numberEntered < 0)
                {
                    MessageBox.Show("You cannot have negative number of dependencies!");
                    return;
                }
                else if (numberEntered > 15)
                {
                    MessageBox.Show("Please don't lie about number of dependencies... lol.");
                    return;
                }
            }
            catch (FormatException)
            {

                MessageBox.Show("You need to enter an integer for dependencies!");
                return;
            }

            //INPUT:
            string employeeName = textBox1.Text;
            int workID = Convert.ToInt32(textBox2.Text);
            string job = comboBox1.Text;
            double hours = Convert.ToDouble(textBox3.Text);
            int dependents = Convert.ToInt32(textBox4.Text);

            //VARIABLES:
            float netPay = 0F;
            float grossPay = 0F;
            float wage = 0F;
            double wageModifier = 1.0;
            double wageBonus = 0;
            switch (job)
            {
                case "Computer Programmer":
                    wage = BASE_WAGE_PROG;
                    wageModifier = OVERTIME_MODIFIER;
                    break;
                case "Electronics Engineer":
                    wage = BASE_WAGE_ENG;
                    wageBonus = OVERTIME_BONUS;
                    break;
                case "Technician":
                    wage = BASE_WAGE_TECH;
                    wageModifier = OVERTIME_MODIFIER;
                    break;
                default:
                    MessageBox.Show("Please use an existing job!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
            }

            //CALC TOTAL PAY:
            if (hours <= FULL_TIME)
            {
                netPay = wage * (float)hours;
            }
            else
            {
                netPay = wage * (float)FULL_TIME + 
                    ((float)(hours - FULL_TIME) * (wage + (float)wageBonus) * (float)wageModifier);
            }
            grossPay = netPay;
            Console.WriteLine("\nGross Pay: " + grossPay);

            //CALC TAXES:
            float socialSecTax = netPay * (float)SOCIAL_SEC_TAX;
            float federalIncTax = netPay * (float)FEDERAL_INC_TAX;
            float provincialTax = netPay * (float)PROVINCIAL_TAX;
            Console.WriteLine("\nTax Dues: " + socialSecTax + "  " + federalIncTax + "  " + provincialTax);
            
            //CALC DEPENDENTS:
            float dependentFees = 0.00F;
            if (dependents > 3)
            {
                dependentFees = (float)dependents * PLUS3_DEPENDENT;
            }
            else
            {
                dependentFees = BASE_DEPENDENT;
            }
            Console.WriteLine("\nDependent Dues: " + dependentFees);

            //CALC NET PAY:
            netPay = netPay - socialSecTax - federalIncTax - provincialTax
                - dependentFees - UNION_FEES;
            Console.WriteLine("\nNet Pay: " + netPay);

            //RECORD:
            using (StreamWriter writer = new StreamWriter("Employee_Net_Pay.txt"))
            {
                writer.WriteLine("Employee: " + employeeName);
                writer.WriteLine(" Work ID: " + workID);
                writer.WriteLine("GrossPay: " + grossPay);
                writer.WriteLine("   SSTax: " + socialSecTax);
                writer.WriteLine("   FITax: " + federalIncTax);
                writer.WriteLine("   PITax: " + provincialTax);
                writer.WriteLine("UnionFee: " + UNION_FEES);
                writer.WriteLine("  DepFee: " + dependentFees);
                writer.WriteLine();
                writer.WriteLine("  NetPay: " + netPay);
            }
            MessageBox.Show("Recorded!");

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
