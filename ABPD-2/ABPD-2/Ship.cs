namespace ABPD_2;

using System;
using System.Collections.Generic;

public class Ship 
{
    public string Name { get; private set; }
    public double MaxSpeed { get; private set; }
    public int MaxContainers { get; private set; }
    public double MaxWeight { get; private set; }
    public List<Container> Containers { get; private set; }

    public Ship(string name, double maxSpeed, int maxContainers, double maxWeight) {
        Name = name;
        MaxSpeed = maxSpeed;
        MaxContainers = maxContainers;
        MaxWeight = maxWeight;
        Containers = new List<Container>();
    }

    public void LoadContainer(Container container) 
    {
        if (Containers.Count >= MaxContainers) 
        {
            throw new Exception($"Nie można załadować kontenera. Statek {Name} osiągnął maksymalną liczbę kontenerów ({MaxContainers}).");
        }

        double totalWeight = GetTotalWeight() + container.CurrentLoad;

        if (totalWeight > MaxWeight * 1000) 
        {
            throw new Exception($"Nie można załadować kontenera. Przekroczono maksymalną wagę ładunku ({MaxWeight} ton) na statku {Name}.");
        }

        Containers.Add(container);
        Console.WriteLine($"Kontener {container.SerialNumber} został załadowany na statek {Name}.");
    }

    public void RemoveContainer(string serialNumber) 
    {
        Container container = Containers.Find(c => c.SerialNumber.Equals(serialNumber, StringComparison.OrdinalIgnoreCase));

        if (container == null) 
        {
            throw new Exception($"Kontener o numerze seryjnym {serialNumber} nie został znaleziony na statku {Name}.");
        }

        Containers.Remove(container);
        Console.WriteLine($"Kontener {serialNumber} został usunięty ze statku {Name}.");
    }
    
    public double GetTotalWeight() 
    {
        double totalWeight = 0;

        foreach (var container in Containers) 
        {
            totalWeight += container.CurrentLoad; 
        }

        return totalWeight;
    }
    
    public void TransferContainerTo(Ship targetShip, string serialNumber) 
    {
        Container container = Containers.Find(c => c.SerialNumber.Equals(serialNumber, StringComparison.OrdinalIgnoreCase));

        if (container == null) 
        {
            throw new Exception($"Kontener o numerze seryjnym {serialNumber} nie został znaleziony na statku {Name}.");
        }

        try 
        {
            targetShip.LoadContainer(container);
            Containers.Remove(container);
            Console.WriteLine($"Kontener {serialNumber} został przeniesiony ze statku {Name} na statek {targetShip.Name}.");
        }
        catch (Exception ex) {
            Console.WriteLine($"Błąd przenoszenia kontenera: {ex.Message}");
        }
    }
    
    public void DisplayInfo() 
    {
        Console.WriteLine($"=== Informacje o statku: {Name} ===");
        Console.WriteLine($"Maksymalna prędkość: {MaxSpeed} węzłów");
        Console.WriteLine($"Maksymalna liczba kontenerów: {MaxContainers}");
        Console.WriteLine($"Maksymalna waga ładunku: {MaxWeight} ton");
        Console.WriteLine($"Aktualna liczba kontenerów: {Containers.Count}");
        Console.WriteLine($"Aktualna waga ładunku: {GetTotalWeight() / 1000} ton");
        Console.WriteLine("=== Lista kontenerów na statku ===");
        foreach (var container in Containers) 
        {
            Console.WriteLine($"Kontener: {container.SerialNumber}, Typ: {container.GetType().Name}, Ładunek: {container.CurrentLoad} kg");
        }
    }
}