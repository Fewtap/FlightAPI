namespace FlightData
{
    public struct Room
    {
        public string RoomNumber { get; set; }
        public string FlightHash { get; set; }

        public Room(string _rn, string _fh)
        {
            RoomNumber = _rn;
            FlightHash = _fh;
        }
    }

    public class Flight
    {
        

        public string Rute { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime Planned { get; set; }
        public DateTime? Estimated { get; set; }
        public string status_kl { get; set; }
        public string status_en { get; set; }
        public string status_da { get; set; }
        public string FlightHash { get; set; }
        public string ArrivalICAO { get; set; }
        public string DepartureICAO { get; set; }      

    }



    

    

}
