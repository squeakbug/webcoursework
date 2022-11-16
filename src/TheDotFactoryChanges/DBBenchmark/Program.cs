using System;
using System.Diagnostics;
using System.Text;
using System.Data.SqlClient;
using System.IO;

using CsvHelper;
using CsvHelper.Configuration;
using Npgsql;
using System.Globalization;

namespace DBBenchmark
{
    internal class Program
    {
        private static string PgHost = "localhost";
        private static string PgUser = "postgres";
        private static string PgDBname = "mypgsqldb";
        private static string PgPassword = "postgres";
        private static string PgPort = "5432";

        private static string SSHost = "localhost,1433";
        private static string SSUser = "SA";
        private static string SSDBname = "master";
        private static string SSPassword = "P@ssword";
        private static string SSPort = "1433";

        private static string TestDBName = "the_dotfactory_test";
        private static string TestDBSnapName = "the_dotfactory_test_snap";
        private static string TestSSDBSnapPath = $@"/var/opt/mssql/data/{TestDBSnapName}.ss";
        private static string TestPgDBSnapPath = $@"/var/opt/pgsql/data/{TestDBSnapName}.ss";

        private static string SSQueryStatFilePath = $@"/var/opt/run_result/query_runs_pg.csv";
        private static string PgQueryStatFilePath = $@"/var/opt/run_result/query_runs_ss.csv";

        public static void SSBenchQuery(string dbname, string serverAddr, string user, string pass, string commText)
        {
            string connString = $"Data Source={serverAddr}; database={dbname};" +
                                $" User Id={user}; Password={pass}; Pooling=False";

            using (SqlConnection cnn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = commText;
                    cnn.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                cnn.Close();
            }
        }

        public static void PgBenchQuery(string dbname, string serverAddr, string user, string pass, string commText)
        {
            string connString = $"Server={serverAddr}; Database={dbname}; Username={user}; Password={pass}";

            using (NpgsqlConnection cnn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = commText;
                    cnn.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                cnn.Close();
            }
        }

        public static void SSBenchTransaction(string dbname, string serverAddr, string user, string pass, string commText)
        {
            string connString = $"Data Source={serverAddr}; database={dbname};" +
                                $" User Id={user}; Password={pass}; Pooling=False";

            using (SqlConnection cnn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = commText;
                    cnn.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                cnn.Close();
            }
        }

        public static void PgBenchTransaction(string dbname, string serverAddr, string user, string pass, string commText)
        {
            string connString = $"Server={serverAddr}; Database={dbname}; Username={user}; Password={pass}";

            using (NpgsqlConnection cnn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = commText;
                    cnn.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                cnn.Close();
            }
        }

        public static void PgBench()
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };
            using var writer = File.AppendText(PgQueryStatFilePath);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
            csvWriter.WriteField("TransactionsPerSecond");
            csvWriter.WriteField("SelectQueriesPerSecond");
            csvWriter.NextRecord();

            try
            {
                Common.PgCreateTestDatabase(TestDBName, PgHost, PgUser, PgPassword);
                Common.PgCreateDatabaseSnapshot(TestDBName, PgHost, TestDBSnapName, TestPgDBSnapPath, PgUser, PgPassword);
                Common.PgFillTestDatabase(TestDBName, PgHost, PgUser, PgPassword);

                var sw = new Stopwatch();
                sw.Start();
                PgBenchQuery(TestDBName, PgHost, PgUser, PgPassword, $"SELECT * FROM dbo.Convertions");
                sw.Stop();
                var elapsed = sw.ElapsedMilliseconds;
                csvWriter.WriteField(elapsed.ToString());

                sw.Start();
                PgBenchTransaction(TestDBName, PgHost, PgUser, PgPassword, $"SELECT * FROM dbo.Convertions");
                sw.Stop();
                elapsed = sw.ElapsedMilliseconds;
                csvWriter.WriteField(elapsed.ToString());

                csvWriter.NextRecord();
            }
            finally
            {
                Common.SSRestoreDatabaseBySnapshot(TestDBName, PgHost, TestDBSnapName, PgUser, PgPassword);
            }

            writer.Flush();
        }

        public static void SSBench()
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };
            using var writer = File.AppendText(SSQueryStatFilePath);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
            csvWriter.WriteField("TransactionsPerSecond");
            csvWriter.WriteField("SelectQueriesPerSecond");
            csvWriter.NextRecord();

            try
            {
                Common.SSCreateTestDatabase(TestDBName, SSHost, SSUser, SSPassword);
                Common.SSCreateDatabaseSnapshot(TestDBName, SSHost, TestDBSnapName, TestSSDBSnapPath, SSUser, SSPassword);
                Common.SSFillTestDatabase(TestDBName, SSHost, SSUser, SSPassword);

                var sw = new Stopwatch();
                sw.Start();
                SSBenchQuery(TestDBName, SSHost, SSUser, SSPassword, $"SELECT * FROM dbo.Convertions");
                sw.Stop();
                var elapsed = sw.ElapsedMilliseconds;
                csvWriter.WriteField(elapsed.ToString());
                csvWriter.WriteField(elapsed.ToString());
                csvWriter.NextRecord();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Common.SSRestoreDatabaseBySnapshot(TestDBName, SSHost, TestDBSnapName, SSUser, SSPassword);
            }

            writer.Flush();
        }

        static void Main(string[] args)
        {
            string dbmsName = args[1];

            if (dbmsName == "pg")
            {
                Console.WriteLine("Starting PostgreSQL benchmark");
                PgBench();
            }
            else if (dbmsName == "ss")
            {
                Console.WriteLine("Starting SQL Server benchmark");
                SSBench();
            }

            Console.WriteLine("Exit");
        }
    }
}
