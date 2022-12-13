using Microsoft.AspNetCore.Mvc;
using FlightData;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace FlightAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {

        private readonly ILogger<FlightsController> _logger;

        public FlightsController(ILogger<FlightsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{date}")]
        public string GetFlightsFromDatabase(DateTime date)
        {
            string connectionString = "server=127";
            // Connect to the MySQL database
            connectionString = @"server=lin-13041-7784-mysql-primary.servers.linodedb.net;user id=linroot;password=7ZmXl9xm9J@qsBIZ;database=Departures";


            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {

                    return "Error ... " + ex.ToString() + "\n StatusCode: 500, Internal Server Error" + "\n\n" + "Connection string: " + connectionString;
                }



                // Build the SQL query
                string query = "SELECT * FROM Flights WHERE DATE(Planned) = @date";

                // Execute the query and retrieve the flights
                var flights = new List<Flight>();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@date", date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a Flight object for each record in the result set
                            var flight = new Flight
                            {
                                Rute = reader.GetString("Rute"),
                                DepartureAirport = reader.GetString("DepartureAirport"),
                                ArrivalAirport = reader.GetString("ArrivalAirport"),
                                Planned = reader.GetDateTime("Planned"),
                                Estimated = reader.IsDBNull("Estimated") ? (DateTime?)null : reader.GetDateTime("Estimated"),
                                status_kl = reader.GetString("status_kl"),
                                status_en = reader.GetString("status_en"),
                                status_da = reader.GetString("status_da"),
                                FlightHash = reader.GetString("FlightHash"),
                                ArrivalICAO = reader.GetString("ArrivalICAO"),
                                DepartureICAO = reader.GetString("DepartureICAO"),
                            };

                            flight.Planned = flight.Planned.AddHours(-3);
                            if(flight.Estimated != null)
                            {
                                flight.Estimated = flight.Estimated.Value.AddHours(-3);
                            }

                            var properties = flight.GetType().GetProperties();

                            // Iterate over the array of properties
                            foreach (var property in properties)
                            {
                                // Get the value of the property
                                var value = property.GetValue(flight);

                                // Print the property name and value
                                Console.WriteLine($"{property.Name}: {value}");
                            }
                            flights.Add(flight);
                            
                        }
                    }
                }
                connection.Close();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Include;
                return JsonConvert.SerializeObject(flights, settings);
            }
            

            
        }


        [HttpPost("{Hash}-{roomnumber}")]
        public void PostRoom(string Hash, string roomnumber)
        {
            // Create the connection string
            string connectionString = "server=lin-13041-7784-mysql-primary.servers.linodedb.net;user id=linroot;password=7ZmXl9xm9J@qsBIZ;database=Departures";

            // Open the connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create the INSERT INTO statement
                string query = "INSERT INTO Rooms (FlightHash, RoomNumber) VALUES (@Hash, @roomnumber)";

                // Create a command using the query and the connection
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add the parameters and their values to the command
                    command.Parameters.AddWithValue("@Hash", Hash);
                    command.Parameters.AddWithValue("@roomnumber", roomnumber);

                    // Execute the query
                    command.ExecuteNonQuery();
                }
            }


        }
        
        

    }


}

