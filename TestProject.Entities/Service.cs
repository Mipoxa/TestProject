using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Entities;
//Сутність Service для зберігання інформації про сервіси та їх ціни
public class Service
{
    public Guid Id {get; init;} = Guid.NewGuid();
    public string Name {get; private set;} = string.Empty;
    public decimal Price {get; private set;}
    public Service(string name, decimal price)
    {
        UpdatePrice(price);
        Name = name;
    }
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            throw new ArgumentException("Invalid Price!");
        }
        Price = newPrice;
    }
}