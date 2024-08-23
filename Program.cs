using RailwayEF;

using(var db = new ApplicationContext())
{
    // db.CreateNewFlight("Kyiv", "Poltava", new DateTime(2024, 7, 14, 8, 0, 0));
    // db.CreateNewFlight("Lviv", "Poltava", new DateTime(2024, 7, 14, 9, 0, 0));
    // db.CreateNewFlight("Odesa", "Poltava", new DateTime(2024, 7, 14, 10, 0, 0));
    // db.CreateNewFlight("Poltava", "Odesa", new DateTime(2024, 7, 14, 10, 0, 0));

    foreach (var item in db.GetAllFlightsByArrivalCityName("Poltava"))
        Console.WriteLine($"#{item.Id} | {db.GetCityById(item.DepartureCityId).Name} => {db.GetCityById(item.ArrivalCityId).Name} | {item.DepartureTime}");


    db.BuyTicket(1);
}
