using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Organizer.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DirectorySelectionButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && Directory.Exists(folderBrowserDialog.SelectedPath))
            {
                DirectoryTextBox.Text = folderBrowserDialog.SelectedPath.ToLowerInvariant();
            }
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string extension = InputWindow.InputBox("enter an extension: (*.*)", "organizer");

            if (string.IsNullOrEmpty(extension))
            {
                return;
            }

            if (extension.Contains("(") || extension.Contains(")"))
            {
                extension = extension.Replace("(", "").Replace(")", "");
            }

            if (!extension.Contains("."))
            {
                extension = "." + extension;
            }

            if (!extension.Contains("*"))
            {
                extension = "*" + extension;
            }

            ExtensionListBox.Items.Add(new ListBoxItem { Content = extension });
        }

        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ExtensionListBox.Items.Remove(ExtensionListBox.SelectedItem);
        }

        private void ClearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ExtensionListBox.Items.Clear();
        }

        private void OrganizeButton_Click(object sender, RoutedEventArgs e)
        {
            string prefix = (bool)AddPrefixCheckBox.IsChecked ? PrefixTextBox.Text : "";
            string suffix = (bool)AddSuffixCheckBox.IsChecked ? SuffixTextBox.Text : "";
            DirectoryInfo directoryInfo = new DirectoryInfo(DirectoryTextBox.Text);

            if (!int.TryParse(StartingIndexTextBox.Text, out int startIndex))
            {
                startIndex = 0;
            }

            int i = startIndex;

            if (FileExtensionsCheckBox.IsChecked == true && ExtensionListBox.Items.Count > 0)
            {
                foreach (ListBoxItem lbi in ExtensionListBox.Items)
                {
                    FileInfo[] files = directoryInfo.GetFiles(lbi.Content.ToString());
                    foreach (FileInfo file in files)
                    {
                        file.MoveTo(Path.Combine(DirectoryTextBox.Text, prefix + i++ + suffix + file.Extension));
                    }

                    if (ResetIndexCheckBox.IsChecked == true && ResetIndexCheckBox.IsEnabled == true)
                    {
                        i = startIndex;
                    }
                }
                
            }
            else
            {
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.MoveTo(Path.Combine(DirectoryTextBox.Text, prefix + i++ + suffix + file.Extension));
                }
            }
        }
    }
}
