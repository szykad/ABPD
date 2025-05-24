using ABPD_5.Data;
using ABPD_5.Dtos;

namespace ABPD_5.Services;

public class PatientService(DatabaseContext context) : IPatientService
{
    public PatientDetailsDto? GetPatientDetails(int id)
    {
        var patient = context.Patients
            .Where(p => p.IdPatient == id)
            .Select(p => new PatientDetailsDto
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Birthdate = p.Birthdate,
                Prescriptions = p.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionDetailsDto
                    {
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        DoctorFullName = pr.Doctor.FirstName + " " + pr.Doctor.LastName,
                        Medicaments = pr.PrescriptionMedicaments
                            .Select(pm => new MedicamentDetailsDto
                            {
                                Name = pm.Medicament.Name,
                                Description = pm.Medicament.Description,
                                Type = pm.Medicament.Type,
                                Dose = pm.Dose,
                                Details = pm.Details
                            }).ToList()
                    }).ToList()
            })
            .FirstOrDefault();

        return patient;
    }
}