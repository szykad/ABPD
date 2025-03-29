using System;
using System.Collections.Generic;
namespace ABPD_2;

public static class Generator
{
    private static Dictionary<string, int> containerCounters = new Dictionary<string, int>
    {
        { "C", 0 }, { "L", 0 }, { "G", 0 }  
    };
    
    private static HashSet<string> existingSerialNumbers = new HashSet<string>();

    public static string GenerateSerialNumber(string containerType)
    {
        if (!containerCounters.ContainsKey(containerType))
        {
            throw new Exception("Nieobsługiwany typ kontenera.");
        }

        containerCounters[containerType]++;
        int serialNumberPart = containerCounters[containerType];

        string newSerialNumber = $"KON-{containerType}-{serialNumberPart}";

        if (existingSerialNumbers.Contains(newSerialNumber))
        {
            throw new Exception("Numer seryjny już istnieje");
        }
        
        existingSerialNumbers.Add(newSerialNumber);

        return newSerialNumber;
    }
}
