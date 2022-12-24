using System;
using System.Diagnostics;
using System.Text;
using System.Data.SqlClient;
using System.IO;

using CsvHelper;
using CsvHelper.Configuration;
using Npgsql;
using System.Globalization;
using System.Net.Sockets;

namespace DBBenchmark
{
    internal class Program
    {
        private static string PgHost = "192.168.43.215";
        private static string PgUser = "postgres";
        private static string PgDBname = "mypgsqldb";
        private static string PgPassword = "postgres";

        private static string SSHost = "192.168.43.215";
        private static string SSUser = "SA";
        private static string SSDBname = "master";
        private static string SSPassword = "P@ssword";
        private static string SSPort = "1433";

        private static string TestDBName = "the_dotfactory_test";
        private static string TestDBSnapName = "the_dotfactory_test_snap";
        private static string TestSSDBSnapPath = $@"/var/opt/mssql/data/{TestDBSnapName}.ss";
        private static string TestPgDBSnapPath = $@"/var/opt/pgsql/data/{TestDBSnapName}.ss";

        private static string SSQueryStatFilePath = $@"/app/run_result/query_runs_pg.csv";
        private static string PgQueryStatFilePath = $@"/app/run_result/query_runs_ss.csv";
        private static string SSQueryStatFilePathWin = $@"query_runs_pg.csv";
        private static string PgQueryStatFilePathWin = $@"query_runs_ss.csv";

        private static int DataPort = 8050;
        private static int runsCount = 100;

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
            string connString = $"Server={serverAddr}; Port=5432; database={dbname}; User Id={user}; Password={pass}; Pooling=False";

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
                    cmd.CommandText = "BEGIN TRANSACTION;";
                    cnn.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = commText;
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "COMMIT TRANSACTION;";
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                cnn.Close();
            }
        }

        public static void PgBenchTransaction(string dbname, string serverAddr, string user, string pass, string commText)
        {
            string connString = $"Server={serverAddr}; Port=5432; database={dbname}; User Id={user}; Password={pass}; Pooling=False";

            using (NpgsqlConnection cnn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "BEGIN;";
                    cnn.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = commText;
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "COMMIT;";
                    var reader = cmd.ExecuteReader();
                    reader.Close();
                }
                cnn.Close();
            }
        }

        public static void PgBench(string filename)
        {
            using var writer = File.AppendText(filename);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            Common.PgCreateTestDatabase(TestDBName, PgHost, PgUser, PgPassword);
            Console.WriteLine("DB Created");
            Common.PgCreateDatabaseSnapshot(TestDBName, PgHost, TestDBSnapName, TestPgDBSnapPath, PgUser, PgPassword);
            Console.WriteLine("Snap Created");

            try
            {
                Common.PgFillTestDatabase(TestDBName, PgHost, PgUser, PgPassword);
                Console.WriteLine("Filled");
                long totalTime = 0;

                var sw = new Stopwatch();
                for (int i = 0; i < runsCount; i++)
                {
                    sw.Restart();
                    PgBenchQuery(TestDBName, PgHost, PgUser, PgPassword, $"SELECT * FROM public.\"Convertions\"");
                    sw.Stop();
                    var elapsed = sw.ElapsedMilliseconds;
                    totalTime += elapsed;
                }
                csvWriter.WriteField(((double)totalTime / runsCount).ToString());

                totalTime = 0;
                for (int i = 0; i < runsCount; i++)
                {
                    sw.Restart();
                    PgBenchTransaction(TestDBName, PgHost, PgUser, PgPassword, $"SELECT * FROM public.\"Convertions\"");
                    sw.Stop();
                    var elapsed = sw.ElapsedMilliseconds;
                    totalTime += elapsed;
                }
                csvWriter.WriteField(((double)totalTime / runsCount).ToString());

                csvWriter.NextRecord();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Common.PgRestoreDatabaseBySnapshot(TestDBName, PgHost, TestDBSnapName, PgUser, PgPassword);
            }
            writer.Flush();

            string text = System.IO.File.ReadAllText("/app/run_result/query_runs_pg.csv");
            SendDataTo(text, PgHost, DataPort);
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            Console.WriteLine("Processed file '{0}'.", path);
        }

        private static void SendDataTo(string textToSend, string ip, int port)
        {
            TcpClient client = new TcpClient(ip, port);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            client.Close();
        }

        public static void SSBench(string filename)
        {
            using var writer = File.AppendText(filename);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            Common.SSCreateTestDatabase(TestDBName, SSHost, SSUser, SSPassword);
            Console.WriteLine("DB Created");
            Common.SSCreateDatabaseSnapshot(TestDBName, SSHost, TestDBSnapName, TestSSDBSnapPath, SSUser, SSPassword);
            Console.WriteLine("Snap Created");

            try
            {
                Common.SSFillTestDatabase(TestDBName, SSHost, SSUser, SSPassword);
                Console.WriteLine("Filled");
                long totalTime = 0;

                for (int i = 0; i < runsCount; i++)
                {
                    var sw = new Stopwatch();
                    sw.Restart();
                    SSBenchQuery(TestDBName, SSHost, SSUser, SSPassword, $"SELECT * FROM dbo.Convertions");
                    sw.Stop();
                    var elapsedQuery = sw.ElapsedMilliseconds;
                    totalTime += elapsedQuery;
                }
                csvWriter.WriteField(((double)totalTime / runsCount).ToString());

                totalTime = 0;
                for (int i = 0; i < runsCount; i++)
                {
                    var sw = new Stopwatch();
                    sw.Restart();
                    SSBenchTransaction(TestDBName, SSHost, SSUser, SSPassword, $"SELECT * FROM dbo.Convertions");
                    sw.Stop();
                    var elapsedTransaction = sw.ElapsedMilliseconds;
                    totalTime += elapsedTransaction;
                }
                csvWriter.WriteField(((double)totalTime / runsCount).ToString());

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

            string text = System.IO.File.ReadAllText("/app/run_result/query_runs_ss.csv");
            SendDataTo(text, PgHost, DataPort);
        }

        static void Main(string[] args)
        {
            string dbmsName = args[2];
            string statFilename = args[1];
            Console.WriteLine("dbname: {0}, statfile: {1}", dbmsName, statFilename);

            if (dbmsName == "pgdb")
            {
                Console.WriteLine("Starting PostgreSQL benchmark");
                PgBench(statFilename);
            }
            else if (dbmsName == "ssdb")
            {
                Console.WriteLine("Starting SQL Server benchmark");
                SSBench(statFilename);
            }

            Console.WriteLine("Exit");
        }
    }
}
