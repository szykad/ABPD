namespace ABPD_5.Dtos;

public class PrescriptionDetailsDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public string DoctorFullName { get; set; }

    public List<MedicamentDetailsDto> Medicaments { get; set; }
}