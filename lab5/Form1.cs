using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            double number = -1;

            if(numberInputTextBox.Text == "")
            {
                MessageBox.Show($"Введенное число: {number}");
                return;
            }
                
            try 
            {
                number = double.Parse(numberInputTextBox.Text);
                if (Math.Abs(number) > double.MaxValue)
                {
                    DialogResult dialogResult = MessageBox.Show(
                    "Произошла ошибка при попытке конвертировать число. Повторить ввод числа?",
                    "Сообщение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                    if (dialogResult == DialogResult.No)
                        Application.Exit();
                    else
                        numberInputTextBox.Text = "";
                    return;
                }

                MessageBox.Show($"Введенное число: {number}");
            }
            catch
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Произошла ошибка при попытке конвертировать число. Повторить ввод числа?", 
                    "Сообщение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (dialogResult == DialogResult.No)
                    Application.Exit();
                else
                    numberInputTextBox.Text = "";
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
