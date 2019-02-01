using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioAppenderWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var audioConcatenator = new AudioConcatenator();
            var output = audioConcatenator.Concatenate(new[]
            {
                @"C:\chapter1.mp3",
                @"C:\chapter1.mp3"
            });
            using (var fileStream = File.Create(@"C:\Users\truck\Music\condatenated.mp3"))
            {
                output.Seek(0, SeekOrigin.Begin);
                output.CopyTo(fileStream);
                output.Flush();
            }
        }
    }
}
