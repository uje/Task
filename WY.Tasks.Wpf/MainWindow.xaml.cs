using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WY.Tasks.Wpf {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);
            base.Hide();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e) {
            var tm=new Tasks.Core.TaskManager();
            tm.Initialize();
        }
    }
}
