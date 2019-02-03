using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

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

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            string file;
            var dialog = new OpenFileDialog();
            dialog.Filter = "(*.mp3)|*.mp3";
            if(dialog.ShowDialog() == true)
            {
                file = dialog.FileName;
            }
            //TODO: Get textbox control from stackpanel  and save filename to it.
            var parent = this.Parent;
            
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var audioConcatenator = new AudioConcatenator();

            var files = new List<string>();
            foreach(var selectedFile in FileStack.Children)
            {
                if(selectedFile is StackPanel)
                {
                    var selectedFileStack = (StackPanel) selectedFile;
                    foreach(var property in selectedFileStack.Children)
                    {
                        if( property.GetType() == typeof(TextBox))
                        {
                            var textbox = (TextBox)property;
                            if(string.IsNullOrWhiteSpace(textbox.Text))
                            {
                                files.Add(textbox.Text);
                            }
                        }
                    }
                }
            }

            var output = audioConcatenator.Concatenate(files.ToArray());
            using (var fileStream = File.Create(@"C:\Users\truck\Music\condatenated.mp3"))
            {
                output.Seek(0, SeekOrigin.Begin);
                output.CopyTo(fileStream);
                output.Flush();
            }

        }

        private void OuputFilePicker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddFileSource_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
