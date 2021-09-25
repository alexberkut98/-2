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
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }    
        public new string Name { get; set; }
        public int st { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Не введено имя для файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Name = textBox1.Text;
                st = 0;
                Close();
            }
        }

        //Изменения уже появились но работают ли они в обе стороны?
        private void Form6_Load(object sender, EventArgs e)
        {
            st = -1;//Я ошибся и никакого создания новой диаграммы пока не планирую.
        }
    }
}
