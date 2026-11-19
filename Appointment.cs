using System.Data;
using HospitalManagement.Data;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.UI;

public sealed class BookingForm : Form
{
    private readonly ComboBox _department = new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 360
    };

    private readonly ComboBox _doctor = new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 360
    };

    private readonly ComboBox _patient = new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 360
    };

    private readonly DateTimePicker _date = new()
    {
        Format = DateTimePickerFormat.Short,
        Width = 360,
        MinDate = DateTime.Today
    };

    private readonly DateTimePicker _time = new()
    {
        Format = DateTimePickerFormat.Custom,
        CustomFormat = "hh:mm tt",
        ShowUpDown = true,
        Width = 360
    };

    public BookingForm()
    {
        Text = "Book Appointment";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(520, 510);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Ui.Background;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            ColumnCount = 1,
            RowCount = 8
        };

        root.Controls.Add(new Label
        {
            Text = "Book an appointment",
            Font = Ui.Heading,
            AutoSize = true,
            ForeColor = Ui.Navy
        });

        AddField(root, "Patient", _patient);
        AddField(root, "Department", _department);
        AddField(root, "Doctor", _doctor);
        AddField(root, "Date", _date);
        AddField(root, "Time", _time);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true
        };

        var save = Ui.Button("Book appointment", true);
        save.Click += Save;

        var cancel = Ui.Button("Cancel");
        cancel.Click += (_, _) => DialogResult = DialogResult.Cancel;

        buttons.Controls.Add(save);
        buttons.Controls.Add(cancel);
        root.Controls.Add(buttons);
        Controls.Add(root);

        LoadLookups();

        _department.SelectedIndexChanged += (_, _) =>
        {
            try
            {
                LoadDoctors();
            }
            catch (Exception ex)
            {
                _doctor.DataSource = null;
                MessageBox.Show(
                    ex.Message,
                    "Could not load doctors",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        };
    }

    private static void AddField(
        TableLayoutPanel root, string label, Control control)
    {
        var panel = new Panel
        {
            Height = 56,
            Dock = DockStyle.Top
        };

        var fieldLabel = Ui.Label(label, 9, true);
        fieldLabel.Dock = DockStyle.Top;

        control.Dock = DockStyle.Bottom;
        control.Font = Ui.Normal;

        panel.Controls.Add(control);
        panel.Controls.Add(fieldLabel);
        fieldLabel.BringToFront();

        root.Controls.Add(panel);
    }

    private void LoadLookups()
    {
        _patient.DisplayMember = "Name";
        _patient.ValueMember = "PatientID";
        _patient.DataSource = AppointmentStore.GetPatients();

        _department.DisplayMember = "DeptName";
        _department.ValueMember = "DepartmentID";
        _department.DataSource = AppointmentStore.GetDepartments();

        LoadDoctors();
    }

    private void LoadDoctors()
    {
        if (_department.SelectedValue is null ||
            _department.SelectedValue is DataRowView)
        {
            _doctor.DataSource = null;
            return;
        }

        _doctor.DisplayMember = "DoctorDisplay";
        _doctor.ValueMember = "DoctorID";
        _doctor.DataSource = AppointmentStore.GetDoctorsByDepartment(
            Convert.ToInt32(_department.SelectedValue));
    }

    private void Save(object? sender, EventArgs e)
    {
        try
        {
            if (_patient.SelectedValue is null ||
                _patient.SelectedValue is DataRowView ||
                _doctor.SelectedValue is null ||
                _doctor.SelectedValue is DataRowView)
            {
                MessageBox.Show(
                    "Select a patient and a doctor before booking.",
                    "Missing information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var date = _date.Value.Date;
            var time = new TimeSpan(_time.Value.Hour, _time.Value.Minute, 0);

            AppointmentStore.AddAppointment(
                Convert.ToInt32(_patient.SelectedValue),
                Convert.ToInt32(_doctor.SelectedValue),
                date,
                time,
                "Scheduled");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
        {
            MessageBox.Show(
                "The appointment could not be saved because a database " +
                "uniqueness constraint was violated. The slot may already be booked.",
                "Could not book appointment",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Could not book appointment",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static class AppointmentStore
    {
        public static DataTable GetPatients() =>
            Db.Query(
                "SELECT PatientID, Name FROM dbo.Patient ORDER BY Name");

        public static DataTable GetDepartments() =>
            Db.Query(
                "SELECT DepartmentID, DeptName FROM dbo.Department ORDER BY DeptName");

        public static DataTable GetDoctorsByDepartment(int departmentId) =>
            Db.Query(
                @"SELECT DoctorID,
                         Name + CASE WHEN Specialty IS NULL OR Specialty = ''
                                     THEN ''
                                     ELSE ' - ' + Specialty END AS DoctorDisplay
                  FROM dbo.Doctor
                  WHERE DepartmentID = @departmentId
                  ORDER BY Name",
                new SqlParameter("@departmentId", SqlDbType.Int)
                {
                    Value = departmentId
                });

        public static void AddAppointment(
            int patientId,
            int doctorId,
            DateTime date,
            TimeSpan time,
            string status)
        {
            using var connection = Db.Open();
            using var transaction =
                connection.BeginTransaction(IsolationLevel.Serializable);

            int nextId;

            using (var idCommand = new SqlCommand(
                @"SELECT ISNULL(MAX(AppointmentID), 0)
                  FROM dbo.Appointment WITH (UPDLOCK, HOLDLOCK)",
                connection,
                transaction))
            {
                nextId = checked(Convert.ToInt32(idCommand.ExecuteScalar()) + 1);
            }

            using (var conflictCommand = new SqlCommand(
                @"SELECT COUNT(*)
                  FROM dbo.Appointment
                  WHERE DoctorID = @doctorId
                    AND AppointmentDate = @date
                    AND AppointmentTime = @time",
                connection,
                transaction))
            {
                conflictCommand.Parameters.Add(
                    "@doctorId", SqlDbType.Int).Value = doctorId;
                conflictCommand.Parameters.Add(
                    "@date", SqlDbType.Date).Value = date.Date;
                conflictCommand.Parameters.Add(
                    "@time", SqlDbType.Time).Value = time;

                if (Convert.ToInt32(conflictCommand.ExecuteScalar()) > 0)
                {
                    throw new InvalidOperationException(
                        "That doctor already has an appointment at this date and time.");
                }
            }

            using (var insertCommand = new SqlCommand(
                @"INSERT INTO dbo.Appointment
                    (AppointmentID, PatientID, DoctorID,
                     AppointmentDate, AppointmentTime, Status)
                  VALUES
                    (@id, @patientId, @doctorId, @date, @time, @status)",
                connection,
                transaction))
            {
                insertCommand.Parameters.Add(
                    "@id", SqlDbType.Int).Value = nextId;
                insertCommand.Parameters.Add(
                    "@patientId", SqlDbType.Int).Value = patientId;
                insertCommand.Parameters.Add(
                    "@doctorId", SqlDbType.Int).Value = doctorId;
                insertCommand.Parameters.Add(
                    "@date", SqlDbType.Date).Value = date.Date;
                insertCommand.Parameters.Add(
                    "@time", SqlDbType.Time).Value = time;
                insertCommand.Parameters.Add(
                    "@status", SqlDbType.VarChar, 20).Value = status;

                insertCommand.ExecuteNonQuery();
            }

            transaction.Commit();
        }
    }
}