using System;
using System.Collections.Generic;
using ABPD_2;

public class Program
{
    private static List<Container> containers = new List<Container>();
    private static List<Ship> ships = new List<Ship>();

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("=== MENU ZARZĄDZANIA STATKIEM TRANSPORTOWYM ===");
            Console.WriteLine("1. Wyświetl wszystkie kontenery");
            Console.WriteLine("2. Wyświetl informacje o statku i jego ładunku");
            Console.WriteLine("3. Dodaj kontener");
            Console.WriteLine("4. Załaduj ładunek do kontenera");
            Console.WriteLine("5. Załaduj kontener na statek");
            Console.WriteLine("6. Załaduj listę kontenerów na statek");
            Console.WriteLine("7. Usuń kontener ze statku");
            Console.WriteLine("8. Rozładuj kontener");
            Console.WriteLine("9. Zastąp kontener na statku");
            Console.WriteLine("10. Przenieś kontener między dwoma statkami");
            Console.WriteLine("11. Wyjdź");
            Console.Write("Wybierz opcję: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllContainers();
                    break;
                case "2":
                    DisplayShipInfo();
                    break;
                case "3":
                    CreateContainer();
                    break;
                case "4":
                    LoadIntoContainer();
                    break;
                case "5":
                    LoadContainerOntoShip();
                    break;
                case "6":
                    LoadContainersListOntoShip();
                    break;
                case "7":
                    RemoveContainerFromShip();
                    break;
                case "8":
                    UnloadContainer();
                    break;
                case "9":
                    ReplaceContainerOnShip();
                    break;
                case "10":
                    TransferContainerBetweenShips();
                    break;
                case "11":
                    Console.WriteLine("Dziękujemy za skorzystanie z systemu!");
                    return;
                default:
                    Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void DisplayAllContainers()
    {
        Console.WriteLine("=== LISTA KONTENERÓW ===");
        foreach (var container in containers)
        {
            Console.WriteLine(
                $"Kontener: {container.SerialNumber}, Typ: {container.GetType().Name}, Ładunek: {container.CurrentLoad} kg");
        }
    }

    private static void DisplayShipInfo()
    {
        Console.Write("Podaj nazwę statku: ");
        string shipName = Console.ReadLine();
        Ship ship = ships.Find(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));

        if (ship != null)
        {
            Console.WriteLine($"Statek: {ship.Name}");
            Console.WriteLine($"Maksymalna prędkość: {ship.MaxSpeed} węzłów");
            Console.WriteLine($"Maksymalna liczba kontenerów: {ship.MaxContainers}");
            Console.WriteLine($"Maksymalna waga ładunku: {ship.MaxWeight} ton");
            Console.WriteLine("=== LISTA KONTENERÓW ===");
            foreach (var container in ship.Containers)
            {
                Console.WriteLine(
                    $"Kontener: {container.SerialNumber}, Typ: {container.GetType().Name}, Ładunek: {container.CurrentLoad} kg");
            }
        }
        else
        {
            Console.WriteLine("Statek nie został znaleziony.");
        }
    }

