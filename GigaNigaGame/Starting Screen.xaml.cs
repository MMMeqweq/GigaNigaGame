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
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.K)
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
