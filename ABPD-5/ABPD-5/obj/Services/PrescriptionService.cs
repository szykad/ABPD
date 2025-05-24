using ABPD_5.model;

namespace ABPD_5.Services;

using Data;
using Dtos;

public class PrescriptionService(DatabaseContext context) : IPrescriptionService
{
    public int AddPrescription(AddPrescriptionDto dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO nie może być null.");

        if (dto.Medicaments == null || dto.Medicaments.Count == 0)
            throw new ArgumentException("Lista leków nie może być pusta.");

        if (dto.Medicaments.Count > 10)
            throw new ArgumentException("Recepta nie może zawierać więcej niż 10 leków.");

        if (dto.DueDate < dto.Date)
            throw new ArgumentException("Data ważności nie może być wcześniejsza niż data wystawienia.");

        var doctor = context.Doctors.FirstOrDefault(d => d.IdDoctor == dto.IdDoctor);
        if (doctor == null)
            throw new ArgumentException($"Lekarz o ID {dto.IdDoctor} nie istnieje.");

        var patient = context.Patients.FirstOrDefault(p => p.IdPatient == dto.IdPatient);
        if (patient == null)
        {
            if (string.IsNullOrWhiteSpace(dto.PatientFirstName) ||
                string.IsNullOrWhiteSpace(dto.PatientLastName) ||
                dto.PatientBirthdate == null)
            {
                throw new ArgumentException("Pacjent nie istnieje i brakuje danych.");
            }

            patient = new Patient
            {
                IdPatient = dto.IdPatient,
                FirstName = dto.PatientFirstName,
                LastName = dto.PatientLastName,
                Birthdate = dto.PatientBirthdate.Value
            };

            context.Patients.Add(patient);
            context.SaveChanges();
        }

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = doctor.IdDoctor
        };

        context.Prescriptions.Add(prescription);
        context.SaveChanges();

        foreach (var med in dto.Medicaments)
        {
            var medicament = context.Medicaments.FirstOrDefault(m => m.IdMedicament == med.IdMedicament);
            if (medicament == null)
                throw new ArgumentException($"Lek o ID {med.IdMedicament} nie istnieje.");

            context.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = medicament.IdMedicament,
                Dose = med.Dose,
                Details = med.Details
            });
        }

        context.SaveChanges();
        return prescription.IdPrescription;
    }
}