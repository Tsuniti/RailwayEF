using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayEF.Entities;

public class Ticket
{
    public int Id { get; set; }
    public int SeatNumber { get; set; }
    public double Price { get; set; }
    public bool IsPurchased { get; set; } = false;

    public int FlightId { get; set; }
    public Flight Flight { get; set; }
}
