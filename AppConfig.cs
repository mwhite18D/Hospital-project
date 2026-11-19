namespace HospitalManagement.Data;

public static class AppConfig
{
    public static string ConnectionString { get; set; } =
        @"Server=LAPTOP-PB7129UL;Database=Hospital;" +
        "Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
}