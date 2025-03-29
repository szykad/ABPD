namespace ABPD_2;

public class LiquidContainer : Container, IHazardNotifier
{
    public double LiqudAmount { get; private set; } 
    private bool AnyHazard { get; set; } 
    
    public LiquidContainer(string serialNumber, double maxCapacity, double weight, double height, double depth, bool anyHazard, double liquidAmount)
        : base(serialNumber, maxCapacity, weight, height, depth)
    {
        AnyHazard = anyHazard;
        LiqudAmount = liquidAmount;
    }
    
    public void CheckHazard(string textMessage, string serialNumber)
    {
        Console.WriteLine($"Uwaga! Niebezpieczeństwo! {serialNumber}: {textMessage}");
    }
    
    public override void Load(double amount)
    {
        double allowedCapacity;
        if (AnyHazard)
        {
            allowedCapacity = MaxCapacity * 0.5;
        }
        else
        {
            allowedCapacity = MaxCapacity * 0.9;
        }

        if (CurrentLoad + amount > allowedCapacity)
        {
            CheckHazard("Próba załadowania ponad dozwoloną pojemność kontenera!", SerialNumber);
            throw new Exception($"Nie można załadować więcej niż {allowedCapacity} kg do kontenera {SerialNumber}.");
        }

        CurrentLoad += amount;
        Console.WriteLine($"Załadowano {amount} kg do kontenera {SerialNumber}. " +
                          $"Aktualny ładunek: {CurrentLoad} kg.");
    }
}