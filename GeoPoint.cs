using RailwayEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayEF;

public static class GeoPoint
{

    public static double Distance(City city1, City city2)
    {
        double R = 6371.0; // Радиус Земли в километрах

        // Конвертация градусов в радианы
        double lat1 = ToRadians(city1.Latitude);
        double lon1 = ToRadians(city1.Longitude);
        double lat2 = ToRadians(city2.Latitude);
        double lon2 = ToRadians(city2.Longitude);

        // Разница координат
        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

       
        // Вычисление формулы гаверсинуса
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Вычисление расстояния
        double distance = R * c;

        return distance;
    }

    // Функция для конвертации градусов в радианы
    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

}
