using Microsoft.AspNetCore.Mvc;
using ABPD_5.Dtos;
using ABPD_5.Services;

namespace ABPD_5.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController(IPrescriptionService prescriptionService) : ControllerBase
{
    [HttpPost]
    public IActionResult AddPrescription([FromBody] AddPrescriptionDto dto)
    {
        try
        {
            var newId = prescriptionService.AddPrescription(dto);
            return CreatedAtRoute(
                routeName: "GetPatientDetails",
                routeValues: new { id = dto.IdPatient },
                value: new { PrescriptionId = newId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}