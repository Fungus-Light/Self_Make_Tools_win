using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace RealTimeNetWork
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string CurrentLanguage = "ZH";

        private NetWorkAdapter[] adapters;
        private NetWorkMonitor monitor;
        private DispatcherTimer timer;
        Help HelpForm = new Help();

        private bool ishelpon=false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MessageBar.Content = " ";
            monitor = new NetWorkMonitor();
            this.adapters = monitor.Adapters;
            /* If the length of adapters is zero, then no instance 
             * exists in the networking category of performance console.*/
            if (adapters.Length == 0)
            {
                AdapterList.Visibility = Visibility.Collapsed;
                if (CurrentLanguage=="ZH")
                {
                    MessageBox.Show("未找到任何网络设备");
                }
                else
                {
                    MessageBox.Show("No network adapters found on this computer.");
                }
                
                return;
            }
            foreach(NetWorkAdapter adapter in adapters)
            {
                this.AdapterList.Items.Add(adapter);
            }
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerCounter_Tick;
        }

        private void AdapterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            monitor.StopMonitoring();
            // Start a timer to obtain new performance counter sample every second.
            monitor.StartMonitoring(adapters[this.AdapterList.SelectedIndex]);
            if (this.timer.IsEnabled)
            {
                this.timer.Stop();
            }
            this.timer.Start();
        }

        private void TimerCounter_Tick(object sender, System.EventArgs e)
        {
            NetWorkAdapter adapter = this.adapters[this.AdapterList.SelectedIndex];
            /* The DownloadSpeedKbps and UploadSpeedKbps are double values. You can also 
             * use properties DownloadSpeed and UploadSpeed, which are long values but 
             * are measured in bytes per second. */

            if (CurrentLanguage=="ZH")
            {
                this.MessageBar.Content = String.Format("下载 {0:n} kbps", adapter.DownloadSpeedKbps) + "\n" + String.Format("上传 {0:n} kbps", adapter.UploadSpeedKbps);
            }
            else
            {
                this.MessageBar.Content = String.Format("Download {0:n} kbps", adapter.DownloadSpeedKbps) + "\n" + String.Format("UpLoad {0:n} kbps", adapter.UploadSpeedKbps);
            }
            
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            if (!ishelpon)
            {
                HelpForm.Visibility=Visibility.Visible;
                ishelpon = true;
            }
            else
            {
                HelpForm.Visibility=Visibility.Hidden;
                ishelpon = false;
            }
        }

        private void ChageLaguage(object sender, RoutedEventArgs e)
        {
            if (CurrentLanguage == "ZH")
            {
                CurrentLanguage = "EN";
                SetMessage("Current Speed","Adapters List","Settings","Quit");
            }
            else
            {
                CurrentLanguage = "ZH";
                SetMessage("当前实时网速","网络适配器列表","设置","退出");
            }
            LanguageSwitcher.Content = CurrentLanguage;
        }

        private void SetMessage(string lab_net,string lab_list,string setting,string quit)
        {
            lab_current_net.Content = lab_net;
            lab_current_list.Content = lab_list;
            //settingBTN.Content = setting;
            quitBTN.Content = quit;
        }
    }
}
