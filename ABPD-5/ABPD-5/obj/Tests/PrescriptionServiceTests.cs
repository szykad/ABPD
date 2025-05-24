using ABPD_5.Data;
using ABPD_5.Dtos;
using ABPD_5.model;
using ABPD_5.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ABPD_5.Tests.Services;

public class PrescriptionServiceTests
{
    private DatabaseContext GetContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new DatabaseContext(options);

        context.Doctors.Add(new Doctor
        {
            IdDoctor = 1,
            FirstName = "Kamil",
            LastName = "Zięba",
            Email = "kamil@med.pl"
        });

        context.Medicaments.Add(new Medicament
        {
            IdMedicament = 1,
            Name = "Apap",
            Description = "Na ból",
            Type = "Tabletka"
        });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public void AddPrescription_ShouldThrow_WhenMedicamentDoesNotExist()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new DatabaseContext(options);

        context.Doctors.Add(new Doctor
        {
            IdDoctor = 1,
            FirstName = "Test",
            LastName = "Lekarz",
            Email = "test@med.pl"
        });


        context.SaveChanges();

        var service = new PrescriptionService(context);

        var dto = new AddPrescriptionDto
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(3),
            IdDoctor = 1,
            IdPatient = 500,
            PatientFirstName = "Nowy",
            PatientLastName = "Pacjent",
            PatientBirthdate = new DateTime(1990, 1, 1),
            Medicaments = new List<PrescriptionMedicamentDto>
            {
                new PrescriptionMedicamentDto
                {
                    IdMedicament = 999,
                    Dose = 1,
                    Details = "brak"
                }
            }
        };

        var ex = Assert.Throws<ArgumentException>(() => service.AddPrescription(dto));
        Assert.Contains("nie istnieje", ex.Message);
    }
}