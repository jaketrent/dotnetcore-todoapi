using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace HelloDotnetCoreApi.Data
{

  public interface IPostgresConnection
  {
    void WriteData(string statement, object parameters = null);
  }

  public class PostgresConnection : IPostgresConnection
  {
    private IConfiguration _config;

    public PostgresConnection(IConfiguration config)
    {
      _config = config;
    }
    public void WriteData(string statement, object parameters = null)
    {
      IDbConnection conn = null;

      try
      {
        conn = OpenConnection();

        using (var cmd = conn.CreateCommand())
        {

          cmd.Connection = conn;
          cmd.CommandText = statement;

          foreach (var prop in parameters.GetType().GetProperties())
          {
            var param = cmd.CreateParameter();
            // TODO: verify - is this the correct named param format?
            param.ParameterName = ":" + prop.Name;
            param.Value = prop.GetValue(parameters);
            cmd.Parameters.Add(param);
          }

          cmd.ExecuteNonQuery();
        }
      }
      finally
      {
        CloseConnection(conn);
      }
    }

    private IDbConnection OpenConnection()
    {
      var conn = new NpgsqlConnection(_config.GetConnectionString("todo"));
      conn.Open();
      return conn;
    }

    private static void CloseConnection(IDbConnection conn)
    {
      conn?.Dispose();
    }
  }


}