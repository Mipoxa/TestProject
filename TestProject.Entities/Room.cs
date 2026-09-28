using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Entities;
//Сутність Room для зберігання інформації про кімнати, їх розмір та ціну
public class Room
{
    public Guid Id {get; init;} = Guid.NewGuid();
    public string Name {get; private set;} = string.Empty;
    public int Size {get; private set;}
    public decimal Price {get; private set;} // Ціна за годину
    private readonly List<Service> availableServices = new(); //Сервіси
    public IReadOnlyCollection<Service> AvailableServices => availableServices.AsReadOnly();
    public Room(string name, int capacity, decimal baseHourlyRate, IEnumerable<Service>? services = null)
    {
        Update(name, capacity, baseHourlyRate);
        if (services != null)
        {
            availableServices.AddRange(services);
        }
    }
    public void AddService(Service service)
    {
        if (availableServices.Any(s => s.Id == service.Id))
        {
            throw new InvalidOperationException("Service already exists!");
        }
        availableServices.Add(service);
    }
    public void RemoveService(Guid serviceId)
    {
        var service = availableServices.FirstOrDefault(fservice => fservice.Id == serviceId);
        if (service == null)
        {
            throw new InvalidOperationException("Service not found!");
        }
        availableServices.Remove(service);
    }
    public bool HasService(Guid serviceId) => availableServices.Any(s => s.Id == serviceId);

    public void AreServicesAvailable(IEnumerable<Guid> serviceIds)
    {
        List<Guid> missing = serviceIds.Where(id => !HasService(id)).ToList();
        if (missing.Count > 0)
        {
            throw new InvalidOperationException("Service not available!");
        }
    }
    public void Update(string name, int size, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Invalid name!", nameof(name));
        if (size <= 0)
            throw new ArgumentException("Invalid size!", nameof(size));
        if (price <= 0)
            throw new ArgumentException("Invalid price!", nameof(price));
        Name = name;
        Size = size;
        Price = price;
    }
}