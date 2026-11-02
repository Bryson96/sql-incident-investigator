using System.ComponentModel;
using Microsoft.Data.SqlClient;
using ModelContextProtocol.Server;

[McpServerToolType]
public class DatabaseTools
{
    // The ai_reader login can only read, so the AI cannot change data.
    static string connectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=MCP_DB;User Id=ai_reader;Password=ChangeMe_Reader1!;TrustServerCertificate=True";

    // Runs a query and returns the rows as plain text, one row per line.
    static string RunSql(SqlCommand command)
    {
        string result = "";
        int rowCount = 0;

        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && rowCount < 200)
        {
            string line = "";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                line = line + reader.GetName(i) + "=" + reader.GetValue(i) + " | ";
            }
            result = result + line + "\n";
            rowCount++;
        }
        reader.Close();

        if (rowCount == 0)
        {
            return "No rows.";
        }
        return result;
    }

    [McpServerTool(Name = "describe_table")]
    [Description("Lists the columns and data types of one table, e.g. schema Sales, table Orders.")]
    public static string DescribeTable(string schema, string table)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        SqlCommand command = new SqlCommand(
            "SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS " +
            "WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table", connection);
        command.Parameters.AddWithValue("@schema", schema);
        command.Parameters.AddWithValue("@table", table);

        string result = RunSql(command);
        connection.Close();
        return result;
    }

    [McpServerTool(Name = "run_query")]
    [Description("Runs one read-only SELECT query against TSQLV6 and returns up to 200 rows.")]
    public static string RunQuery(string sql)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            return RunSql(command);
        }
        catch (SqlException error)
        {
            // Send the error back so the AI can fix its query and try again.
            return "SQL error: " + error.Message;
        }
        finally
        {
            connection.Close();
        }
    }
}