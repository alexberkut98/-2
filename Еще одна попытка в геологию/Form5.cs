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
    public partial class Form5 : Form
    {
        public int st { get; set; }
        public Form5()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            st = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            st = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
