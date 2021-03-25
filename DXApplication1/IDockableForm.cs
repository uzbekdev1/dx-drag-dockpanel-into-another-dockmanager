using System.Windows.Forms;
using DevExpress.XtraBars.Docking;

namespace DXApplicationDock
{
    public interface IDockingForm
    {
        DockManager DockManager { get; }

        Form NativeForm { get; }
    }
}