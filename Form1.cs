using System.Data;
using HospitalManagement.Data;
using HospitalManagement.UI;
using Microsoft.Data.SqlClient;

namespace Hospital_project;

public partial class Form1 : Form
{
    private readonly Panel _content = new()
    {
        Dock = DockStyle.Fill,
        BackColor = Ui.Background,
        Padding = new Padding(18)
    };

    private readonly Label _pageTitle = new()
    {
        Text = "Dashboard",
        AutoSize = true,
        Font = Ui.Heading,
        ForeColor = Ui.Navy,
        Padding = new Padding(0, 10, 0, 10)
    };

    public Form1()
    {
        InitializeComponent();

        Text = "Hospital Database System";
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1100, 700);
        BackColor = Ui.Background;
        Font = Ui.Normal;

        var shell = new Panel { Dock = DockStyle.Fill };
        Controls.Add(shell);

        var sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 220,
            BackColor = Ui.Navy,
            Padding = new Padding(12)
        };

        var brand = new Label
        {
            Text = "HOSPITAL\nDATABASE",
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 14),
            AutoSize = true,
            Padding = new Padding(8, 16, 8, 26)
        };
        sidebar.Controls.Add(brand);

        var nav = new FlowLayoutPanel
        {
            Location = new Point(12, 104),
            Width = 196,
            Height = 590,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        sidebar.Controls.Add(nav);

        AddNav(nav, "Dashboard", ShowDashboard);
        AddNav(nav, "Book appointment", OpenBooking);
        AddNav(nav, "Departments", () => ShowDataPage("Departments", "Department"));
        AddNav(nav, "Doctors", () => ShowDataPage("Doctors", "Doctor"));
        AddNav(nav, "Patients", () => ShowDataPage("Patients", "Patient"));
        AddNav(nav, "Appointments", () => ShowDataPage("Appointments", "Appointment"));
        AddNav(nav, "Medical records", () => ShowDataPage("Medical records", "MedicalRecord"));
        AddNav(nav, "Prescriptions", () => ShowDataPage("Prescriptions", "Prescription"));
        AddNav(nav, "Billing", () => ShowDataPage("Billing", "Billing"));
        AddNav(nav, "Reports", ShowReports);

        var top = new Panel
        {
            Dock = DockStyle.Top,
            Height = 58,
            BackColor = Color.White,
            Padding = new Padding(20, 8, 20, 8)
        };
        top.Controls.Add(_pageTitle);

        shell.Controls.Add(_content);
        shell.Controls.Add(top);
        shell.Controls.Add(sidebar);

        ShowDashboard();
    }
    private void ShowDataPage(string title, string table)
    {
        try
        {
            if (table != "Department" && table != "Doctor" &&
                table != "Patient" && table != "Appointment" &&
                table != "MedicalRecord" && table != "Prescription" &&
                table != "Billing")
            {
                throw new ArgumentException("Unknown table.");
            }

            var data = Db.Query($"SELECT * FROM dbo.[{table}]");

            var grid = new DataGridView { Dock = DockStyle.Fill };
            Ui.StyleGrid(grid);
            grid.DataSource = data;

            SetPage(title, grid);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Could not load data",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void AddNav(FlowLayoutPanel nav, string text, Action action)
    {
        var button = new Button
        {
            Text = "  " + text,
            Width = 190,
            Height = 42,
            TextAlign = ContentAlignment.MiddleLeft,
            FlatStyle = FlatStyle.Flat,
            BackColor = Ui.Navy,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9.5f),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 2, 0, 2)
        };

        button.FlatAppearance.BorderSize = 0;
        button.Click += (_, _) => action();
        nav.Controls.Add(button);
    }

    private void SetPage(string title, Control control)
    {
        _pageTitle.Text = title;

        foreach (Control oldControl in _content.Controls)
        {
            oldControl.Dispose();
        }

        _content.Controls.Clear();
        control.Dock = DockStyle.Fill;
        _content.Controls.Add(control);
    }

    private void ShowDashboard()
    {
        try
        {
            var today = DateTime.Today;

            var patients = Convert.ToInt32(
                Db.Scalar("SELECT COUNT(*) FROM dbo.Patient"));

            var doctors = Convert.ToInt32(
                Db.Scalar("SELECT COUNT(*) FROM dbo.Doctor"));

            var appointments = Convert.ToInt32(
                Db.Scalar(
                    @"SELECT COUNT(*) FROM dbo.Appointment
                      WHERE AppointmentDate = @date",
                    new SqlParameter("@date", SqlDbType.Date) { Value = today }));

            var schedule = Db.Query(
                @"SELECT a.AppointmentID, p.Name AS Patient, d.Name AS Doctor,
                         a.AppointmentDate, a.AppointmentTime, a.Status
                  FROM dbo.Appointment a
                  JOIN dbo.Patient p ON p.PatientID = a.PatientID
                  JOIN dbo.Doctor d ON d.DoctorID = a.DoctorID
                  WHERE a.AppointmentDate = @date
                  ORDER BY a.AppointmentTime, d.Name",
                new SqlParameter("@date", SqlDbType.Date) { Value = today });

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var cards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false
            };

            AddMetric(cards, "Patients", patients.ToString());
            AddMetric(cards, "Doctors", doctors.ToString());
            AddMetric(cards, "Today's appointments", appointments.ToString());

            var grid = new DataGridView { Dock = DockStyle.Fill };
            Ui.StyleGrid(grid);
            grid.DataSource = schedule;

            var schedulePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var label = new Label
            {
                Text = "Today's appointment schedule",
                Font = new Font("Segoe UI Semibold", 12),
                ForeColor = Ui.Navy,
                Dock = DockStyle.Top,
                Height = 34
            };

            schedulePanel.Controls.Add(grid);
            schedulePanel.Controls.Add(label);
            label.BringToFront();

            root.Controls.Add(cards, 0, 0);
            root.Controls.Add(schedulePanel, 0, 1);

            SetPage("Dashboard", root);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Could not load dashboard",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static void AddMetric(Control parent, string title, string value)
    {
        var panel = new Panel
        {
            Width = 235,
            Height = 90,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 12, 0),
            Padding = new Padding(14)
        };

        var titleLabel = new Label
        {
            Text = title,
            ForeColor = Color.Gray,
            AutoSize = true,
            Dock = DockStyle.Top
        };

        var valueLabel = new Label
        {
            Text = value,
            ForeColor = Ui.Navy,
            Font = new Font("Segoe UI Semibold", 21),
            AutoSize = true,
            Dock = DockStyle.Top
        };

        panel.Controls.Add(valueLabel);
        panel.Controls.Add(titleLabel);
        parent.Controls.Add(panel);
    }

    private void OpenBooking()
    {
        try
        {
            using var booking = new BookingForm();

            if (booking.ShowDialog(this) == DialogResult.OK)
            {
                ShowDashboard();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Could not open booking form",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ShowReports()
    {
        try
        {
            SetPage("Reports", new ReportsPage());
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Could not open reports",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ShowPlaceholder(string title)
    {
        var label = new Label
        {
            Text = $"{title} management page has not been implemented yet.",
            AutoSize = true,
            Font = Ui.Normal,
            ForeColor = Ui.Navy,
            Padding = new Padding(8)
        };

        var panel = new Panel
        {
            BackColor = Color.White,
            Padding = new Padding(14)
        };

        panel.Controls.Add(label);
        SetPage(title, panel);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
    }
}