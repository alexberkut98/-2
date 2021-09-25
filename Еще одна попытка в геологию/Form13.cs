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
    public partial class Form13 : Form
    {
        public int f { get; set; }
        public new string Name { get; set; }
        public Form13(List<string> Names1,string Name2)
        {
            InitializeComponent();
            foreach (string s in Names1)
            {
                //В этот список не должно попасть имя диаграммы, в которой мы сейчас находимся
                if (s != Name2)
                    comboBox1.Items.Add(s);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            f = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            f = 2;
        }

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

        private void Form13_Load(object sender, EventArgs e)
        {
            f = -1;
        }
    }
}
