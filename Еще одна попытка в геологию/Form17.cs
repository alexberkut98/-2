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
    public partial class Form17 : Form
    {
        public Form17()
        {
            InitializeComponent();
            i = 0;
        }
        public int i { get; set; }
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

        private void Form17_Load(object sender, EventArgs e)
        {
            i = -1;
        }
    }
}
