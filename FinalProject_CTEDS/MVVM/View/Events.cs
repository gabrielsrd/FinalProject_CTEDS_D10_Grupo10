using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FinalProject_CTEDS.MVVM.View
{
    internal class Events
    {
        public static class ClickedButton
        {
            public static Button actual = new Button();
        }
        public static void Button_Clicked(object sender, RoutedEventArgs e)
        {
            
            ClickedButton.actual.Background = Brushes.White;
            Button btn = sender as Button;
            btn.Background = Brushes.Gray;
            ClickedButton.actual = btn;
            
            foreach (DataRow row in Model.DbData.SearchDb().Rows)
            {
                if (row["Id"].ToString() == btn.Name.Replace("ID", ""))
                {
                    MainWindow.MainElement.rtbEditor.Document.Blocks.Clear();
                    string rtfText = row["Text"].ToString();
                    byte[] byteArray = Encoding.ASCII.GetBytes(rtfText);
                    using (MemoryStream ms = new MemoryStream(byteArray))
                    {
                        TextRange tr = new TextRange(MainWindow.MainElement.rtbEditor.Document.ContentStart, MainWindow.MainElement.rtbEditor.Document.ContentEnd);
                        tr.Load(ms, DataFormats.Rtf);
                    }

                    //MainWindow.MainElement.rtbEditor.Text = row["Text"].ToString();
                    MainWindow.MainElement.txtDate.Text = row["Date"].ToString();
                    MainWindow.MainElement.txtAuthor.Text = row["Author"].ToString();
                    MainWindow.MainElement.txtTitle.Text = row["Title"].ToString();
                    MainWindow.MainElement.txtId.Text = row["id"].ToString();
                    MainWindow.MainElement.newTxt = false;
                    MainWindow.MainElement.saveButton.Content = "Atualizar";
                    MainWindow.MainElement.removeButton.Visibility = Visibility.Visible;
                    break;
                }
            }
            //MVVM.ViewModel.LoadFromDb.RemoveTitle(btn.Name);


        }
    }
}
