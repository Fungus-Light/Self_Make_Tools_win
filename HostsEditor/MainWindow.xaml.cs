using System;
using System.Collections.Generic;
using System.IO;
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

namespace HostsEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            isLoaded = false;
            InitializeComponent();
        }

        bool isLoaded = false;

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = Environment.SystemDirectory + "\\drivers\\etc\\hosts";
            if (File.Exists(path))
            {
                //File.SetAttributes(path, File.GetAttributes(path) & (~FileAttributes.ReadOnly));//取消只读
                StreamReader reader = new StreamReader(path);
                string str = reader.ReadToEnd();
                HContent.Text = str;
                reader.Close();
                isLoaded = true;
            }
            
        }

        private void enableBtn_Click(object sender, RoutedEventArgs e)
        {
            bool editable = HContent.IsEnabled;
            HContent.IsEnabled = !editable;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isLoaded == false)
            {
                MessageBox.Show("Please load first!!!");
                return;
            }
            string path = Environment.SystemDirectory + "\\drivers\\etc\\hosts";

            if (File.Exists("hosts.bak"))
            {
                System.IO.File.SetAttributes("hosts.bak", FileAttributes.Normal);
                File.Delete("hosts.bak");
            }
            File.Copy(path,"hosts.bak");
            if (File.Exists("hosts.bak"))
            {
                MessageBox.Show("baked the old file in ./host.bak");
            }
            else
            {
                MessageBox.Show("Error Happended Cannot bake the hosts file!!!");
                return;
            }
            System.IO.File.SetAttributes(path, FileAttributes.Normal);
            File.WriteAllText(path, HContent.Text);
            System.IO.File.SetAttributes(path,File.GetAttributes(path)|FileAttributes.ReadOnly);
            MessageBox.Show("Save Done!!!");
        }

        private void helpBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("By Fungus-Light\nhttps://github.com/Fungus-Light");
        }
    }
}
