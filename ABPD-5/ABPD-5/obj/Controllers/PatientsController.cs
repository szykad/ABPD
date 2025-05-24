using Microsoft.AspNetCore.Mvc;
using ABPD_5.Services;

namespace ABPD_5.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController(IPatientService patientService) : ControllerBase
{
    [HttpGet("{id:int}", Name = "GetPatientDetails")]
    public IActionResult GetPatientDetails(int id)
    {
        var result = patientService.GetPatientDetails(id);

        if (result == null)
            return NotFound($"Pacjent o ID {id} nie istnieje.");

        return Ok(result);
    }
}