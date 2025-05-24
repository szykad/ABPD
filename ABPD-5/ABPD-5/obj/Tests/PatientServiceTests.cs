using ABPD_5.Data;
using ABPD_5.model;
using ABPD_5.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class PatientServiceTests
{
    private DatabaseContext GetContextWithData()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase("Test")
            .Options;

        var context = new DatabaseContext(options);

        var doctor = new Doctor { IdDoctor = 1, FirstName = "Adam", LastName = "Kowalski", Email = "adam@warta.pl" };
        var medicament = new Medicament { IdMedicament = 1, Name = "Apap", Description = "Ból", Type = "Tabletka" };
        var patient = new Patient { IdPatient = 1, FirstName = "Anna", LastName = "Nowak", Birthdate = new DateTime(1990, 1, 1) };

        var prescription = new Prescription
        {
            IdPrescription = 1,
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(7),
            IdDoctor = 1,
            IdPatient = 1,
            Doctor = doctor,
            Patient = patient
        };

        var pm = new PrescriptionMedicament
        {
            IdPrescription = 1,
            IdMedicament = 1,
            Prescription = prescription,
            Medicament = medicament,
            Dose = 2,
            Details = "6 x dziennie"
        };

        context.Doctors.Add(doctor);
        context.Medicaments.Add(medicament);
        context.Patients.Add(patient);
        context.Prescriptions.Add(prescription);
        context.PrescriptionMedicaments.Add(pm);
        context.SaveChanges();

        return context;
    }

    [Fact]
    public void GetPatientDetails_ReturnsCorrectData()
    {
        var context = GetContextWithData();
        var service = new PatientService(context);

        var result = service.GetPatientDetails(1);

        Assert.NotNull(result);
        Assert.Equal("Anna", result.FirstName);
        Assert.Single(result.Prescriptions);
        Assert.Equal("Apap", result.Prescriptions[0].Medicaments[0].Name);
    }
}