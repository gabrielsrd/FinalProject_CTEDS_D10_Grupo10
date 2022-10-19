using System;
using System.Collections.Generic;
using System.Data;
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

namespace FinalProject_CTEDS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static class MainElement
        {
            public static StackPanel stackPanel = new StackPanel();
            public static RichTextBox rtbEditor = new RichTextBox();
            public static TextBox txtTitle = new TextBox();
            public static TextBox txtId = new TextBox();
            public static TextBox txtAuthor = new TextBox();
            public static TextBox txtDate = new TextBox();
            public static Button newButton = new Button();
            public static Button saveButton = new Button();
            public static Button removeButton = new Button();
            public static Button clickButton = new Button();
            public static bool newTxt = true;
        }
        public MainWindow()
        {
            InitializeComponent();
            MainElement.stackPanel = stackName;
            MainElement.rtbEditor = rtbEditor;
            MainElement.txtTitle = txtTitle;
            MainElement.txtId = txtId;
            MainElement.txtAuthor = txtAuthor;
            MainElement.txtDate = txtDate;
            MainElement.newButton = newButton;
            MainElement.saveButton = saveButton;
            MainElement.removeButton = removeButton;
            MainElement.clickButton = newButton;
            MainElement.saveButton.Content = "Salvar";
            MainElement.removeButton.Visibility = Visibility.Hidden;
            MVVM.ViewModel.LoadFromDb.LoadTitle();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                saveButton_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                newButton_Click(sender, e);
            }
            else if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control)
            {
                removeButton_Click(sender, e);
            }
        }
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime thisDay = DateTime.Now;
            string title = MainElement.txtTitle.Text;
            string author = MainElement.txtAuthor.Text;
            string id = MainElement.txtId.Text;
            string txtBlock; 
            TextRange tr = new TextRange(MainElement.rtbEditor.Document.ContentStart, MainElement.rtbEditor.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Rtf);
                txtBlock = Encoding.ASCII.GetString(ms.ToArray());
            }

            if (MainElement.newTxt == false)
            {
                MVVM.Model.DbData.UpdateDb(int.Parse(id.Replace("ID", "")), title, txtBlock, author, thisDay);
                MVVM.ViewModel.LoadFromDb.LoadOneTitle(int.Parse(id.Replace("ID", "")));
                

            }
            else
            {
                MVVM.Model.DbData.InsertDb(title, txtBlock, author, thisDay);
                MVVM.ViewModel.LoadFromDb.LoadOneTitle(null);
                MainElement.newTxt = false;
                MainElement.saveButton.Content = "Atualizar";
                MainElement.removeButton.Visibility = Visibility.Visible;

            }
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            MVVM.View.Events.ClickedButton.actual.Background = Brushes.White;
            MainElement.rtbEditor.Document.Blocks.Clear();
            MainElement.txtTitle.Text = "";
            MainElement.txtId.Text = "";
            MainElement.txtAuthor.Text = "";
            MainElement.txtDate.Text = "";
            MainElement.newTxt = true;
            MainElement.saveButton.Content = "Salvar";
            MainElement.removeButton.Visibility=Visibility.Hidden;

        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {

            MVVM.ViewModel.LoadFromDb.RemoveTitle(MainElement.txtId.Text);
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }
    }
}
