using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Controls;

namespace PdfInterpreter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded+=(s,e)=>MyDebug.Show(this);
        }

        private void PathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// 一下是输出和输入路径选择按钮的事件处理函数
        /// </summary>
        /// 
        //选择要输入的文件路径
        private void Input_Path_Choose_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "所有文件(*.*)|*.*", // 设置文件过滤器
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) // 设置初始目录
            };

            // 显示对话框
            if (openFileDialog.ShowDialog() == true)
            {
                // 获取用户选择的文件路径
                string selectedPath = openFileDialog.FileName;
                // 将路径显示在 TextBox 中
                InputPathTextBox.Text = selectedPath;
            }
        }

        //选择文件输出路径
        private void Output_Path_Choose_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "所有文件(*.*)|*.*", // 设置文件过滤器
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) // 设置初始目录
            };

            // 显示对话框
            if (openFileDialog.ShowDialog() == true)
            {
                // 获取用户选择的文件路径
                string selectedPath = openFileDialog.FileName;
                // 将路径显示在 TextBox 中
                OutputPathTextBox.Text = selectedPath;
            }
        }
        private async void Convert_Click(object sender, RoutedEventArgs e)
        {

            string inputPath = InputPathTextBox.Text;
            string outputPath = OutputPathTextBox.Text;

            PdfParser parser = new();

            // 检查输入路径和输出路径是否为空
            if (string.IsNullOrWhiteSpace(inputPath) || string.IsNullOrWhiteSpace(outputPath))
            {
                MessageBox.Show("请输入输入路径和输出路径", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TestTextBox.Text = await parser.FindPdf(inputPath, outputPath);
            MyDebug.Log(TestTextBox.Text);
        }
    }
}