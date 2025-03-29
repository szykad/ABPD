namespace ABPD_2;

using System;

public class FridgeContainer : Container
{
    public string ProductType { get; set; } 
    public double MinimalTemperature { get; set; }
    public double CurrentTemperature { get; private set; }

    public FridgeContainer(string serialNumber, double maxCapacity, double weight, double height, double depth,
                                 string productType, double minimalTemperature, double startingTemperature)
        : base(Generator.GenerateSerialNumber("C"), maxCapacity, weight, height, depth)
    {
        ProductType = productType;
        MinimalTemperature = minimalTemperature;
        SetTemperature(startingTemperature);
    }

    public void SetTemperature(double temperature)
    {
        if (temperature < MinimalTemperature)
        {
            throw new Exception($"Temperatura {temperature}stopni C jest za niska dla produktu {ProductType}. Minimalna temperatura: {MinimalTemperature} stopni C");
        }

        CurrentTemperature = temperature;
        Console.WriteLine($"Temperatura w kontenerze {SerialNumber} została ustawiona na {CurrentTemperature}stopni C");
    }

    public void LoadProduct(string productType, double amount)
    {
        if (!ProductType.Equals(productType))
        {
            throw new Exception($"Kontener może przechowywać tylko produkt typu {ProductType}");
        }

        if (CurrentLoad + amount > MaxCapacity)
        {
            throw new OverfillException($"Nie można załadować więcej niż {MaxCapacity} kg do kontenera {SerialNumber}");
        }

        CurrentLoad += amount;
        Console.WriteLine($"Załadowano {amount} kg produktu {ProductType} do kontenera {SerialNumber}. Aktualny ładunek: {CurrentLoad} kg");
    }

    public override void Unload()
    {
        Console.WriteLine($"Opróżnianie kontenera {SerialNumber} z {ProductType}. Pozostaje 0 kg");
        CurrentLoad = 0;
    }
}