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
    public partial class Form8 : Form
    {
        public Form8(List<string> Names1)
        {
            Name = "";
            InitializeComponent();
            foreach (string s in Names1)
                comboBox1.Items.Add(s);
        }
        public new string Name { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
                MessageBox.Show("Не введено имя для диаграммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Name = comboBox1.Text;
                Close();
            }
            Close();
        }
    }
}
