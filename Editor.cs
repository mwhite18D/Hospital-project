namespace HospitalManagement.Models;

public sealed record FieldDefinition(
    string Name,
    string Label,
    string Kind = "text",
    string? LookupTable = null,
    string? LookupKey = null,
    string? LookupDisplay = null,
    string[]? Choices = null,
    bool Required = false);

public sealed record EntityDefinition(
    string Table,
    string PrimaryKey,
    string Title,
    string[] SearchColumns,
    FieldDefinition[] Fields)
{
    public static readonly EntityDefinition[] All =
    [
        new("Department", "DepartmentID", "Departments",
            ["DeptName", "Location"],
            [
                new("DeptName", "Department name", Required: true),
                new("Location", "Location")
            ]),

        new("Doctor", "DoctorID", "Doctors",
            ["Name", "Specialty", "Phone", "Email"],
            [
                new("Name", "Doctor name", Required: true),
                new("Specialty", "Specialty"),
                new("Phone", "Phone"),
                new("Email", "Email"),
                new("DepartmentID", "Department", "lookup",
                    "Department", "DepartmentID", "DeptName", Required: true)
            ]),

        new("Patient", "PatientID", "Patients",
            ["Name", "Phone", "Email", "Address"],
            [
                new("Name", "Patient name", Required: true),
                new("DOB", "Date of birth", "date"),
                new("Gender", "Gender", "choice",
                    Choices: ["F", "M", "X", "U"]),
                new("Phone", "Phone"),
                new("Address", "Address"),
                new("Email", "Email")
            ]),

        new("Appointment", "AppointmentID", "Appointments",
            ["Status"],
            [
                new("PatientID", "Patient", "lookup",
                    "Patient", "PatientID", "Name", Required: true),
                new("DoctorID", "Doctor", "lookup",
                    "Doctor", "DoctorID", "Name", Required: true),
                new("AppointmentDate", "Appointment date", "date", Required: true),
                new("AppointmentTime", "Appointment time", "time", Required: true),
                new("Status", "Status", "choice",
                    Choices: ["Scheduled", "Completed", "Cancelled", "No-Show"],
                    Required: true)
            ]),

        new("MedicalRecord", "RecordID", "Medical records",
            ["Diagnosis", "Notes"],
            [
                new("AppointmentID", "Appointment", "lookup",
                    "Appointment", "AppointmentID", "AppointmentID", Required: true),
                new("Diagnosis", "Diagnosis"),
                new("Notes", "Clinical notes", "multiline"),
                new("RecordDate", "Record date", "date")
            ]),

        new("Prescription", "PrescriptionID", "Prescriptions",
            ["MedicineName", "Dosage", "Instructions"],
            [
                new("RecordID", "Medical record", "lookup",
                    "MedicalRecord", "RecordID", "Diagnosis", Required: true),
                new("MedicineName", "Medicine name", Required: true),
                new("Dosage", "Dosage"),
                new("Instructions", "Instructions", "multiline")
            ]),

        new("Billing", "BillID", "Billing",
            ["PaymentMethod", "Status"],
            [
                new("AppointmentID", "Appointment", "lookup",
                    "Appointment", "AppointmentID", "AppointmentID", Required: true),
                new("Amount", "Amount", "decimal", Required: true),
                new("PaymentDate", "Payment date", "date"),
                new("PaymentMethod", "Payment method", "choice",
                    Choices: ["Cash", "Credit Card", "Debit Card", "Insurance", "Other"]),
                new("Status", "Payment status", "choice",
                    Choices: ["Pending", "Paid", "Overdue", "Partially Paid", "Refunded"],
                    Required: true)
            ])
    ];
}