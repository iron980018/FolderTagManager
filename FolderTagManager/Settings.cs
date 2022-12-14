using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FolderTagManager
{
    public partial class Settings : Form
    {
        private bool adress_change = false;
        private string s;
        public Settings(Form1 father)
        {
            InitializeComponent();
            this.Tag = father;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            s = textBox1.Text;
            adress_change = true;
        }

        private void button1_Click(object sender, EventArgs e)//離開
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)//儲存
        {
            if (adress_change)
            {
                adress_change = false;
                ((Form1)this.Tag).change_adress(s);
                MessageBox.Show("成功 ! ");
            }
        }
    }
}
