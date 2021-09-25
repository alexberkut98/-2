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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            st = 0;
        }
        public new string Name { get; set; }
        public int st { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Не введено имя для диаграммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Name = textBox1.Text;
                st = 0;
                Close();
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            st = -1;//Я ошибся и никакого создания новой диаграммы пока не планирую.
        }
    }
}
