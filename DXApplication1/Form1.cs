using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;

namespace DXApplicationDock
{
    public partial class Form1 : XtraForm, IDockingForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        public DockManager DockManager => dockManager1;

        [Browsable(false)]
        public Form NativeForm => this;
         
        private void button1_Click(object sender, System.EventArgs e)
        {
            var form = new Form2();

            form.Show();
        }
    }
}