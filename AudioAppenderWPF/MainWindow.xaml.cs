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
            if(string.IsNullOrWhiteSpace(OutputFileTextbox.Text))
            {
                //TODO: display message
                return;
            }

            string file = string.Empty;
            var dialog = new OpenFileDialog();
            dialog.Filter = "(*.mp3)|*.mp3";
            if(dialog.ShowDialog() == true)
            {
                file = dialog.FileName;
            }
            var textbox = GetButtonTextbox((Button)sender);
            textbox.Text = file;
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
                            if(!string.IsNullOrWhiteSpace(textbox.Text))
                            {
                                files.Add(textbox.Text);
                            }
                        }
                    }
                }
            }

            var output = audioConcatenator.Concatenate(files.ToArray());
            using (var fileStream = File.Create(OutputFileTextbox.Text))
            {
                output.Seek(0, SeekOrigin.Begin);
                output.CopyTo(fileStream);
                output.Flush();
            }

        }

        private void OuputFilePicker_Click(object sender, RoutedEventArgs e)
        {
            string file= string.Empty;
            var dialog = new OpenFileDialog();
            dialog.Filter = "(*.mp3)|*.mp3";
            if (dialog.ShowDialog() == true)
            {
                file = dialog.FileName;
            }
            OutputFileTextbox.Text = file;
        }

        private void AddFileSource_Click(object sender, RoutedEventArgs e)
        {

        }

        private TextBox GetButtonTextbox(Button button)
        {
            var stackPanel = (StackPanel)button.Parent;
            foreach(var child in stackPanel.Children)
            {
                if(child.GetType() == typeof(TextBox))
                    return (TextBox)child;
            }

            return null;//Have to do this becuase UIElementCollection doesn't have Linq support
        }
    }
}
