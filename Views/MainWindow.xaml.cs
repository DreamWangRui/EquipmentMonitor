using System.Windows;

namespace EquipmentMonitor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            if (DataContext is System.IDisposable disp)
            {
                disp.Dispose();
            }
            base.OnClosed(e);
        }
    }
}