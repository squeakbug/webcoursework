using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;
using System.IO;

namespace E2ETest
{
    internal static class Common
    {
        public static void RemoveTestSnapshot(string serverAddr, string snapshotName)
        {
            using (SqlConnection cnn = new SqlConnection($"Data Source={serverAddr}; database=master; User Id=SA; Password=P@ssword"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 1000;
                    cmd.CommandText = $"IF EXISTS(SELECT * FROM sys.databases WHERE name = '{snapshotName}')" +
                                      $"    DROP DATABASE {snapshotName};";
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
                cnn.Close();
            }
        }

        public static void CreateTestDatabase(string dbname, string serverAddr)
        {
            using (SqlConnection cnn = new SqlConnection($"Data Source={serverAddr}; database=master; User Id=SA; Password=P@ssword"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 1000;
                    cmd.CommandText = $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{dbname}')" +
                                      $"BEGIN \n" +
                                      $"    CREATE DATABASE [{dbname}]\n" +
                                      $"END\n";
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
                cnn.Close();
            }
        }

        public static void CreateDatabaseSnapshot(string dbname, string serverAddr,
            string snapshotName, string snapshotPath)
        {
            using (SqlConnection cnn = new SqlConnection($"Data Source={serverAddr}; database=master; User Id=SA; Password=P@ssword"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 1000;
                    cmd.CommandText = $"CREATE DATABASE {snapshotName} " +
                                      $"ON ( NAME = {dbname}," +
                                      $"     FILENAME = '{snapshotPath}' ) " +
                                      $"     AS SNAPSHOT OF {dbname};";
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
                cnn.Close();
            }
        }

        public static void RestoreDatabaseBySnapshot(string databaseName, string serverAddr, string snapshotName)
        {
            using (SqlConnection cnn = new SqlConnection($"Data Source={serverAddr}; database=master; User Id=SA; Password=P@ssword"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 1000;
                    cmd.CommandText = $"RESTORE DATABASE {databaseName} " +
                                      $"FROM DATABASE_SNAPSHOT = '{snapshotName}'; " +
                                      $"DROP DATABASE {snapshotName};";
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
                cnn.Close();
            }
        }
    }
}
