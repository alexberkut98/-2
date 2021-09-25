using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Еще_одна_попытка_в_геологию
{
    public partial class Form7 : Form
    {
        public Form7(List<string>Names1)
        {
            InitializeComponent();
            foreach (string s in Names1)
                comboBox1.Items.Add(s);
        }
        public int f { get; set; }
    private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            f = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            f = 2;
        }
        public new string Name { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" && f == 2)
                MessageBox.Show("Не введено имя для диаграммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Name = comboBox1.Text;
                Close();
            }
            Close();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            f = -1;
        }
    }
}
