namespace ABPD_2;

public abstract class Container(string serialNumber, double maxCapacity, double weight, double height, double depth)
{
    public string SerialNumber { get; set; } = serialNumber;
    public double MaxCapacity { get; set; } = maxCapacity;
    public double CurrentLoad { get; set; } = 0;
    private double Weight { get; set; } = weight;
    private double Height { get; set; } = height;
    private double Depth { get; set; } = depth;

    public virtual void Load(double amount)
    {
        if (CurrentLoad + amount > MaxCapacity)
        {
            throw new OverfillException($"Przekroczono maksymalna pojemność kontenera {SerialNumber} " +
                                        $"Maksymalna pojemnosć to {MaxCapacity} kg.");
        }

        CurrentLoad += amount;
    }


    public virtual void Unload()
    {
        CurrentLoad = 0;
    }
}

public class OverfillException(string message) : Exception(message);