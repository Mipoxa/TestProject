using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Entities;
//Сутність Booking для зберігання інформації про записи.
public class Booking
{
    public Guid Id {get; init;} = Guid.NewGuid();
    public Guid RoomId {get; private set; } //Id Прив'язаної кімнати
    public Room? Room { get; private set; } //Прив'язана кімната
    public ICollection<Service> Services { get; private set; } = new List<Service>(); //Прив'язані сервіси
    public DateTime StartTime {get; private set;}
    public DateTime EndTime {get; private set;}
    public decimal Price {get; private set;}
    public DateTime CreatedAt {get; init;} = DateTime.UtcNow; //Коли запис був зареєстрований
    private Booking(){}
    public Booking(Guid roomId, DateTime startTime, DateTime endTime, decimal price, IEnumerable<Service> services)
    {
        RoomId = roomId;
        StartTime = startTime;
        EndTime = endTime;
        Price = price;
        Services = services.ToList();
    }
}