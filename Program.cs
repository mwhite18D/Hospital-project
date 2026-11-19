using HospitalManagement.UI;

namespace Hospital_project;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new ConnectionForm());
    }
}