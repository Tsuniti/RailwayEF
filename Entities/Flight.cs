using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayEF.Entities;

public class Flight
{
    public int Id { get; set; }
    public DateTime DepartureTime { get; set; }
    
    public int DepartureCityId { get; set; }
    public City DepartureCity { get; set; }

    public int ArrivalCityId { get; set; }
    public City ArrivalCity { get; set; }

    public List<Ticket> Flights { get; set; } = new List<Ticket>();
}
