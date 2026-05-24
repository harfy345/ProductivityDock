using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Navigation;
namespace ProductivityApp.UI
{
    /// <summary>
    /// Interaction logic for BlockWindowRecommendationPanel.xaml
    /// </summary>
    public partial class BlockWindowRecommendationPanel : System.Windows.Controls.UserControl
    {
        public BlockWindowRecommendationPanel()
        {
            InitializeComponent();
        }


private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = e.Uri.AbsoluteUri,
            UseShellExecute = true
        });

        e.Handled = true;
    }
}
}
