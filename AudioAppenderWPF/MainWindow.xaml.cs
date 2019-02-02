using System.IO;
using System.Windows;

namespace AudioAppenderWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
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
