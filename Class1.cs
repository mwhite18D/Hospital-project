using System.Drawing;
using System.Windows.Forms;

namespace HospitalManagement.UI;

public static class Ui
{
    public static readonly Color Navy = Color.FromArgb(23, 42, 69);
    public static readonly Color Blue = Color.FromArgb(37, 99, 235);
    public static readonly Color Background = Color.FromArgb(245, 247, 251);
    public static readonly Color Text = Color.FromArgb(35, 48, 66);
    public static readonly Font Normal = new("Segoe UI", 9.5f);
    public static readonly Font Heading = new("Segoe UI Semibold", 17f);

    public static Button Button(string text, bool primary = false)
    {
        var b = new Button
        {
            Text = text,
            AutoSize = true,
            Padding = new Padding(14, 7, 14, 7),
            FlatStyle = FlatStyle.Flat,
            BackColor = primary ? Blue : Color.White,
            ForeColor = primary ? Color.White : Text,
            Font = new Font("Segoe UI Semibold", 9f),
            Cursor = Cursors.Hand,
            Margin = new Padding(4)
        };

        b.FlatAppearance.BorderColor = primary ? Blue : Color.FromArgb(215, 222, 232);
        return b;
    }

    public static Label Label(string text, float size = 9.5f, bool bold = false) =>
        new()
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = Text,
            Margin = new Padding(2, 7, 2, 4)
        };

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.RowHeadersVisible = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 249);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Navy;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9f);
        grid.DefaultCellStyle.Font = Normal;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
        grid.DefaultCellStyle.SelectionForeColor = Text;
        grid.RowTemplate.Height = 34;
        grid.ColumnHeadersHeight = 38;
    }

    public static Panel Card(Control content)
    {
        var p = new Panel
        {
            BackColor = Color.White,
            Padding = new Padding(16),
            Margin = new Padding(8),
            Dock = DockStyle.Fill
        };

        p.Controls.Add(content);
        content.Dock = DockStyle.Fill;
        return p;
    }
}
