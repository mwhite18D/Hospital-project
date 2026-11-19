using HospitalManagement.Data;
using Hospital_project;

namespace HospitalManagement.UI;

public sealed class ConnectionForm : Form
{
    private readonly TextBox _connection = new()
    {
        Dock = DockStyle.Top,
        Width = 600
    };

    private readonly Label _message =
        Ui.Label("Enter the connection details for your SQL Server database.");

    public ConnectionForm()
    {
        Text = "Hospital Management - Database Connection";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(720, 340);
        MinimumSize = new Size(620, 320);
        BackColor = Ui.Background;
        Font = Ui.Normal;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            ColumnCount = 1,
            RowCount = 6
        };

        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var title = new Label
        {
            Text = "Hospital Management System",
            Font = Ui.Heading,
            ForeColor = Ui.Navy,
            AutoSize = true
        };

        _connection.Text = AppConfig.ConnectionString;
        _connection.Font = Ui.Normal;
        _message.AutoSize = true;

        var test = Ui.Button("Test connection");
        test.Click += (_, _) => TestConnection();

        var connect = Ui.Button("Connect", true);
        connect.Click += Connect;

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true
        };

        buttons.Controls.Add(connect);
        buttons.Controls.Add(test);

        root.Controls.Add(title);
        root.Controls.Add(Ui.Label("SQL Server connection", 11, true));
        root.Controls.Add(Ui.Label("Connection string"));
        root.Controls.Add(_connection);
        root.Controls.Add(buttons);
        root.Controls.Add(_message);

        Controls.Add(root);
    }

    private bool TestConnection()
    {
        try
        {
            AppConfig.ConnectionString = _connection.Text.Trim();

            using var connection = Db.Open();
            using var command = new Microsoft.Data.SqlClient.SqlCommand(
                @"SELECT
                (SELECT COUNT(*) FROM dbo.Patient),
                (SELECT COUNT(*) FROM dbo.Doctor)",
                connection);
            using var reader = command.ExecuteReader();

            reader.Read();
            _message.Text =
                $"Connected to {connection.DataSource} / {connection.Database}. " +
                $"Patients: {reader.GetInt32(0)}, Doctors: {reader.GetInt32(1)}.";
            _message.ForeColor = Color.SeaGreen;
            return true;
        }
        catch (Exception ex)
        {
            _message.Text = "Connection failed: " + ex.Message;
            _message.ForeColor = Color.Firebrick;
            return false;
        }
    }

    private void Connect(object? sender, EventArgs e)
    {
        if (!TestConnection())
        {
            return;
        }

        Hide();

        using var main = new Form1();
        main.ShowDialog(this);

        Close();
    }
}