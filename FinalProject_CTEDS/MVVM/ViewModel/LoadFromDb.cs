using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace FinalProject_CTEDS.MVVM.ViewModel
{
    public static class NoteBlockData
    {
        public static DataTable table = new DataTable();
    }
    internal class LoadFromDb
    {
        public static void LoadTitle()
        {
            DataTable table = Model.DbData.SearchDb();
            foreach (DataRow dataRow in table.Rows)
            {
                //MessageBox.Show(dataRow["Title"].ToString());
                Button titleButton = new Button();
                titleButton.Content = dataRow["Title"].ToString();
                titleButton.Height = 40;
                titleButton.Margin = new Thickness(20, 10, 20, 10);
                titleButton.Foreground = Brushes.Black;
                titleButton.Background = Brushes.White;
                string name = "ID" + dataRow["Id"].ToString();
                titleButton.Name = name;
                titleButton.Click += new RoutedEventHandler(View.Events.Button_Clicked);
                MainWindow.MainElement.stackPanel.RegisterName(titleButton.Name, titleButton);
                MainWindow.MainElement.stackPanel.Children.Add(titleButton);
                NoteBlockData.table = table;
            }
        }

        public static void RemoveTitle(string NameID)
        {
            object a = new object();
            RoutedEventArgs b = new RoutedEventArgs();
            UIElement child = MainWindow.MainElement.stackPanel.FindName("ID"+NameID) as UIElement;
            if (child == null)
                MessageBox.Show(NameID);
            MainWindow.MainElement.stackPanel.Children.Remove(child);
            MVVM.Model.DbData.DeleteFromDb(int.Parse(NameID.Replace("ID", "")));
            MainWindow.MainElement.newButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));


        }

        public static void LoadOneTitle(int ?id)
        {
            DataTable table = Model.DbData.SearchDb();
            DataRow dataRow = table.Rows[table.Rows.Count - 1];


            if (id == null)
            {
                //MessageBox.Show(dataRow["Title"].ToString());
                Button titleButton = new Button();
                titleButton.Content = dataRow["Title"].ToString();
                titleButton.Height = 40;
                titleButton.Margin = new Thickness(20, 10, 20, 10);
                titleButton.Foreground = Brushes.Black;
                MVVM.View.Events.ClickedButton.actual.Background = Brushes.White;
                titleButton.Background = Brushes.Gray;
                string name = "ID" + dataRow["Id"].ToString();
                titleButton.Name = name;
                titleButton.Click += new RoutedEventHandler(View.Events.Button_Clicked);
                MainWindow.MainElement.stackPanel.RegisterName(titleButton.Name, titleButton);
                MainWindow.MainElement.stackPanel.Children.Add(titleButton);
                NoteBlockData.table = table;
                MainWindow.MainElement.txtId.Text = dataRow["Id"].ToString();
            }
            else 
            {
                object child = MainWindow.MainElement.stackPanel.FindName("ID" + id.ToString());
                if (child is Button)
                {
                    Button wantedChild = child as Button;
                    wantedChild.Content= MainWindow.MainElement.txtTitle.Text;
                }
                
            }

            

        }
    }
}
