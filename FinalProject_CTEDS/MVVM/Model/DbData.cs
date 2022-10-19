using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FinalProject_CTEDS.MVVM.Model
{
    public class DbData
    {
        public static string myConn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documentos\Projetos\FinalProject_CTEDS\FinalProject_CTEDS\dbGrid.mdf;Integrated Security=True";

        public static SqlConnection ConnectToDb()
        {
            SqlConnection dbData = new SqlConnection();
            dbData.ConnectionString = myConn;

            try
            {
                dbData.Open();
                Console.WriteLine("Conectado!");
            }
            catch (SqlException)
            {
                MessageBox.Show("ERRO AO CONECTAR NA BASE DE DADOS");
            }

            return dbData;
        }

        public static DataTable SearchDb()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            using (cmd.Connection = ConnectToDb())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "select  * from tableGrid";
                

                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Erro ao acessar o banco de Dados!");
                }
            }
            return dt;

        }

        public static void InsertDb(string title,string text,string author, DateTime thisDay)
        { 
            //Console.WriteLine(thisDay.ToString());
            SqlCommand cmd = new SqlCommand();
            using (cmd.Connection = ConnectToDb())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO tableGrid (Title, Text, Date, Author) VALUES (@title,@text,@date,@author)";
                cmd.Parameters.AddWithValue("@date", thisDay.ToString());
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@text", text);
                cmd.Parameters.AddWithValue("@author", author);
                try
                {
                    cmd.ExecuteNonQuery();
                    MainWindow.MainElement.txtDate.Text = thisDay.ToString();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Erro ao acessar o banco de Dados!");
                }
            }
        }

        public static void DeleteFromDb(int id)
        {
            SqlCommand cmd = new SqlCommand();
            using (cmd.Connection = ConnectToDb())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "DELETE from tableGrid where Id=@id";
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Erro ao acessar o banco de Dados!");
                }
            }
        }

        public static void UpdateDb(int id, string title, string text, string author, DateTime thisDay)
        {

            //MessageBox.Show(id.ToString());
            //Console.WriteLine(thisDay.ToString());
            SqlCommand cmd = new SqlCommand();
            using (cmd.Connection = ConnectToDb())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tableGrid SET Title= @title , Text= @text , Date= @date , Author= @author WHERE Id=@id";
                cmd.Parameters.AddWithValue("@date", thisDay.ToString());
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@text", text);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    cmd.ExecuteNonQuery();
                    MainWindow.MainElement.txtDate.Text = thisDay.ToString();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Erro ao acessar o banco de Dados!");
                }
            }
        }

    }
}
