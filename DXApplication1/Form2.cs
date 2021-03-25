using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking.Helpers;
using DevExpress.XtraEditors;

namespace DXApplicationDock
{
    public partial class Form2 : XtraForm
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void dockPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = dockPanel1.PointToScreen(e.Location);

            foreach (var opener in Application.OpenForms)
                if (opener is IDockingForm && ((IDockingForm) opener).DockManager == dockPanel1.DockManager &&
                    ((IDockingForm) opener).NativeForm.Bounds.Contains(pos))
                    return;

            foreach (var opener in Application.OpenForms)
                if (opener is IDockingForm)
                {
                    var form = opener as IDockingForm;

                    if (form.DockManager != dockPanel1.DockManager)
                        if (form.NativeForm.Bounds.Contains(pos))
                        {
                            var oldDockManager = dockPanel1.DockManager;

                            // Register panel with new dock manager.  
                            dockPanel1.Register(form.DockManager);

                            // Add panel to new dock manager root panels, and remove from old dock manager root panels.  
                            form.DockManager.RootPanels.AddRange(new[] {dockPanel1});

                            var pr = oldDockManager.RootPanels.GetType().GetProperty("InnerList",
                                BindingFlags.Instance | BindingFlags.NonPublic);
                            if (pr != null)
                            {
                                var innerList = pr.GetValue(oldDockManager.RootPanels, null) as ArrayList;

                                if (innerList != null) innerList.Remove(dockPanel1);
                            }

                            // Move panel's dock layout from old dock manager to new dock manager.  
                            pr = dockPanel1.GetType().GetProperty("DockLayout",
                                BindingFlags.Instance | BindingFlags.NonPublic);
                            if (pr != null)
                            {
                                var dockLayout = pr.GetValue(dockPanel1, null) as DockLayout;

                                pr = form.DockManager.GetType().GetProperty("LayoutManager",
                                    BindingFlags.Instance | BindingFlags.NonPublic);
                                if (pr != null)
                                {
                                    var layoutManager = pr.GetValue(oldDockManager, null) as DockLayoutManager;
                                    if (layoutManager != null) layoutManager.RemoveLayout(dockLayout);

                                    layoutManager = pr.GetValue(form.DockManager, null) as DockLayoutManager;

                                    if (layoutManager != null)
                                    {
                                        var mi = layoutManager.GetType().GetMethod("AddLayoutCore",
                                            BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder,
                                            new[] {typeof(LayoutInfo)}, null);

                                        if (mi != null) mi.Invoke(layoutManager, new object[] {dockLayout});
                                    }
                                }
                            }

                            break;
                        }
                }
        }
    }
}