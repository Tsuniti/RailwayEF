using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RailwayEF.Entities;
using System.Diagnostics;

namespace RailwayEF;

public class ApplicationContext : DbContext
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Flight>(entity =>
        {
            entity
            .HasOne(flight => flight.DepartureCity)
            .WithMany(city => city.DepartureFlights)
            .HasForeignKey(flight => flight.DepartureCityId);

            entity
            .HasOne(flight => flight.ArrivalCity)
            .WithMany(city => city.ArrivalFlights)
            .HasForeignKey(flight => flight.ArrivalCityId);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity
            .HasOne(ticket => ticket.Flight)
            .WithMany(flight => flight.Flights)
            .HasForeignKey(ticket => ticket.FlightId);
        });


        modelBuilder.Entity<City>().HasData(
            new City { Id = 1, Name = "Kyiv", Latitude = 50.4501, Longitude = 30.5234 },
            new City { Id = 2, Name = "Lviv", Latitude = 49.8397, Longitude = 24.0297 },
            new City { Id = 3, Name = "Odesa", Latitude = 46.4825, Longitude = 30.7233 },
            new City { Id = 4, Name = "Dnipro", Latitude = 48.4647, Longitude = 35.0462 },
            new City { Id = 5, Name = "Kharkiv", Latitude = 49.9935, Longitude = 36.2304 },
            new City { Id = 6, Name = "Zaporizhzhia", Latitude = 47.8388, Longitude = 35.1396 },
            new City { Id = 7, Name = "Mykolaiv", Latitude = 46.9750, Longitude = 31.9946 },
            new City { Id = 8, Name = "Vinnytsia", Latitude = 49.2331, Longitude = 28.4682 },
            new City { Id = 9, Name = "Poltava", Latitude = 49.5883, Longitude = 34.5514 },
            new City { Id = 10, Name = "Chernihiv", Latitude = 51.4982, Longitude = 31.2893 },
            new City { Id = 11, Name = "Chernivtsi", Latitude = 48.2908, Longitude = 25.9345 },
            new City { Id = 12, Name = "Kherson", Latitude = 46.6354, Longitude = 32.6169 },
            new City { Id = 13, Name = "Ivano-Frankivsk", Latitude = 48.9226, Longitude = 24.7103 },
            new City { Id = 14, Name = "Sumy", Latitude = 50.9077, Longitude = 34.7981 },
            new City { Id = 15, Name = "Ternopil", Latitude = 49.5535, Longitude = 25.5948 },
            new City { Id = 16, Name = "Uzhhorod", Latitude = 48.6208, Longitude = 22.2879 },
            new City { Id = 17, Name = "Zhytomyr", Latitude = 50.2649, Longitude = 28.6767 },
            new City { Id = 18, Name = "Rivne", Latitude = 50.6199, Longitude = 26.2516 },
            new City { Id = 19, Name = "Lutsk", Latitude = 50.7472, Longitude = 25.3254 },
            new City { Id = 20, Name = "Kropyvnytskyi", Latitude = 48.5079, Longitude = 32.2623 }
            );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=RailwayDB.db");
    }

    public async Task CreateNewFlight(string departureCityName, string arrivalCityName, DateTime departureTime)
    {
        City DepartureCity = await Cities.FirstOrDefaultAsync(c => c.Name == departureCityName);
        City ArrivalCity = await Cities.FirstOrDefaultAsync(c => c.Name == arrivalCityName);

        if (DepartureCity == null || ArrivalCity == null)
            throw new ArgumentException("Inaccessible city for flight indicated");

        var NewFlight = new Flight
            { DepartureCityId = DepartureCity.Id, ArrivalCityId = ArrivalCity.Id, DepartureTime = departureTime };

        await AddAsync(NewFlight);
        await SaveChangesAsync();


        // Вычислю расстояние между городами в метрах, умножаю на 1.2, округляю до двух чисел после запятой
        double TicketPrice = Math.Round(GeoPoint.Distance(ArrivalCity, DepartureCity) * 1.2, 2);
        int NewFlightId = NewFlight.Id;

        for (int i = 0; i <= 60; ++i)
        {
            await AddAsync(new Ticket { SeatNumber = i, Price = TicketPrice, FlightId = NewFlightId });
        }

        await SaveChangesAsync();
    }

    public City GetCityById(int id)
    {
        return Cities.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Flight> GetAllFlightsByArrivalCityName(string name)
    {
        var city = Cities.FirstOrDefault(c => c.Name == name);

        if (city == null)
            throw new ArgumentException("Inaccessible city for GetAllFlightsByArrivalCityName");


        return Flights.Where(f => f.ArrivalCityId == city.Id);

    }

    public async Task BuyTicket (int flightId)
    {

        if (!await Flights.AnyAsync(f => f.Id == flightId))
        {
            throw new ArgumentException("There is no flight with this ID");
        }
        var availableTicket = await Tickets.FirstOrDefaultAsync(t => t.FlightId == flightId && !t.IsPurchased);

        if (availableTicket == null)
        {
            Console.WriteLine("This flight is sold out");
            return;
        }

        availableTicket.IsPurchased = true;
        Update(availableTicket);
        await SaveChangesAsync();
        Console.WriteLine($"Ticket #{availableTicket.Id} purchased successfully");
    }
}
