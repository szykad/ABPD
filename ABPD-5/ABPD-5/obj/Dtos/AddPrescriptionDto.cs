namespace ABPD_5.Dtos;

using System.ComponentModel.DataAnnotations;

public class AddPrescriptionDto
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public int IdDoctor { get; set; }

    [Required]
    public int IdPatient { get; set; }

    public string? PatientFirstName { get; set; }
    public string? PatientLastName { get; set; }
    public DateTime? PatientBirthdate { get; set; }

    [Required]
    public List<PrescriptionMedicamentDto> Medicaments { get; set; }
}