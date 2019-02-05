using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using SourceChord.FluentWPF;

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
           ErrorText.Visibility = Visibility.Hidden;

            string file = string.Empty;
            var dialog = new OpenFileDialog();
            dialog.Filter = "(*.mp3)|*.mp3";
            if (dialog.ShowDialog() == true)
            {
                file = dialog.FileName;
            }
            var textbox = GetButtonTextbox((Button)sender);
            textbox.Text = file;
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var files = new List<string>();
            foreach (var selectedFile in FileStack.Children)
            {
                if (selectedFile is StackPanel)
                {
                    var selectedFileStack = (StackPanel)selectedFile;
                    foreach (var property in selectedFileStack.Children)
                    {
                        if (property.GetType() == typeof(TextBox))
                        {
                            var textbox = (TextBox)property;
                            if (!string.IsNullOrWhiteSpace(textbox.Text))
                            {
                                files.Add(textbox.Text);
                            }
                        }
                    }
                }
            }

            if(files.Count <2)
            {
                //If there is not at least two files the tool won't do anything so just return
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "(*.mp3)|*.mp3";
            saveFileDialog.DefaultExt = "mp3";
            if (saveFileDialog.ShowDialog() == true)
            {
                var audioConcatenator = new AudioConcatenator();

                

                var output = audioConcatenator.Concatenate(files.ToArray());
                using (var fileStream = File.Create(saveFileDialog.FileName))
                {
                    output.Seek(0, SeekOrigin.Begin);
                    output.CopyTo(fileStream);
                    output.Flush();
                }
            }

        }

        private void AddFileSource_Click(object sender, RoutedEventArgs e)
        {
            var newFileSelector = new StackPanel();
            PointerTracker.SetEnabled(newFileSelector, true);
            newFileSelector.Margin = new Thickness(3);
            newFileSelector.HorizontalAlignment = HorizontalAlignment.Left;
            newFileSelector.Orientation = Orientation.Horizontal;
            newFileSelector.Width = 350;

            var textBoxRevealStyle = (Style)Application.Current.Resources["TextBoxRevealStyle"];
            var newTextbox = new TextBox
            {
                Width = 275,
                Style = textBoxRevealStyle
            };
            newFileSelector.Children.Add(newTextbox);

            var buttonRevealStyle = (Style)Application.Current.Resources["ButtonRevealStyle"];
            var systemBaseHighColorBrush = (Brush)Application.Current.Resources["SystemBaseHighColorBrush"];
            var newButton = new Button
            {
                Style = buttonRevealStyle,
                Foreground = systemBaseHighColorBrush,
                Background = AccentColors.ImmersiveSystemAccentDark1Brush,
                Content = "Open",
                Width = 75
                
            };
            newButton.Click += OpenFile_Click;
            newFileSelector.Children.Add(newButton);

            FileStack.Children.Insert(FileStack.Children.Count -1, newFileSelector);
        }

        private TextBox GetButtonTextbox(Button button)
        {
            var stackPanel = (StackPanel)button.Parent;
            foreach (var child in stackPanel.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    return (TextBox)child;
                }
            }

            return null;//Have to do this becuase UIElementCollection doesn't have Linq support
        }
    }
}
