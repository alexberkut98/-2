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
    public partial class Form9 : Form
    {
        public int i { get; set; }
        public Form9()
        {
            InitializeComponent();
            i = 0;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            i = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            i = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            i = 3;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            i = 4;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            i = 5;
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            i = 3;
        }
    }
}
