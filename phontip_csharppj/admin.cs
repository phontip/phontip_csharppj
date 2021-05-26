using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace phontip_csharppj
{
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
        }

       
        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if (passw_textBox.Text =="password")
            {
                stock Obj = new stock();
                Obj.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong Password.Contact The Admin");
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            login Obj = new login();
            Obj.Show();
            this.Hide();
        }

       
    }
}
