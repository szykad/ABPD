namespace ABPD_2;

public class GasContainer : Container, IHazardNotifier
{
    public double Pressure { get; private set; } 
    public bool AnyHazard { get; set; }  

    public GasContainer(string serialNumber, double maxCapacity, double weight, double height, double depth, double pressure, bool anyHazard)
        : base(serialNumber, maxCapacity, weight, height, depth)
    {
        Pressure = pressure;
        AnyHazard = anyHazard;
    }
    public override void Unload()
    {
        double trashLoad = CurrentLoad * 0.05;
        Console.WriteLine($"Opróżniamy kontener {SerialNumber}, pozostawiając {trashLoad} kg ładunku");
        CurrentLoad = trashLoad;
    }

    public void CheckHazard(string textMassage, string serialNumber)
    {
        Console.WriteLine($"Uwaga! Niebezpieczeństwo! {serialNumber}: {textMassage}");
    }
    
    public override void Load(double amount)
    {
        if (CurrentLoad + amount > MaxCapacity)
        {
            CheckHazard("Próba załadowania ponad dozwoloną pojemność kontenera", SerialNumber);
            throw new Exception($"Nie można załadować więcej niż {MaxCapacity} kg do kontenera {SerialNumber}");
        }

        CurrentLoad += amount;
        Console.WriteLine($"Załadowano {amount} kg do kontenera {SerialNumber}. Aktualny ładunek: {CurrentLoad} kg");
    }
}