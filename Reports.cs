using System.Data;
using HospitalManagement.Data;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.UI;

public sealed class ReportsPage : UserControl
{
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill };

    private readonly DateTimePicker _date = new()
    {
        Format = DateTimePickerFormat.Short,
        Value = DateTime.Today,
        Width = 150
    };

    private readonly ComboBox _patient = new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 250
    };

    private readonly Label _description =
        Ui.Label("Choose a report to display results.");

    public ReportsPage()
    {
        Ui.StyleGrid(_grid);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };

        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false
        };

        var daily = Ui.Button("Doctor daily schedule", true);
        daily.Click += (_, _) => RunReport(LoadDaily);

        var history = Ui.Button("Patient visit history");
        history.Click += (_, _) => RunReport(LoadHistory);

        var revenue = Ui.Button("Revenue by department");
        revenue.Click += (_, _) => RunReport(LoadRevenue);

        buttons.Controls.Add(daily);
        buttons.Controls.Add(history);
        buttons.Controls.Add(revenue);

        var filters = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false
        };

        filters.Controls.Add(Ui.Label("Date:"));
        filters.Controls.Add(_date);
        filters.Controls.Add(Ui.Label("Patient:"));
        filters.Controls.Add(_patient);

        _patient.DisplayMember = "Name";
        _patient.ValueMember = "PatientID";
        _patient.DataSource = Db.Query(
            "SELECT PatientID, Name FROM dbo.Patient ORDER BY Name");

        var result = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        _description.Dock = DockStyle.Top;
        _description.Height = 30;

        result.Controls.Add(_grid);
        result.Controls.Add(_description);
        _description.BringToFront();

        root.Controls.Add(buttons, 0, 0);
        root.Controls.Add(filters, 0, 1);
        root.Controls.Add(result, 0, 2);

        Controls.Add(root);
    }

    private static void RunReport(Action loadReport)
    {
        try
        {
            loadReport();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Could not load report",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void SetReport(DataTable data, string description)
    {
        _grid.DataSource = data;
        _description.Text = description;
    }

    private void LoadDaily()
    {
        var data = Db.Query(
            @"SELECT d.Name AS Doctor, d.Specialty,
                     a.AppointmentTime, p.Name AS Patient,
                     p.Phone, a.Status
              FROM dbo.Appointment a
              JOIN dbo.Doctor d ON d.DoctorID = a.DoctorID
              JOIN dbo.Patient p ON p.PatientID = a.PatientID
              WHERE a.AppointmentDate = @date
              ORDER BY d.Name, a.AppointmentTime",
            new SqlParameter("@date", SqlDbType.Date)
            {
                Value = _date.Value.Date
            });

        SetReport(data, "Doctor daily schedule - " + _date.Value.ToShortDateString());
    }

    private void LoadHistory()
    {
        if (_patient.SelectedValue is null ||
            _patient.SelectedValue is DataRowView)
        {
            MessageBox.Show("Select a patient.");
            return;
        }

        var data = Db.Query(
            @"SELECT a.AppointmentID, a.AppointmentDate, a.AppointmentTime,
                     d.Name AS Doctor, d.Specialty, a.Status,
                     mr.Diagnosis, mr.Notes, mr.RecordDate,
                     b.Amount, b.PaymentMethod, b.Status AS BillingStatus
              FROM dbo.Appointment a
              JOIN dbo.Doctor d ON d.DoctorID = a.DoctorID
              LEFT JOIN dbo.MedicalRecord mr
                     ON mr.AppointmentID = a.AppointmentID
              LEFT JOIN dbo.Billing b
                     ON b.AppointmentID = a.AppointmentID
              WHERE a.PatientID = @patientId
              ORDER BY a.AppointmentDate DESC, a.AppointmentTime DESC",
            new SqlParameter("@patientId", SqlDbType.Int)
            {
                Value = Convert.ToInt32(_patient.SelectedValue)
            });

        SetReport(data, "Patient visit history");
    }

    private void LoadRevenue()
    {
        var data = Db.Query(
            @"SELECT dep.DeptName AS Department,
                     COUNT(DISTINCT a.AppointmentID) AS Appointments,
                     COALESCE(SUM(CASE WHEN b.Status = 'Paid'
                                       THEN b.Amount ELSE 0 END), 0) AS PaidBills,
                     COALESCE(SUM(CASE WHEN b.Status IN ('Pending', 'Overdue')
                                       THEN b.Amount ELSE 0 END), 0) AS UnpaidBills
              FROM dbo.Department dep
              LEFT JOIN dbo.Doctor d ON d.DepartmentID = dep.DepartmentID
              LEFT JOIN dbo.Appointment a ON a.DoctorID = d.DoctorID
              LEFT JOIN dbo.Billing b ON b.AppointmentID = a.AppointmentID
              GROUP BY dep.DepartmentID, dep.DeptName
              ORDER BY dep.DeptName");

        SetReport(data, "Paid bills and pending/overdue bills by department");
    }
}