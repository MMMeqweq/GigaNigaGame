using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data;
using System.Data.OleDb;
using System.Runtime.CompilerServices;

namespace GigaNigaGame
{
    /// <summary>
    /// Interaction logic for Starting_Screen.xaml
    /// </summary>
    public partial class Starting_Screen : Window
    {
        public Starting_Screen()
        {
            InitializeComponent();
            Lists.CurrentPlayerId = -1;
            Leader();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        public static int GetOrCreatePlayer(string playerName)
        {
            // Check if player exists
            string selectSql =
                $"SELECT ID FROM [Players + score] WHERE PlayerName = '{playerName}'";

            DataTable dt = DAL.GetDataTable(selectSql);

            // Player exists
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["ID"]);
            }

            // Player does not exist -> insert
            string insertSql =
                $"INSERT INTO [Players + score] (PlayerName) VALUES ('{playerName}')";

            DAL.ExecuteNonQuery(insertSql);

            // Get newly created player ID
            DataTable newDt =
                DAL.GetDataTable(
                    $"SELECT ID FROM [Players + score] WHERE PlayerName = '{playerName}'");

            return Convert.ToInt32(newDt.Rows[0]["ID"]);
        }

        private void Leader()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\royna\source\repos\GigaNigaGame\GigaNigaGame\Folders\DataBase\DataBase.accdb;Persist Security Info=True";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT TOP 3 [PlayerName], [PlayerWins] FROM [Players + score] ORDER BY [PlayerWins] DESC";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    int rank = 1;

                    while (reader.Read())
                    {
                        string name = reader["PlayerName"].ToString();
                        int score = Convert.ToInt32(reader["PlayerWins"]);

                        string leaderboardText = $"{rank} -- {name}:{score}";

                        if (rank == 1)
                            this.Num1.Text = leaderboardText; 
                        if (rank == 2)
                            this.Num2.Text = leaderboardText; 
                        if (rank == 3)
                            this.Num3.Text = leaderboardText; 



                        rank++;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Lists.CurrentPlayerId = GetOrCreatePlayer(NameTextBox.Text);
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }
    }
}
