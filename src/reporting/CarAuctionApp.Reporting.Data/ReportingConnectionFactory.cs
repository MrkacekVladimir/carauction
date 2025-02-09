using Npgsql;
using System.Data;

namespace CarAuctionApp.Reporting.Data;

public class ReportingConnectionFactory
{
    private readonly string _connectionString;

    public ReportingConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

}
