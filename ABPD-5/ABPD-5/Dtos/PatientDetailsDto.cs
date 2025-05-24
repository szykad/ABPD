﻿namespace ABPD_5.Dtos;

public class PatientDetailsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }

    public List<PrescriptionDetailsDto> Prescriptions { get; set; }
}