
class Program
{
    static void Main()
    {
        ParkingGarage garage = new ParkingGarage();

        bool running = true;
        while (running)
        {
            // Ny meny med clear funktion som renar koderna.
            Console.Clear();
            Console.WriteLine(); // Luft
            Console.WriteLine("Valet Parking");
            Console.WriteLine("*************");
            Console.WriteLine(); // Luft
            Console.WriteLine("Menu:");
            Console.WriteLine(); // Luft

            // Menyval
            Console.WriteLine("1. Lägg till fordon");
            Console.WriteLine("2. Ta bort fordon");
            Console.WriteLine("3. Flytta fordon");
            Console.WriteLine("4. Sök efter fordon");
            Console.WriteLine("5. Visa hela parkeringsgaraget");
            Console.WriteLine("6. Skriv ut sammanfattande rapport");
            Console.WriteLine("7. Avsluta programmet");
            Console.WriteLine(); // Luft
            Console.Write("Välj ett alternativ: ");
            string option = Console.ReadLine(); // Tar in valet

            switch (option)
            {
                case "1":
                    // Användaren anger typ och registreringsnummer för fordonet som ska parkeras.
                    Console.Write("Ange fordonstyp (CAR/MC): ");
                    string type = Console.ReadLine().ToUpper();
                    Console.Write("Ange registreringsnummer : ");
                    string regNumber = Console.ReadLine();
                    garage.ParkVehicle(type, regNumber);
                    break;

                case "2":
                    // Användaren anger registreringsnumret för fordonet som ska tas bort.
                    Console.Write("Ange registreringsnummer: ");
                    regNumber = Console.ReadLine();
                    garage.RemoveVehicle(regNumber);
                    break;

                case "3":
                    // Användaren anger från vilken plats fordonet ska flyttas och till vilken plats.
                    Console.Write("Ange från plats: ");
                    if (int.TryParse(Console.ReadLine(), out int fromSpot) && fromSpot > 0 && fromSpot <= 100)
                    {
                        Console.Write("Ange till plats: ");
                        if (int.TryParse(Console.ReadLine(), out int toSpot) && toSpot > 0 && toSpot <= 100)
                        {
                            garage.MoveVehicle(fromSpot, toSpot);
                        }
                        else
                        {
                            Console.WriteLine("Ogiltig plats.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig plats.");
                    }
                    break;

                case "4":
                    // Användaren anger registreringsnumret för fordonet som ska sökas.
                    Console.Write("Ange registreringsnummer: ");
                    regNumber = Console.ReadLine();
                    garage.SearchVehicle(regNumber);
                    break;

                case "5":
                    // Skriver ut statusen för alla parkeringsrutor.
                    garage.PrintParkingSpots();
                    break;

                case "6":
                    // Skriver ut en sammanfattningsrapport över parkeringsplatsens status.
                    garage.PrintSummaryReport();
                    break;

                case "7":
                    // Avslutar programmet.
                    running = false;
                    break;

                default:
                    // Felaktigt menyval.
                    Console.WriteLine("Ogiltigt alternativ, försök igen.");
                    break;
            }

            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }
    }
}

public class ParkingGarage
{
    private ParkingSpot[] parkingSpots = new ParkingSpot[100];

    public void ParkVehicle(string type, string regNumber)
    {
        string vehicle = type + "#" + regNumber;
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] == null)
            {
                parkingSpots[i] = new ParkingSpot(vehicle);
                Console.WriteLine($"Fordon parkerat på platsen {i + 1}.");
                return;
            }
            else if (type == "MC" && parkingSpots[i].VehicleInfo.StartsWith("MC") && !parkingSpots[i].VehicleInfo.Contains(","))
            {
                parkingSpots[i].VehicleInfo += "," + regNumber;
                Console.WriteLine($"Andra motorcykeln parkerad på platsen {i + 1}.");
                return;
            }
        }
        Console.WriteLine("Inga lediga platser.");
    }

    public void MoveVehicle(int fromSpot, int toSpot)
    {
        if (parkingSpots[fromSpot - 1] != null && parkingSpots[toSpot - 1] == null)
        {
            parkingSpots[toSpot - 1] = parkingSpots[fromSpot - 1];
            parkingSpots[fromSpot - 1] = null;
            Console.WriteLine($"Fordonet har flyttats från platsen {fromSpot} till plats {toSpot}.");
        }
        else
        {
            Console.WriteLine("Ogiltig flytt.");
        }
    }

    public void RemoveVehicle(string regNumber)
    {
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] != null && parkingSpots[i].VehicleInfo.Contains(regNumber))
            {
                var parkingDuration = DateTime.Now - parkingSpots[i].ArrivalTime;
                parkingSpots[i] = null;
                Console.WriteLine($"Fordonet {regNumber} borttagen från plats {i + 1}. Parkerad i {parkingDuration.TotalMinutes} minuter.");
                return;
            }
        }
        Console.WriteLine("Fordonet hittades inte.");
    }

    public void SearchVehicle(string regNumber)
    {
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] != null && parkingSpots[i].VehicleInfo.Contains(regNumber))
            {
                Console.WriteLine($"Fordonet med registreringsnummer {regNumber} finns på plats {i + 1}.");
                return;
            }
        }
        Console.WriteLine("Fordonet hittades inte.");
    }

    public void PrintParkingSpots()
    {
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i] != null)
            {
                Console.WriteLine($"Plats {i + 1}: {parkingSpots[i].VehicleInfo}");
            }
            else
            {
                Console.WriteLine($"Plats {i + 1}: Tom.");
            }
        }
    }

    public void PrintSummaryReport()
    {
        int occupiedSpots = 0;
        foreach (var spot in parkingSpots)
        {
            if (spot != null)
            {
                occupiedSpots++;
            }
        }
        Console.WriteLine($"Sammanfattningsrapport: {occupiedSpots} av {parkingSpots.Length} platser är upptagna.");
    }
}

public class ParkingSpot
{
    public string VehicleInfo { get; set; }
    public DateTime ArrivalTime { get; }

    public ParkingSpot(string vehicleInfo)
    {
        VehicleInfo = vehicleInfo;
        ArrivalTime = DateTime.Now;
    }
}
