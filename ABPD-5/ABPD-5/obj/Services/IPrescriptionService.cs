using ABPD_5.Dtos;

namespace ABPD_5.Services;

public interface IPrescriptionService

{
    int AddPrescription(AddPrescriptionDto dto);
}