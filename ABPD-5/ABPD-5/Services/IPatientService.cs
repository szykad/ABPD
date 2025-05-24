using ABPD_5.Dtos;

namespace ABPD_5.Services;

public interface IPatientService

{
    PatientDetailsDto? GetPatientDetails(int id);
}