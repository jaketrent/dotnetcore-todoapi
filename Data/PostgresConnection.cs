using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace HelloDotnetCoreApi.Data
{

  public interface IPostgresConnection
  {
    IEnumerable<T> WriteData<T>(string statement, IRowMapper<T> rowMapper, object parameters = null);
  }

  public interface IRowMapper<T>
  {
    T mapRow(IDataReader reader);
  }

  public class PostgresConnection : IPostgresConnection
  {
    private IConfiguration _config;

    public PostgresConnection(IConfiguration config)
    {
      _config = config;
    }
    public IEnumerable<T> WriteData<T>(string statement, IRowMapper<T> rowMapper, object parameters = null)
    {
      IDbConnection conn = null;

      var retvals = new List<T>();

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
            param.ParameterName = ":" + prop.Name;
            param.Value = prop.GetValue(parameters);
            cmd.Parameters.Add(param);
          }

          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              retvals.Add(rowMapper.mapRow(reader));
            }
          }
        }
      }
      finally
      {
        CloseConnection(conn);
      }

      return retvals;
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