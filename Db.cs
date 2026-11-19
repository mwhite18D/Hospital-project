using System.Data;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.Data;

public static class Db
{
    public static SqlConnection Open()
    {
        var connection = new SqlConnection(AppConfig.ConnectionString);

        try
        {
            connection.Open();
            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    }

    public static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        using var connection = Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);

        using var adapter = new SqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public static object? Scalar(string sql, params SqlParameter[] parameters)
    {
        using var connection = Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);
        return command.ExecuteScalar();
    }

    public static int Execute(string sql, params SqlParameter[] parameters)
    {
        using var connection = Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);
        return command.ExecuteNonQuery();
    }
}