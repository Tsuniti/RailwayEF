using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayEF.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public List<Flight> DepartureFlights { get; set; } = new List<Flight>();
    public List<Flight> ArrivalFlights { get; set; } = new List<Flight>();
}
