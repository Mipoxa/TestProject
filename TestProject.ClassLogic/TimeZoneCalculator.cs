using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.ClassLogic;

public interface ITimeZoneCalculator
{
    decimal CalculateRoomPrice(decimal baseHourlyRate, DateTime start, DateTime end);
}
public class TimeZoneCalculator : ITimeZoneCalculator
{
    public decimal CalculateRoomPrice(decimal baseHourlyRate, DateTime start, DateTime end)
    {
        if (end <= start)
        {
            throw new ArgumentException("Invalid Time!");
        }
        decimal total = 0m;
        DateTime cursor = start;
        while (cursor < end) //Розбиваємо час по відрізках, розділяємо їх на нульовій хвилині кожної години, помножуємо кожен відрізок на відповідну ціну.
        {
            var nextHourMark = cursor.Date.AddHours(cursor.Hour + 1);
            var segmentEnd = nextHourMark < end ? nextHourMark : end;
            var hours = (decimal)(segmentEnd - cursor).TotalHours;
            total += baseHourlyRate * GetMultiplier(cursor.Hour) * hours;
            cursor = segmentEnd;
        }
        return Math.Round(total, 2);
    }
    private static decimal GetMultiplier(int hour) //тут можна редагувтаи вартості часових зон
    {
        if (hour is >= 12 and < 14) return 1.15m;
        if (hour is >= 6 and < 9) return 0.90m;
        if (hour is >= 9 and < 18) return 1.00m;
        if (hour is >= 18 and < 23) return 0.80m;
        return 1.00m;
    }
}