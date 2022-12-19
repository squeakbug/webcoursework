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

        private static string SSHost = "192.168.10.101";
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

        public static void PgBench(string filename)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };
            using var writer = File.AppendText(filename);
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

        public static void SSBench(string filename)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                NewLine = Environment.NewLine,
                HasHeaderRecord = false
            };

            string text = System.IO.File.ReadAllText("/app/run_result/query_runs_ss.csv");
            Console.WriteLine("1) \n {0} \n\n", text);

            using var writer = File.AppendText(filename);
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
            //csvWriter.WriteField("TransactionsPerSecond");
            //csvWriter.WriteField("SelectQueriesPerSecond");
            //csvWriter.NextRecord();

            Common.SSCreateTestDatabase(TestDBName, SSHost, SSUser, SSPassword);
            Console.WriteLine("DB Created");
            Common.SSCreateDatabaseSnapshot(TestDBName, SSHost, TestDBSnapName, TestSSDBSnapPath, SSUser, SSPassword);
            Console.WriteLine("Snap Created");

            try
            {
                Common.SSFillTestDatabase(TestDBName, SSHost, SSUser, SSPassword);
                Console.WriteLine("Filled");

                var sw = new Stopwatch();
                sw.Start();
                SSBenchQuery(TestDBName, SSHost, SSUser, SSPassword, $"SELECT * FROM dbo.Convertions");
                sw.Stop();
                var elapsedQuery = sw.ElapsedMilliseconds;
                sw.Restart();
                SSBenchTransaction(TestDBName, SSHost, SSUser, SSPassword, $"SELECT * FROM dbo.Convertions");
                sw.Stop();
                var elapsedTransaction = sw.ElapsedMilliseconds;
                csvWriter.WriteField(elapsedQuery.ToString());
                csvWriter.WriteField(elapsedTransaction.ToString());
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

            text = System.IO.File.ReadAllText("/app/run_result/query_runs_ss.csv");
            Console.WriteLine("2) \n {0} \n\n", text);

            using (FileStream fs = File.Create("/app/run_result/test.txt", 1024))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
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
