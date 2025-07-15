using System;
using System.Windows;
using System.Data.SqlClient;

namespace E_Scooter
{
    public partial class MainWindow : Window
    {
        private const double BaseFareMinutes = 1.00;
        private const double PricePerMinute = 0.25;

        private const double PricePerKilometer = 1.00;

        private const string connectionString = @"Server=MFT252NB;Database=EScooterDB;Trusted_Connection=True;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MinutesInput.Text = string.Empty;
            FareResult.Text = string.Empty;
            RadioMinutes.IsChecked = false;
            RadioKilometers.IsChecked = false;
        }


        private void CalculateFare_Click(object sender, RoutedEventArgs e)
        {
            FareResult.Text = "";

            if (!double.TryParse(MinutesInput.Text, out double value) || value < 0)
            {
                FareResult.Text = "Enter a valid number";
                return;
            }

            double fare;
            string mode;

            if (RadioMinutes.IsChecked == true)
            {
                fare = BaseFareMinutes + (value * PricePerMinute);
                mode = "Minuten";
                FareResult.Text = $"Amount Due: {fare:F2} € ";
            }
            else if (RadioKilometers.IsChecked == true)
            {
                fare = BaseFareMinutes + (value * PricePerKilometer);
                mode = "Kilometer";
                FareResult.Text = $"Amount Due: {fare:F2} € ";
            }
            else
            {
                FareResult.Text = "Please select a mode.";
                return;
            }

            SaveRideToDatabase(value, fare, mode);
        }

        private void SaveRideToDatabase(double value, double fare, string mode)
        {
            string tableName = mode == "Minuten" ? "RidesMin" : "RidesKm";
            string columnName = mode == "Minuten" ? "Minutes" : "Kilometers";

            string query = $"INSERT INTO {tableName} ({columnName}, Fare, RideDate) VALUES (@Value, @Fare, @RideDate)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Value", value);
                command.Parameters.AddWithValue("@Fare", fare);
                command.Parameters.AddWithValue("@RideDate", DateTime.Now);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Speichern: " + ex.Message);
                }
            }
        }

    }
}