    private static void CreateContainer()
    {
        Console.WriteLine("Wybierz typ kontenera do utworzenia: ");
        Console.WriteLine("1. Płynowy L");
        Console.WriteLine("2. Chłodniczy C");
        Console.WriteLine("3. Gazowy G");
        Console.Write("Wybierz opcję: ");
        string containerChoice = Console.ReadLine();

        Console.Write("Podaj maksymalną pojemność (kg): ");
        double maxCapacity = double.Parse(Console.ReadLine());

        Container newContainer;

        switch (containerChoice)
        {
            case "1":
                Console.Write("Podaj ilość płynu: ");
                double liquidAmount = double.Parse(Console.ReadLine());
                bool anyHazard;
                while (true)
                {
                    Console.Write("Czy jest to niebezpieczna ciecz? (T/N): ");
                    string hazardInput = Console.ReadLine().Trim().ToUpper();
                    if (hazardInput == "T")
                    {
                        anyHazard = true;
                        break;
                    }
                    else if (hazardInput == "N")
                    {
                        anyHazard = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy wybór. Wpisz 'T' dla Tak lub 'N' dla Nie");
                    }
                }
                string liquidSerialNumber = Generator.GenerateSerialNumber("C");

                newContainer = new LiquidContainer(liquidSerialNumber, maxCapacity, 0, 0, 0, anyHazard, liquidAmount);

                break;
            case "2":
                Console.Write("Podaj typ produktu: ");
                string productType = Console.ReadLine();
                Console.Write("Podaj wymaganą temperaturę (C): ");
                double requiredTemperature = double.Parse(Console.ReadLine());
                Console.Write("Podaj początkową temperaturę (C): ");
                double initialTemperature = double.Parse(Console.ReadLine());
                string fridgeSerialNumber = Generator.GenerateSerialNumber("C");
                newContainer = new FridgeContainer(fridgeSerialNumber, maxCapacity, 0, 0, 0, productType,
                    requiredTemperature, initialTemperature);
                break;
            case "3":
                Console.Write("Podaj ciśnienie (w atmosferach): ");
                double pressure = double.Parse(Console.ReadLine());
                string gasSerialNumber = Generator.GenerateSerialNumber("G");
                newContainer = new GasContainer(gasSerialNumber, maxCapacity, 0, 0, 0, pressure, true);
                break;
            default:
                Console.WriteLine("Nieprawidłowy wybór.");
                return;
        }

        containers.Add(newContainer);
        Console.WriteLine($"Kontener został utworzony i dodany do listy.");
    }

