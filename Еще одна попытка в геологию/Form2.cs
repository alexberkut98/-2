using System;
using System.Windows.Forms;

namespace Еще_одна_попытка_в_геологию
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public int S { get; set; }
       public int T { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            int p = 1;
            int i = 0;
            if (textBox1.Text == "")
                MessageBox.Show("Не введена длинна единичного отрезка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                while (p != 0 && i < textBox1.Text.Length)
                {
                    if (textBox1.Text[i] == '0' && i != 0 || textBox1.Text[i] == '1' || textBox1.Text[i] == '2' || textBox1.Text[i] == '3' || textBox1.Text[i] == '4' || textBox1.Text[i] == '5' || textBox1.Text[i] == '6' || textBox1.Text[i] == '7' || textBox1.Text[i] == '8' || textBox1.Text[i] == '9')
                        i++;
                    else
                        p = 0;
                }
                if (p == 1)
                {
                    T = Convert.ToInt32(textBox1.Text);
                    Close();
                }
                else
                {
                    textBox1.Text = "";
                    MessageBox.Show("Некорректный ввод длинны отрезка ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            S = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            S = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            S = 3;
        }
    }
}
