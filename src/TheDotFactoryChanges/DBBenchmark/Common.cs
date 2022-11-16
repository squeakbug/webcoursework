using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Xml.Linq;

namespace DBBenchmark
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options)
            : base(options)
        { }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserConfig> UserConfig { get; set; }
        public DbSet<Convertion> Convertions { get; set; }
        public DbSet<Font> Fonts { get; set; }
    }

    internal static class Common
    {
        private static void FillTestDatabase(DbContextOptions options)
        {
            var globalCtx = new Context(options);
            globalCtx.Database.EnsureCreated();

            var random = new Random();
            int entriesCnt = 1000;
            for (int i = 0; i < entriesCnt; i++)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var name = new string(Enumerable.Repeat(chars, random.Next() % 100)
                                      .Select(s => s[random.Next(s.Length)]).ToArray());
                globalCtx.Convertions.Add(new Convertion
                {
                    Name = name,
                    Body = "body",
                    Head = "head",
                });
            }
            globalCtx.SaveChanges();
        }

        public static void SSFillTestDatabase(string dbname, string serverAddr, string user, string pass)
        {
            string connString = $"Data Source={serverAddr}; database={dbname}; User Id={user}; Password={pass}; Pooling=False";

            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), connString)
                .Options;

            FillTestDatabase(options);
            
        }

        public static void PgFillTestDatabase(string dbname, string serverAddr, string user, string pass)
        {
            string connString = $"Server={serverAddr}; Database={dbname}; Username={user}; Password={pass}";

            var options = NpgsqlDbContextOptionsBuilderExtensions
                .UseNpgsql(new DbContextOptionsBuilder(), connString)
                .Options;

            FillTestDatabase(options);
        }

        public static void SSCreateTestDatabase(string dbname, string serverAddr, string user, string pass)
        {
            string connString = $"Data Source={serverAddr}; database=master; User Id={user}; Password={pass}; Pooling=False";

            using (SqlConnection cnn = new SqlConnection(connString))
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

        public static void PgCreateTestDatabase(string dbname, string serverAddr, string user, string pass)
        {
            string connString = $"Server={serverAddr}; Database={dbname}; Username={user}; Password={pass}";

            using (NpgsqlConnection cnn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 1000;
                    cmd.CommandText = $"IF NOT EXISTS (SELECT FROM pg_database WHERE datname = '{dbname}')" +
                                      $"BEGIN \n" +
                                      $"    CREATE DATABASE {dbname}\n" +
                                      $"END\n";
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
                cnn.Close();
            }
        }

        public static void SSCreateDatabaseSnapshot(string dbname, string serverAddr,
            string snapshotName, string snapshotPath, string user, string pass)
        {
            string connString = $"Data Source={serverAddr}; database=master; User Id={user}; Password={pass}; Pooling=False";

            using (SqlConnection cnn = new SqlConnection(connString))
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

        public static void PgCreateDatabaseSnapshot(string dbname, string serverAddr,
            string snapshotName, string snapshotPath, string user, string pass)
        {
            string connString = $"Server={serverAddr}; Database={dbname}; Username={user}; Password={pass}";

            using (NpgsqlConnection cnn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
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

        public static void SSRestoreDatabaseBySnapshot(string databaseName, string serverAddr,
            string snapshotName, string user, string pass)
        {
            string connString = $"Data Source={serverAddr}; database=master; User Id={user}; Password={pass}; Pooling=False";

            using (SqlConnection cnn = new SqlConnection(connString))
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

        public static void PgRestoreDatabaseBySnapshot(string dbname, string serverAddr,
            string snapshotName, string user, string pass)
        {
            string connString = $"Server={serverAddr}; Database={dbname}; Username={user}; Password={pass}";

            using (NpgsqlConnection cnn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandTimeout = 1000;
                    cmd.CommandText = $"RESTORE DATABASE {dbname} " +
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