    private static void LoadIntoContainer()
    {
        Console.Write("Podaj numer seryjny kontenera: ");
        string serialNumber = Console.ReadLine();
        Container container =
            containers.Find(c => c.SerialNumber.Equals(serialNumber, StringComparison.OrdinalIgnoreCase));

        if (container != null)
        {
            Console.Write("Podaj masę ładunku do załadowania (kg): ");
            double loadAmount = double.Parse(Console.ReadLine());

            try
            {
                container.Load(loadAmount);
                Console.WriteLine($"Załadowano {loadAmount} kg do kontenera {container.SerialNumber}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Kontener nie został znaleziony.");
        }
    }

    private static void LoadContainerOntoShip()
    {
        Console.Write("Podaj nazwę statku: ");
        string shipName = Console.ReadLine();
        Ship ship = ships.Find(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));

        if (ship == null)
        {
            Console.WriteLine("Statek nie został znaleziony.");
            return;
        }

        Console.Write("Podaj numer seryjny kontenera: ");
        string containerSerialNumber = Console.ReadLine();
        Container container = containers.Find(c =>
            c.SerialNumber.Equals(containerSerialNumber, StringComparison.OrdinalIgnoreCase));

        if (container == null)
        {
            Console.WriteLine("Kontener nie został znaleziony.");
            return;
        }

        try
        {
            ship.LoadContainer(container);
            containers.Remove(container); 
            Console.WriteLine($"Kontener {container.SerialNumber} został załadowany na statek {ship.Name}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    private static void LoadContainersListOntoShip()
    {
        Console.Write("Podaj nazwę statku: ");
        string shipName = Console.ReadLine();
        Ship ship = ships.Find(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));

        if (ship == null)
        {
            Console.WriteLine("Statek nie został znaleziony.");
            return;
        }

        Console.WriteLine("Podaj numery seryjne kontenerów do załadowania, oddzielone przecinkami:");
        string[] containerSerialNumbers = Console.ReadLine().Split(',');

        foreach (var serialNumber in containerSerialNumbers)
        {
            Container container = containers.Find(c =>
                c.SerialNumber.Equals(serialNumber.Trim(), StringComparison.OrdinalIgnoreCase));

            if (container == null)
            {
                Console.WriteLine($"Kontener {serialNumber.Trim()} nie został znaleziony.");
                continue;
            }

            try
            {
                ship.LoadContainer(container);
                containers.Remove(container);
                Console.WriteLine($"Kontener {container.SerialNumber} został załadowany na statek {ship.Name}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas załadunku kontenera {serialNumber.Trim()}: {ex.Message}");
            }
        }
    }

    private static void RemoveContainerFromShip()
    {
        Console.Write("Podaj nazwę statku: ");
        string shipName = Console.ReadLine();
        Ship ship = ships.Find(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));

        if (ship == null)
        {
            Console.WriteLine("Statek nie został znaleziony.");
            return;
        }

        Console.Write("Podaj numer seryjny kontenera do usunięcia: ");
        string containerSerialNumber = Console.ReadLine();

        try
        {
            ship.RemoveContainer(containerSerialNumber);
            Console.WriteLine($"Kontener {containerSerialNumber} został usunięty ze statku {ship.Name}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    private static void UnloadContainer()
    {
        Console.Write("Podaj nazwę statku: ");
        string shipName = Console.ReadLine();
        Ship ship = ships.Find(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));

        if (ship == null)
        {
            Console.WriteLine("Statek nie został znaleziony.");
            return;
        }

        Console.Write("Podaj numer seryjny kontenera do rozładowania: ");
        string containerSerialNumber = Console.ReadLine();
        Container container = ship.Containers.Find(c =>
            c.SerialNumber.Equals(containerSerialNumber, StringComparison.OrdinalIgnoreCase));

        if (container == null)
        {
            Console.WriteLine($"Kontener {containerSerialNumber} nie został znaleziony na statku {ship.Name}.");
            return;
        }

        container.Unload();
        Console.WriteLine($"Kontener {containerSerialNumber} został rozładowany.");
    }

    private static void ReplaceContainerOnShip()
    {
        Console.Write("Podaj nazwę statku: ");
        string shipName = Console.ReadLine();
        Ship ship = ships.Find(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));

        if (ship == null)
        {
            Console.WriteLine("Statek nie został znaleziony.");
            return;
        }

        Console.Write("Podaj numer seryjny kontenera do zastąpienia: ");
        string oldContainerSerialNumber = Console.ReadLine();
        Container oldContainer = ship.Containers.Find(c =>
            c.SerialNumber.Equals(oldContainerSerialNumber, StringComparison.OrdinalIgnoreCase));

        if (oldContainer == null)
        {
            Console.WriteLine($"Kontener {oldContainerSerialNumber} nie został znaleziony na statku {ship.Name}.");
            return;
        }

        Console.Write("Podaj numer seryjny nowego kontenera: ");
        string newContainerSerialNumber = Console.ReadLine();
        Container newContainer = containers.Find(c =>
            c.SerialNumber.Equals(newContainerSerialNumber, StringComparison.OrdinalIgnoreCase));

        if (newContainer == null)
        {
            Console.WriteLine("Nowy kontener nie został znaleziony.");
            return;
        }

        try
        {
            ship.RemoveContainer(oldContainerSerialNumber);
            ship.LoadContainer(newContainer);
            containers.Remove(newContainer); // Usuwamy nowy kontener z listy dostępnych kontenerów
            Console.WriteLine(
                $"Kontener {oldContainerSerialNumber} został zastąpiony kontenerem {newContainerSerialNumber} na statku {ship.Name}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    private static void TransferContainerBetweenShips()
    {
        Console.Write("Podaj nazwę statku źródłowego: ");
        string sourceShipName = Console.ReadLine();
        Ship sourceShip = ships.Find(s => s.Name.Equals(sourceShipName, StringComparison.OrdinalIgnoreCase));

        if (sourceShip == null)
        {
            Console.WriteLine("Statek źródłowy nie został znaleziony.");
            return;
        }

        Console.Write("Podaj nazwę statku docelowego: ");
        string targetShipName = Console.ReadLine();
        Ship targetShip = ships.Find(s => s.Name.Equals(targetShipName, StringComparison.OrdinalIgnoreCase));

        if (targetShip == null)
        {
            Console.WriteLine("Statek docelowy nie został znaleziony.");
            return;
        }

        Console.Write("Podaj numer seryjny kontenera do przeniesienia: ");
        string containerSerialNumber = Console.ReadLine();
        Container container = sourceShip.Containers.Find(c =>
            c.SerialNumber.Equals(containerSerialNumber, StringComparison.OrdinalIgnoreCase));

        if (container == null)
        {
            Console.WriteLine($"Kontener {containerSerialNumber} nie został znaleziony na statku {sourceShip.Name}.");
            return;
        }

        try
        {
            sourceShip.TransferContainerTo(targetShip, containerSerialNumber);
            Console.WriteLine(
                $"Kontener {containerSerialNumber} został przeniesiony ze statku {sourceShip.Name} na statek {targetShip.Name}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }
}