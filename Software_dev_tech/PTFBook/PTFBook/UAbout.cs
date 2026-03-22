using System;
using System.Windows.Forms;

namespace PTFBook
{
    public partial class TAboutBox : Form
    {
        public TAboutBox()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}