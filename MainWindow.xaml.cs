////using System;
////using System.IO;
////using System.Windows;
////using System.Windows.Controls;
////using Microsoft.Win32; // For OpenFileDialog
////using System.Data.SqlClient;
////using System.Globalization;
////using System.Transactions;
////namespace FileInsert
////{

////    public partial class MainWindow : Window
////    {
////        private string filePath;
////        private string fileName;
////        private string fileExtension;

////        public struct AnalogData
////        {
////            public int ChannelIndexNumber;
////            public string ChannelId;
////            public string PhaseId;
////            public string Ccbm;
////            public string ChannelUnits;
////            public float ChannelMultiplier;
////            public float ChannelOffset;
////            public float ChannelSkew;
////            public float MinimumLimit;
////            public float MaximumLimit;
////            public float ChannelRatioPrimary;
////            public float ChannelRatioSecondary;
////            public string DataPrimarySecondary;
////        }

////        public struct DigitalData
////        {
////            public int ChannelNumber;
////            public string ChannelId;
////            public string PhaseId;
////            public string Ccbm;
////            public int NormalState;
////        }

////        public struct ComtradeData
////        {
////            public string Station;
////            public string DeviceId;
////            public int CfgVersion;
////            public float LineFrequency;
////            public int SampleRateCount;
////            public float SampleRate;
////            public int LastSampleRate;
////            public string FirstSampleDate;
////            public string FirstSampleTime;
////            public string TriggerDate;
////            public string TriggerTime;
////            public string DataType;
////            public float TimeMultiplier;
////            public string TimeCode;
////            public string LocalCode;
////            public string TimeQualityIndicatorCode;
////            public int LeapSecondIndicator;
////        }

////        public struct ComtradeData1
////        {


////            public int TotalSignalCount;
////            public int AnalogSignalCount;
////            public int DigitalSignalCount;
////        }

////        // Global Variables
////        private static ComtradeData Comtrade;
////        private static List<AnalogData> Analog = new List<AnalogData>();
////        private static List<DigitalData> Digital = new List<DigitalData>();
////        private static ComtradeData1 Comtrade1;
////        private static List<string> Words = new List<string>();
////        private static int ComtradeIndex;

////        public MainWindow()
////        {
////            InitializeComponent();
////        }
////        // Method to extract revised year from a line
////        public static void ExtractRevisedYear(string line)
////        {
////            var tokens = line.Split(',');

////            // Check if there are at least 3 tokens
////            if (tokens.Length > 2)
////            {

////                if (int.TryParse(tokens[2].Trim(), out int revisionYear))
////                {
////                    Comtrade.CfgVersion = revisionYear;
////                    // Console.WriteLine($"Revised Year Extracted: {Comtrade1.RevisionYear}");
////                }
////                Comtrade.Station = tokens[0];
////                Comtrade.DeviceId = tokens[1];
////            }
////            else
////            {
////                //Console.WriteLine("Error: Line does not have enough tokens to extract revised year.");
////            }
////        }



////        // Method to parse a sentence into words
////        public static void ComtradeParse(string sentence)
////        {
////            string[] tokens = sentence.Split(',');
////            foreach (var token in tokens)
////            {
////                if (Words.Count >= 14) return;
////                Words.Add(token);
////            }
////        }

////        // Process parsed words into ComtradeData
////        public static void ProcessWords(List<string> words)
////        {
////            Comtrade.LineFrequency = float.Parse(words[0]);
////            Comtrade.SampleRateCount = int.Parse(words[1]);
////            Comtrade.SampleRate = float.Parse(words[2]);
////            Comtrade.LastSampleRate = int.Parse(words[3]);
////            Comtrade.FirstSampleDate = words[4];
////            Comtrade.FirstSampleTime = words[5];
////            Comtrade.TriggerDate = words[6];
////            Comtrade.TriggerTime = words[7];
////            Comtrade.DataType = words[8];
////            Comtrade.TimeMultiplier = float.Parse(words[9]);
////            Comtrade.TimeCode = words[10];
////            Comtrade.LocalCode = words[11];
////            Comtrade.TimeQualityIndicatorCode = words[12];
////            Comtrade.LeapSecondIndicator = int.Parse(words[13]);
////        }

////        // Extract the revised year from a line


////        // Count signals
////        public static void SignalCounting(string sentence)
////        {
////            var tokens = sentence.Split(',');
////            Comtrade1.TotalSignalCount = int.Parse(tokens[0].Substring(0, 2));
////            Comtrade1.AnalogSignalCount = int.Parse(tokens[1].Substring(0, 2));
////            Comtrade1.DigitalSignalCount = int.Parse(tokens[2].Substring(0, 2));
////        }

////        // Parse and store analog data
////        public static void ParseAndStoreAnalogData(string line, int index)
////        {
////            var tokens = line.Split(',');
////            AnalogData analog = new AnalogData
////            {
////                ChannelIndexNumber = int.Parse(tokens[0]),
////                ChannelId = tokens[1],
////                PhaseId = tokens[2],
////                Ccbm = tokens[3],
////                ChannelUnits = tokens[4],
////                ChannelMultiplier = float.Parse(tokens[5]),
////                ChannelOffset = float.Parse(tokens[6]),
////                ChannelSkew = float.Parse(tokens[7]),
////                MinimumLimit = float.Parse(tokens[8]),
////                MaximumLimit = float.Parse(tokens[9]),
////                ChannelRatioPrimary = int.Parse(tokens[10]),
////                ChannelRatioSecondary = int.Parse(tokens[11]),
////                DataPrimarySecondary = tokens[12]
////            };
////            Analog.Add(analog);
////        }


////        static void AsciiDat(string line, int Analogcount, int DigitalCount)
////        {
////            string connectionString = "Data Source=SANAL-PROEDISON\\SQLEXPRESS;Initial Catalog=Demo;User ID=sa;Password=mypassword;Encrypt=False;";

////            string[] values = line.Split(',');
////            int[] intValues = Array.ConvertAll(values, s =>
////            {
////                if (string.IsNullOrWhiteSpace(s))
////                {
////                    return 0; // or handle it appropriately
////                }
////                return int.Parse(s);
////            });
////            string query4 = "INSERT INTO AnalogDat(ComtradeIndex,ChannelIndex,DatIndex,Time,Value) VALUES (@ComtradeIndex,@ChannelIndex,@DatIndex,@Time,@Value)";

////            using (SqlConnection con = new SqlConnection(connectionString))
////            {
////                con.Open();
////                for (int i = 2; i < Analogcount; i++)
////                {
////                    using (SqlCommand cmdAnalogDat = new SqlCommand(query4, con))
////                    {
////                        cmdAnalogDat.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
////                        cmdAnalogDat.Parameters.AddWithValue("@ChannelIndex", Analog[i - 2].ChannelIndexNumber);
////                        cmdAnalogDat.Parameters.AddWithValue("@DatIndex", intValues[0]);
////                        cmdAnalogDat.Parameters.AddWithValue("@Time", intValues[1]);
////                        cmdAnalogDat.Parameters.AddWithValue("@Value", intValues[i]);

////                        cmdAnalogDat.ExecuteNonQuery();
////                    }
////                }
////            }
////        }

////        public static void ParseAndStoreDigitalData(string line, int index)
////        {
////            var tokens = line.Split(',');

////            // Check if there are enough tokens before processing
////            if (tokens.Length >= 5)
////            {
////                DigitalData digital = new DigitalData
////                {
////                    ChannelNumber = int.Parse(tokens[0]),
////                    ChannelId = tokens[1],
////                    PhaseId = tokens[2],
////                    Ccbm = tokens[3],
////                    NormalState = int.Parse(tokens[4])
////                };
////                Digital.Add(digital);
////            }
////            else
////            {
////                // Handle the case where the line has insufficient tokens

////            }
////        }


////        // Handle "Choose File" button click
////        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
////        {
////            // Open file dialog for selecting a file
////            OpenFileDialog openFileDialog = new OpenFileDialog
////            {
////                Title = "Select a File",
////                Filter = "CFG Files (*.cfg)|*.cfg|DAT Files (*.dat)|*.dat" // Filter for cfg files
////            };

////            // Show the dialog and check if the user selected a file
////            if (openFileDialog.ShowDialog() == true)
////            {
////                filePath = openFileDialog.FileName;
////                //Comtrade1.Filepath = filePath;
////                fileName = Path.GetFileName(filePath);
////                fileExtension = Path.GetExtension(filePath).ToLower();
////            }
////        }

////        // Handle "Process File" button click
////        private void btnProcessFile_Click(object sender, RoutedEventArgs e)
////        {
////            // Check if a file has been selected

////            // MessageBox.Show("Processing...");






////            using (StreamReader file = new StreamReader(filePath))
////            {
////                string line;
////                int analogIndex = 0, digitalIndex = 0;
////                if (string.Equals(fileExtension, ".cfg", StringComparison.OrdinalIgnoreCase))
////                {

////                    // Read and process lines
////                    line = file.ReadLine();
////                    if (line != null)
////                    {
////                        ExtractRevisedYear(line);
////                        if (Comtrade.CfgVersion == 2013)
////                        {

////                            line = file.ReadLine();
////                            SignalCounting(line);

////                            while ((line = file.ReadLine()) != null && analogIndex < Comtrade1.AnalogSignalCount)
////                            {
////                                ParseAndStoreAnalogData(line, analogIndex);
////                                analogIndex++;
////                            }
////                            ParseAndStoreDigitalData(line, digitalIndex);
////                            while ((line = file.ReadLine()) != null && digitalIndex < Comtrade1.DigitalSignalCount - 1)
////                            {
////                                ParseAndStoreDigitalData(line, digitalIndex);
////                                digitalIndex++;
////                            }
////                            ComtradeParse(line);

////                            while ((line = file.ReadLine()) != null)
////                            {

////                                ComtradeParse(line);

////                            }
////                            ProcessWords(Words);


////                        }
////                    }
////                }
////                if (string.Equals(fileExtension, ".dat", StringComparison.OrdinalIgnoreCase))
////                {
////                    using (StreamReader reader = new StreamReader(filePath))
////                    {
////                        string lineDat;

////                        while ((lineDat = reader.ReadLine()) != null)
////                        {
////                            // Process each line
////                            AsciiDat(lineDat, Comtrade1.AnalogSignalCount, Comtrade1.DigitalSignalCount);


////                        }
////                    }

////                    MessageBox.Show("Dat Completed");


////                }



////            }

////            string connectionString = "Data Source=SANAL-PROEDISON\\SQLEXPRESS;Initial Catalog=Demo;User ID=sa;Password=mypassword;Encrypt=False;";

////            // Queries
////            string query = "INSERT INTO COMTRADE (ComtradeIndex,FilePath,FileName,Error,isProedison,HasHDR) VALUES (@ComtradeIndex,@FilePath,@FileName,@Error,@isProedison,@HasHDR)";
////            string query1 = "INSERT INTO Analog(ComtradeIndex,AnalogIndex,ChannelID,Phase,CCBM,Units,Multiplier,Offset,Skew,minVal,maxVal,PrimaryVal,SecondaryVal,ChannelType) VALUES (@ComtradeIndex,@AnalogIndex,@ChannelID,@Phase,@CCBM,@Units,@Multiplier,@Offset,@Skew,@minVal,@maxVal,@PrimaryVal,@SecondaryVal,@ChannelType)";
////            string query2 = "INSERT INTO Digital(ComtradeIndex,DigitalIndex,ChannelID,Phase,CCBM,InitialState) VALUES (@ComtradeIndex,@DigitalIndex,@ChannelID,@Phase,@CCBM,@InitialState)";
////            string query3 = "INSERT INTO CFG(ComtradeIndex,Station,DeviceID,CfgVersion,Frequency,SampleRate,SampleCountHz,LastSampleCount,FirstSampleTime,TriggerTime,DataType,TimeMultiplier,LocalTime,UTCTime,TimeQualityIndicatorCode,LeapSecond) VALUES (@ComtradeIndex,@Station,@DeviceID,@CfgVersion,@Frequency,@SampleRate,@SampleCountHz,@LastSampleCount,@FirstSampleTime,@TriggerTime,@DataType,@TimeMultiplier,@LocalTime,@UTCTime,@TimeQualityIndicatorCode,@LeapSecond)";


////            using (SqlConnection con = new SqlConnection(connectionString))
////            {
////                con.Open();

////                // Start transaction
////                using (SqlTransaction transaction = con.BeginTransaction())
////                {

////                    ComtradeIndex = 0;

////                    string countQuery = "SELECT COUNT(*) FROM COMTRADE";
////                    using (SqlCommand countCmd = new SqlCommand(countQuery, con, transaction))
////                    {
////                        ComtradeIndex = (int)countCmd.ExecuteScalar() + 1;
////                    }

////                    // Enable identity insert


////                    {
////                        // Insert into Comtrade
////                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
////                        {
////                            cmd.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
////                            cmd.Parameters.AddWithValue("@FilePath", filePath);
////                            cmd.Parameters.AddWithValue("@FileName", fileName);
////                            cmd.Parameters.AddWithValue("@Error", 0); // Assuming no error, adjust based on your needs
////                            cmd.Parameters.AddWithValue("@IsProedison", true); // Adjust as per your requirement
////                            cmd.Parameters.AddWithValue("@HasHDR", false); // Adjust as per your requirement

////                            cmd.ExecuteNonQuery();

////                        }





////                        // Insert into AnalogData
////                        for (int i = 0; i < Comtrade1.AnalogSignalCount; i++)
////                        {
////                            using (SqlCommand cmdAnalog = new SqlCommand(query1, con, transaction))
////                            {
////                                cmdAnalog.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
////                                cmdAnalog.Parameters.AddWithValue("@AnalogIndex", Analog[i].ChannelIndexNumber);
////                                cmdAnalog.Parameters.AddWithValue("@ChannelID", Analog[i].ChannelId);
////                                cmdAnalog.Parameters.AddWithValue("@Phase", Analog[i].PhaseId);
////                                cmdAnalog.Parameters.AddWithValue("@CCBM", Analog[i].Ccbm);
////                                cmdAnalog.Parameters.AddWithValue("@Units", Analog[i].ChannelUnits);
////                                cmdAnalog.Parameters.AddWithValue("@Multiplier", Analog[i].ChannelMultiplier);
////                                cmdAnalog.Parameters.AddWithValue("@Offset", Analog[i].ChannelOffset);
////                                cmdAnalog.Parameters.AddWithValue("@Skew", Analog[i].ChannelSkew);
////                                cmdAnalog.Parameters.AddWithValue("@minVal", Analog[i].MinimumLimit);
////                                cmdAnalog.Parameters.AddWithValue("@maxVal", Analog[i].MaximumLimit);
////                                cmdAnalog.Parameters.AddWithValue("@PrimaryVal", Analog[i].ChannelRatioPrimary);
////                                cmdAnalog.Parameters.AddWithValue("@SecondaryVal", Analog[i].ChannelRatioSecondary);
////                                cmdAnalog.Parameters.AddWithValue("@ChannelType", Analog[i].DataPrimarySecondary);


////                                cmdAnalog.ExecuteNonQuery();

////                            }
////                        }

////                        for (int i = 0; i < Comtrade1.DigitalSignalCount; i++)
////                        {
////                            using (SqlCommand cmdDigital = new SqlCommand(query2, con, transaction))
////                            {

////                                cmdDigital.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
////                                cmdDigital.Parameters.AddWithValue("@DigitalIndex", Digital[i].ChannelNumber);
////                                cmdDigital.Parameters.AddWithValue("@ChannelID", Digital[i].ChannelId);
////                                cmdDigital.Parameters.AddWithValue("@Phase", Digital[i].PhaseId);
////                                cmdDigital.Parameters.AddWithValue("@CCBM", Digital[i].Ccbm);
////                                cmdDigital.Parameters.AddWithValue("@InitialState", Digital[i].NormalState);



////                                cmdDigital.ExecuteNonQuery();

////                            }
////                        }


////                        string time1 = Comtrade.FirstSampleDate + " " + Comtrade.FirstSampleTime;
////                        string time2 = Comtrade.TriggerDate + " " + Comtrade.TriggerTime;
////                        string format = "M/d/yyyy HH:mm:ss.ffffff";
////                        //DateTime firstTime = DateTime.ParseExact(time1, format, CultureInfo.InvariantCulture);
////                        //DateTime LastTime = DateTime.ParseExact(time2, format, CultureInfo.InvariantCulture);


////                        using (SqlCommand cmdcfg = new SqlCommand(query3, con, transaction))
////                        {
////                            cmdcfg.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
////                            cmdcfg.Parameters.AddWithValue("@Station", Comtrade.Station);
////                            cmdcfg.Parameters.AddWithValue("@DeviceID", Comtrade.DeviceId);
////                            cmdcfg.Parameters.AddWithValue("@CfgVersion", Comtrade.CfgVersion);
////                            cmdcfg.Parameters.AddWithValue("@Frequency", Comtrade.LineFrequency);
////                            cmdcfg.Parameters.AddWithValue("@SampleRate", Comtrade.SampleRate);
////                            cmdcfg.Parameters.AddWithValue("@SampleCountHz", Comtrade.SampleRateCount);
////                            cmdcfg.Parameters.AddWithValue("@LastSampleCount", Comtrade.LastSampleRate);
////                            cmdcfg.Parameters.AddWithValue("@FirstSampleTime", DateTime.Parse(time1));
////                            cmdcfg.Parameters.AddWithValue("@TriggerTime", DateTime.Parse(time2));
////                            cmdcfg.Parameters.AddWithValue("@DataType", Comtrade.DataType);
////                            cmdcfg.Parameters.AddWithValue("@TimeMultiplier", Comtrade.TimeMultiplier);
////                            cmdcfg.Parameters.AddWithValue("@LocalTime", Comtrade.TimeCode);
////                            cmdcfg.Parameters.AddWithValue("@UTCTime", Comtrade.LocalCode);
////                            cmdcfg.Parameters.AddWithValue("@TimeQualityIndicatorCode", Comtrade.TimeQualityIndicatorCode);
////                            cmdcfg.Parameters.AddWithValue("@LeapSecond", Comtrade.LeapSecondIndicator);


////                            cmdcfg.ExecuteNonQuery();

////                        }

////                        // Commit the transaction
////                        transaction.Commit();
////                        MessageBox.Show("Completed");

////                    }
////                }
////            }
////        }




////    }
////}






////32 seconds




using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // For OpenFileDialog
using System.Data.SqlClient;
using System.Globalization;
using System.Transactions;
using System.Collections.Generic;
using System.Text;

namespace FileReadAndInsert
{
    public partial class MainWindow : Window
    {
        private string filePath;
        private string fileName;
        private string fileExtension;

        public struct AnalogData
        {
            public int ChannelIndexNumber;
            public string ChannelId;
            public string PhaseId;
            public string Ccbm;
            public string ChannelUnits;
            public float ChannelMultiplier;
            public float ChannelOffset;
            public float ChannelSkew;
            public float MinimumLimit;
            public float MaximumLimit;
            public float ChannelRatioPrimary;
            public float ChannelRatioSecondary;
            public string DataPrimarySecondary;
        }

        public struct DigitalData
        {
            public int ChannelNumber;
            public string ChannelId;
            public string PhaseId;
            public string Ccbm;
            public int NormalState;
        }

        public struct ComtradeData
        {
            public string Station;
            public string DeviceId;
            public int CfgVersion;
            public float LineFrequency;
            public int SampleRateCount;
            public float SampleRate;
            public int LastSampleRate;
            public DateTime FirstTimeStamp;
            
            public DateTime TriggerTimeStamp;
            
            public string DataType;
            public float TimeMultiplier;
            public string TimeCode;
            public string LocalCode;
            public string TimeQualityIndicatorCode;
            public int LeapSecondIndicator;
        }

        public struct ComtradeData1
        {
            public int TotalSignalCount;
            public int AnalogSignalCount;
            public int DigitalSignalCount;
        }

        // Global Variables
        private static ComtradeData Comtrade;
        private static List<AnalogData> Analog = new List<AnalogData>();
        private static List<DigitalData> Digital = new List<DigitalData>();
        private static ComtradeData1 Comtrade1;
        private static List<string> Words = new List<string>();
        private static int ComtradeIndex;
        private static List<int> valueArray = new List<int>();
        private static List<int> timeArray = new List<int>();
        private static List<int> datIndexArray = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
        }

        // Method to extract revised year from a line
        public static void ExtractRevisedYear(string line)
        {
            var tokens = line.Split(',');

            // Check if there are at least 3 tokens
            if (tokens.Length > 2)
            {
                if (int.TryParse(tokens[2].Trim(), out int revisionYear))
                {
                    Comtrade.CfgVersion = revisionYear;
                }
                Comtrade.Station = tokens[0];
                Comtrade.DeviceId = tokens[1];
            }
        }

        // Method to parse a sentence into words
        public static void ComtradeParse(string sentence)
        {
            string[] tokens = sentence.Split(',');
            foreach (var token in tokens)
            {
                if (Words.Count >= 14) return;
                Words.Add(token);
            }
        }

        // Process parsed words into ComtradeData
        public static void ProcessWords(List<string> words)
        {
            Comtrade.LineFrequency = float.Parse(words[0]);
            Comtrade.SampleRateCount = int.Parse(words[1]);
            Comtrade.SampleRate = float.Parse(words[2]);
            Comtrade.LastSampleRate = int.Parse(words[3]);
           string time1 = words[4] + " " + words[5];
            Comtrade.FirstTimeStamp = DateTime.Parse(time1);

            string time2 = words[6] + " " + words[7];

           Comtrade.TriggerTimeStamp = DateTime.Parse(time2);

            Comtrade.DataType = words[8];
            Comtrade.TimeMultiplier = float.Parse(words[9]);
            Comtrade.TimeCode = words[10];
            Comtrade.LocalCode = words[11];
            Comtrade.TimeQualityIndicatorCode = words[12];
            Comtrade.LeapSecondIndicator = int.Parse(words[13]);
        }

        // Count signals
        public static void SignalCounting(string sentence)
        {
            var tokens = sentence.Split(',');
            if (tokens.Length !=3)
            {
                MessageBox.Show("Error: Invalid file format.SIgnal Count");
                Application.Current.Shutdown();

            }
            Comtrade1.TotalSignalCount = int.Parse(tokens[0]);
            Comtrade1.AnalogSignalCount = int.Parse(tokens[1].Substring(0, 2));
            Comtrade1.DigitalSignalCount = int.Parse(tokens[2].Substring(0, 2));
        }

        // Parse and store analog data
        public static void ParseAndStoreAnalogData(string line)
        {
            var tokens = line.Split(',');
            if (tokens.Length != 13)
            {
                MessageBox.Show("Error: Invalid file format. Anlog Signal");
                Application.Current.Shutdown();
            }
            AnalogData analog = new AnalogData
            {
                ChannelIndexNumber = int.Parse(tokens[0]),
                ChannelId = tokens[1],
                PhaseId = tokens[2],
                Ccbm = tokens[3],
                ChannelUnits = tokens[4],
                ChannelMultiplier = float.Parse(tokens[5]),
                ChannelOffset = float.Parse(tokens[6]),
                ChannelSkew = float.Parse(tokens[7]),
                MinimumLimit = float.Parse(tokens[8]),
                MaximumLimit = float.Parse(tokens[9]),
                ChannelRatioPrimary = int.Parse(tokens[10]),
                ChannelRatioSecondary = int.Parse(tokens[11]),
                DataPrimarySecondary = tokens[12]
            };
            Analog.Add(analog);
        }


        public static void ParseAndStoreDigitalData(string line)
        {
            var tokens = line.Split(',');

            // Check if there are enough tokens before processing
            if (tokens.Length >= 5)
            {
                DigitalData digital = new DigitalData
                {
                    ChannelNumber = int.Parse(tokens[0]),
                    ChannelId = tokens[1],
                    PhaseId = tokens[2],
                    Ccbm = tokens[3],
                    NormalState = int.Parse(tokens[4])
                };
                Digital.Add(digital);
            }
        }

        static void AsciiDat(string line, int Analogcount, int DigitalCount)
        {
            string connectionString = "Data Source=SANAL-PROEDISON\\SQLEXPRESS;Initial Catalog=Demo;User ID=sa;Password=mypassword;Encrypt=False;";
            int k = 0;
            string[] values = line.Split(',');
            int[] intValues = Array.ConvertAll(values, s =>
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    return 0; // or handle it appropriately
                }
                return int.Parse(s);
            });
            string query4 = "INSERT INTO AnalogDat(ComtradeIndex,AnalogIndex,DatIndex,Time,Value,Result) VALUES (@ComtradeIndex,@AnalogIndex,@DatIndex,@Time,@Value,@Result)";
            string query5 = "INSERT INTO DigitalDat(ComtradeIndex,DigitalIndex,DatIndex,Time,Value) VALUES (@ComtradeIndex,@DigitalIndex,@DatIndex,@Time,@Value)";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    for (int i = 2; i < Analogcount; i++)
                    {
                        float result = 0;
                        if (Analog[i - 2].DataPrimarySecondary.Equals("S") ) {

                            result= ( (intValues[i]* Analog[i-2].ChannelMultiplier) + Analog[i-2].ChannelOffset) * Analog[i - 2].ChannelRatioPrimary;
                        }

                        else
                        {
                            result = (intValues[i] * Analog[i - 2].ChannelMultiplier) + Analog[i - 2].ChannelOffset;
                        }
                        using (SqlCommand cmdAnalogDat = new SqlCommand(query4, con, transaction))
                        {
                            cmdAnalogDat.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                            cmdAnalogDat.Parameters.AddWithValue("@AnalogIndex", Analog[i - 2].ChannelIndexNumber);
                            cmdAnalogDat.Parameters.AddWithValue("@DatIndex", intValues[0]);
                            cmdAnalogDat.Parameters.AddWithValue("@Time", intValues[1]);
                            cmdAnalogDat.Parameters.AddWithValue("@Value", intValues[i]);
                            cmdAnalogDat.Parameters.AddWithValue("@Result", result);


                            cmdAnalogDat.ExecuteNonQuery();
                        }

                       
                    }
                    for (int i = 2 + Comtrade1.AnalogSignalCount; i < intValues.Length; i++)
                    {

                        using (SqlCommand cmdDigitalDat = new SqlCommand(query5, con, transaction))
                        {
                            cmdDigitalDat.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                            cmdDigitalDat.Parameters.AddWithValue("@DigitalIndex", Digital[k].ChannelNumber);
                            cmdDigitalDat.Parameters.AddWithValue("@DatIndex", intValues[0]);
                            cmdDigitalDat.Parameters.AddWithValue("@Time", intValues[1]);
                            cmdDigitalDat.Parameters.AddWithValue("@Value", intValues[i]);
                            

                            cmdDigitalDat.ExecuteNonQuery();

                        }
                        k++;
                    }
                    transaction.Commit();
                }
            }

        }

        static void BinaryDat(string[] hexChunk)
        {
            string connectionString = "Data Source=SANAL-PROEDISON\\SQLEXPRESS;Initial Catalog=Demo;User ID=sa;Password=mypassword;Encrypt=False;";
            string query4 = "INSERT INTO AnalogDat(ComtradeIndex,AnalogIndex,DatIndex,Time,Value,Result) VALUES (@ComtradeIndex,@AnalogIndex,@DatIndex,@Time,@Value,@Result)";



            string indexHex = hexChunk[3] + hexChunk[2] + hexChunk[1] + hexChunk[0];
            string timeStampHex = hexChunk[7] + hexChunk[6] + hexChunk[5] + hexChunk[4];
            string statusBitsHex = hexChunk[hexChunk.Length - 1] + hexChunk[hexChunk.Length - 2];

            int num2 = Convert.ToInt32(statusBitsHex, 16);
            string statusBitsBin = Convert.ToString(num2, 2).PadLeft(16, '0');

            int num = Convert.ToInt32(indexHex, 16);
            int num1 = Convert.ToInt32(timeStampHex, 16);
            timeArray.Add(num1);
            datIndexArray.Add(num);
            int k = 0;
            int ascii;

            for (int i = 8; i < hexChunk.Length - 2; i += 2)
            {

                string x = hexChunk[i + 1] + hexChunk[i];
                int decimalValue = Convert.ToInt32(x, 16);

                string binaryValue = Convert.ToString(decimalValue, 2).PadLeft(16, '0');
                string invertedBits = "";
                for (int j = 0; j < binaryValue.Length; j++)
                {
                    invertedBits += binaryValue[j] == '1' ? '0' : '1'; // Finding One's Complement
                }
                int a = Convert.ToInt32(invertedBits, 2);
                int ans = (a + 1) * (-1);

                if (binaryValue[0] == '1')
                {
                    ascii = ans;
                }
                else
                {
                    ascii = decimalValue;
                }

                float result = 0;
                if (Analog[k].DataPrimarySecondary.Equals("S"))
                {

                    result = ((ascii* Analog[k].ChannelMultiplier) + Analog[k].ChannelOffset) * Analog[k].ChannelRatioPrimary;
                }

                else
                {
                    result = (ascii * Analog[k].ChannelMultiplier) + Analog[k].ChannelOffset;
                }



                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {


                        using (SqlCommand cmdAnalogDat = new SqlCommand(query4, con, transaction))
                        {
                            cmdAnalogDat.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                            cmdAnalogDat.Parameters.AddWithValue("@AnalogIndex", Analog[k].ChannelIndexNumber);
                            cmdAnalogDat.Parameters.AddWithValue("@DatIndex", num);
                            cmdAnalogDat.Parameters.AddWithValue("@Time", num1);
                            cmdAnalogDat.Parameters.AddWithValue("@Value", ascii);
                            cmdAnalogDat.Parameters.AddWithValue("@Result",result);

                            cmdAnalogDat.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }
                k++;





            }
        }

        static void Binary32Dat(string[] hexChunk)
        {

            string connectionString = "Data Source=SANAL-PROEDISON\\SQLEXPRESS;Initial Catalog=Demo;User ID=sa;Password=mypassword;Encrypt=False;";
            string query4 = "INSERT INTO AnalogDat(ComtradeIndex,AnalogIndex,DatIndex,Time,Value,Result) VALUES (@ComtradeIndex,@AnalogIndex,@DatIndex,@Time,@Value,@Result)";
            int k=0;
            int ascii;
            string indexHex = hexChunk[3] + hexChunk[2] + hexChunk[1] + hexChunk[0];
            string timeStampHex = hexChunk[7] + hexChunk[6] + hexChunk[5] + hexChunk[4];
            string statusBitsHex = hexChunk[hexChunk.Length - 1] + hexChunk[hexChunk.Length - 2];

            int num2 = Convert.ToInt32(statusBitsHex, 16);
            string statusBitsBin = Convert.ToString(num2, 2).PadLeft(16, '0');

            int num = Convert.ToInt32(indexHex, 16);
            int num1 = Convert.ToInt32(timeStampHex, 16);
           

            for (int i = 8; i < hexChunk.Length - 5; i += 4)
            {
                string x = hexChunk[i + 3] + hexChunk[i + 2] + hexChunk[i + 1] + hexChunk[i];
                int decimalValue = Convert.ToInt32(x, 16);

                string binaryValue = Convert.ToString(decimalValue, 2).PadLeft(32, '0');

                if (binaryValue[0] == '1')
                {
                    string invertedBits = "";
                    for (int j = 0; j < binaryValue.Length; j++)
                    {
                        invertedBits += binaryValue[j] == '1' ? '0' : '1'; // Finding One's Complement
                    }
                    int a = Convert.ToInt32(invertedBits, 2);
                    int ans = (a + 1) * (-1);
                    ascii = ans;
                }
                else
                {
                    ascii = decimalValue;
                }



                float result = 0;
                if (Analog[k].DataPrimarySecondary.Equals("S"))
                {

                    result = ((ascii * Analog[k].ChannelMultiplier) + Analog[k].ChannelOffset) * Analog[k].ChannelRatioPrimary;
                }

                else
                {
                    result = (ascii * Analog[k].ChannelMultiplier) + Analog[k].ChannelOffset;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {


                        using (SqlCommand cmdAnalogDat = new SqlCommand(query4, con, transaction))
                        {
                            cmdAnalogDat.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                            cmdAnalogDat.Parameters.AddWithValue("@AnalogIndex", Analog[k].ChannelIndexNumber);
                            cmdAnalogDat.Parameters.AddWithValue("@DatIndex", num);
                            cmdAnalogDat.Parameters.AddWithValue("@Time", num1);
                            cmdAnalogDat.Parameters.AddWithValue("@Value", ascii);
                            cmdAnalogDat.Parameters.AddWithValue("@Result", result);

                            cmdAnalogDat.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }
                k++;








            }
        }

        //Function for float32


        static void Float32(string[] hexChunk)
        {

            string connectionString = "Data Source=SANAL-PROEDISON\\SQLEXPRESS;Initial Catalog=Demo;User ID=sa;Password=mypassword;Encrypt=False;";
            string query4 = "INSERT INTO AnalogDat(ComtradeIndex,AnalogIndex,DatIndex,Time,Value,Result) VALUES (@ComtradeIndex,@AnalogIndex,@DatIndex,@Time,@Value,@Result)";

            int k = 0;
            string indexHex = hexChunk[3] + hexChunk[2] + hexChunk[1] + hexChunk[0];
            string timeStampHex = hexChunk[7] + hexChunk[6] + hexChunk[5] + hexChunk[4];
            string statusBitsHex = hexChunk[hexChunk.Length - 1] + hexChunk[hexChunk.Length - 2];

            int num2 = Convert.ToInt32(statusBitsHex, 16);
            string statusBitsBin = Convert.ToString(num2, 2).PadLeft(16, '0');

            int num = Convert.ToInt32(indexHex, 16);
            int num1 = Convert.ToInt32(timeStampHex, 16);
            

            for (int i = 8; i < hexChunk.Length - 5; i += 4)
            {
                string x = hexChunk[i + 3] + hexChunk[i + 2] + hexChunk[i + 1] + hexChunk[i];
                int decimalValue = Convert.ToInt32(x, 16);

                string binaryValue = Convert.ToString(decimalValue, 2).PadLeft(32, '0');

                double mantisa = ConvertBinaryFractionToDecimal(binaryValue.Substring(9)) + 1.0;
                int exponent = Convert.ToInt32(binaryValue.Substring(1, 8), 2) - 127;

                double ascii = mantisa * Math.Pow(2, exponent);
               

                if (binaryValue[0] == '1')
                {
                    ascii = -ascii;
                }
                double result = 0.0;
                if (Analog[k].DataPrimarySecondary.Equals("S"))
                {

                    result = ((ascii * Analog[k].ChannelMultiplier) + Analog[k].ChannelOffset) * Analog[k].ChannelRatioPrimary;
                }

                else
                {
                    result = (ascii * Analog[k].ChannelMultiplier) + Analog[k].ChannelOffset;
                }




                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {


                        using (SqlCommand cmdAnalogDat = new SqlCommand(query4, con, transaction))
                        {
                            cmdAnalogDat.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                            cmdAnalogDat.Parameters.AddWithValue("@AnalogIndex", Analog[k].ChannelIndexNumber);
                            cmdAnalogDat.Parameters.AddWithValue("@DatIndex", num);
                            cmdAnalogDat.Parameters.AddWithValue("@Time", num1);
                            cmdAnalogDat.Parameters.AddWithValue("@Value", ascii);
                            cmdAnalogDat.Parameters.AddWithValue("@Result", result);

                            cmdAnalogDat.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }
                k++;



            }
        }


        static double ConvertBinaryFractionToDecimal(string binaryFraction)
        {
            double decimalValue = 0;

            for (int i = 0; i < binaryFraction.Length; i++)
            {
                int bit = binaryFraction[i] - '0'; // Convert char ('1' or '0') to int
                double fractionalValue = bit * Math.Pow(2, -(i + 1));
                decimalValue += fractionalValue;
            }

            return decimalValue;
        }






       

        // Handle "Choose File" button click
        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog for selecting a file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a File",
                Filter = "CFG Files (*.cfg)|*.cfg|DAT Files (*.dat)|*.dat" // Filter for cfg files
            };

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                fileName = Path.GetFileName(filePath);
                fileExtension = Path.GetExtension(filePath).ToLower();
            }
        }

        // Handle "Process File" button click
        private void btnProcessFile_Click(object sender, RoutedEventArgs e)
        {
            // Check if a file has been selected
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("No file selected.");
                return;
            }

            var fileLines = File.ReadAllLines(filePath);

            
                int analogIndex = 0, digitalIndex = 0;
                if (string.Equals(fileExtension, ".cfg", StringComparison.OrdinalIgnoreCase))
                {
                // Read and process lines
                string line=fileLines[0];
                if (line != null)
                    {
                        ExtractRevisedYear(line);
                        if (Comtrade.CfgVersion == 2013)
                        {
                        line = fileLines[1];
                        SignalCounting(line);


                        if (fileLines.Length != (11 + Comtrade1.AnalogSignalCount + Comtrade1.DigitalSignalCount))
                        {
                            string n=Convert.ToString(fileLines.Length);
                            MessageBox.Show("Error: Invalid file format.Total lines");
                            Application.Current.Shutdown();
                            

                        }
                        if(Comtrade1.TotalSignalCount != (Comtrade1.AnalogSignalCount + Comtrade1.DigitalSignalCount)) {
                            MessageBox.Show("Error: Invalid file format.Total signals");
                            Application.Current.Shutdown();
                        }
                        
                           for(int i=2;i<Comtrade1.AnalogSignalCount+2; i++)
                        {
                           
                            ParseAndStoreAnalogData(fileLines[i]);
                            analogIndex++;
                        }
                       for(int i= Comtrade1.AnalogSignalCount + 2; i < Comtrade1.DigitalSignalCount + Comtrade1.AnalogSignalCount + 2; i++)
                        {
                            ParseAndStoreDigitalData(fileLines[i]);
                            
                        }
                           for(int i=Comtrade1.AnalogSignalCount + Comtrade1.DigitalSignalCount+2;i< fileLines.Length; i++)
                        {
                            ComtradeParse(fileLines[i]);
                           
                        }
                           ProcessWords(Words);
                    }
                    }
                
                if (string.Equals(fileExtension, ".dat", StringComparison.OrdinalIgnoreCase))
                {
                    if ((string.Equals(Comtrade.DataType, "ASCII", StringComparison.OrdinalIgnoreCase)))
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            string lineDat;
                            while ((lineDat = reader.ReadLine()) != null)
                            {
                                // Process each line
                                AsciiDat(lineDat, Comtrade1.AnalogSignalCount, Comtrade1.DigitalSignalCount);
                            }
                        }
                        MessageBox.Show("Ascii Completed");
                    }

                    if ((string.Equals(Comtrade.DataType, "BINARY", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (!File.Exists(filePath))
                        {
                            Console.WriteLine("Error: File does not exist.");
                            return;
                        }

                        byte[] binaryData = File.ReadAllBytes(filePath);

                        // Convert binary data to hexadecimal and store in an array
                        string[] hexArray = new string[binaryData.Length];
                        for (int i = 0; i < binaryData.Length; i++)
                        {
                            hexArray[i] = $"{binaryData[i]:X2}"; // Convert each byte to a 2-digit hex string
                        }

                        // Define chunk size (Based on number of analog signals)
                        int chunkSize = 8 + (2 * Comtrade1.AnalogSignalCount) + (2 * (Comtrade1.DigitalSignalCount / 16));

                        for (int i = 0; i < hexArray.Length; i += chunkSize)
                        {
                            int currentChunkSize = Math.Min(chunkSize, hexArray.Length - i);
                            string[] currentChunk = new string[currentChunkSize];
                            Array.Copy(hexArray, i, currentChunk, 0, currentChunkSize);

                            // Call the processing function with the current chunk
                            BinaryDat(currentChunk);
                        }
                        MessageBox.Show("Binary Completed");

                    }


                    if ((string.Equals(Comtrade.DataType, "BINARY32", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (!File.Exists(filePath))
                        {
                            Console.WriteLine("Error: File does not exist.");
                            return;
                        }

                        byte[] binaryData = File.ReadAllBytes(filePath);

                        // Convert binary data to hexadecimal and store in an array
                        string[] hexArray = new string[binaryData.Length];
                        for (int i = 0; i < binaryData.Length; i++)
                        {
                            hexArray[i] = $"{binaryData[i]:X2}"; // Convert each byte to a 2-digit hex string
                        }

                        // Define chunk size (Based on number of analog signals)
                        int chunkSize = 8 + (4 * Comtrade1.AnalogSignalCount) + (2 * (Comtrade1.DigitalSignalCount / 16));

                        for (int i = 0; i < hexArray.Length; i += chunkSize)
                        {
                            int currentChunkSize = Math.Min(chunkSize, hexArray.Length - i);
                            string[] currentChunk = new string[currentChunkSize];
                            Array.Copy(hexArray, i, currentChunk, 0, currentChunkSize);

                            // Call the processing function with the current chunk
                            Binary32Dat(currentChunk);
                        }
                        MessageBox.Show("Binary32 Completed");

                    }
                    if ((string.Equals(Comtrade.DataType, "FLOAT32", StringComparison.OrdinalIgnoreCase)))
                    {

                        byte[] binaryData = File.ReadAllBytes(filePath);

                        // Convert binary data to hexadecimal and store in an array
                        string[] hexArray = new string[binaryData.Length];
                        for (int i = 0; i < binaryData.Length; i++)
                        {
                            hexArray[i] = $"{binaryData[i]:X2}"; // Convert each byte to a 2-digit hex string
                        }

                        // Define chunk size (Based on number of analog signals)
                       int chunkSize = 8 + (4 * Comtrade1.AnalogSignalCount) + (2 * (Comtrade1.DigitalSignalCount / 16));

                        for (int i = 0; i < hexArray.Length; i += chunkSize)
                        {
                            int currentChunkSize = Math.Min(chunkSize, hexArray.Length - i);
                            string[] currentChunk = new string[currentChunkSize];
                            Array.Copy(hexArray, i, currentChunk, 0, currentChunkSize);

                            // Call the processing function with the current chunk
                            Float32(currentChunk);
                        }
                        MessageBox.Show("Float32 Completed");

                    }





                }
            }

            string connectionString = "Data Source=SANAL-PROEDISON\\SQLEXPRESS;Initial Catalog=Demo;User ID=sa;Password=mypassword;Encrypt=False;";

            // Queries
            string query = "INSERT INTO COMTRADE (ComtradeIndex,FilePath,FileName,Error,isProedison,HasHDR) VALUES (@ComtradeIndex,@FilePath,@FileName,@Error,@isProedison,@HasHDR)";
            string query1 = "INSERT INTO Analog(ComtradeIndex,AnalogIndex,ChannelID,Phase,CCBM,Units,Multiplier,Offset,Skew,minVal,maxVal,PrimaryVal,SecondaryVal,ChannelType) VALUES (@ComtradeIndex,@AnalogIndex,@ChannelID,@Phase,@CCBM,@Units,@Multiplier,@Offset,@Skew,@minVal,@maxVal,@PrimaryVal,@SecondaryVal,@ChannelType)";
            string query2 = "INSERT INTO Digital(ComtradeIndex,DigitalIndex,ChannelID,Phase,CCBM,InitialState) VALUES (@ComtradeIndex,@DigitalIndex,@ChannelID,@Phase,@CCBM,@InitialState)";
            string query3 = "INSERT INTO CFG(ComtradeIndex,Station,DeviceID,CfgVersion,Frequency,SampleRate,SampleCountHz,LastSampleCount,FirstSampleTime,TriggerTime,DataType,TimeMultiplier,LocalTime,UTCTime,TimeQualityIndicatorCode,LeapSecond) VALUES (@ComtradeIndex,@Station,@DeviceID,@CfgVersion,@Frequency,@SampleRate,@SampleCountHz,@LastSampleCount,@FirstSampleTime,@TriggerTime,@DataType,@TimeMultiplier,@LocalTime,@UTCTime,@TimeQualityIndicatorCode,@LeapSecond)";
            string query4 = "INSERT INTO AnalogDat(ComtradeIndex,ChannelIndex,DatIndex,Time,Value) VALUES (@ComtradeIndex,@ChannelIndex,@DatIndex,@Time,@Value)";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                if (string.Equals(fileExtension, ".cfg", StringComparison.OrdinalIgnoreCase))
                {
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        ComtradeIndex = 0;

                        string countQuery = "SELECT COUNT(*) FROM COMTRADE";
                        using (SqlCommand countCmd = new SqlCommand(countQuery, con, transaction))
                        {
                            ComtradeIndex = (int)countCmd.ExecuteScalar() + 1;
                        }

                        // Insert into Comtrade
                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                            cmd.Parameters.AddWithValue("@FilePath", filePath);
                            cmd.Parameters.AddWithValue("@FileName", fileName);
                            cmd.Parameters.AddWithValue("@Error", 0); // Assuming no error, adjust based on your needs
                            cmd.Parameters.AddWithValue("@isProedison", true); // Adjust as per your requirement
                            cmd.Parameters.AddWithValue("@HasHDR", false); // Adjust as per your requirement

                            cmd.ExecuteNonQuery();
                        }

                        // Insert into AnalogData
                        using (SqlCommand cmdAnalog = new SqlCommand(query1, con, transaction))
                        {
                            foreach (var analog in Analog)
                            {
                                cmdAnalog.Parameters.Clear();
                                cmdAnalog.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                                cmdAnalog.Parameters.AddWithValue("@AnalogIndex", analog.ChannelIndexNumber);
                                cmdAnalog.Parameters.AddWithValue("@ChannelID", analog.ChannelId);
                                cmdAnalog.Parameters.AddWithValue("@Phase", analog.PhaseId);
                                cmdAnalog.Parameters.AddWithValue("@CCBM", analog.Ccbm);
                                cmdAnalog.Parameters.AddWithValue("@Units", analog.ChannelUnits);
                                cmdAnalog.Parameters.AddWithValue("@Multiplier", analog.ChannelMultiplier);
                                cmdAnalog.Parameters.AddWithValue("@Offset", analog.ChannelOffset);
                                cmdAnalog.Parameters.AddWithValue("@Skew", analog.ChannelSkew);
                                cmdAnalog.Parameters.AddWithValue("@minVal", analog.MinimumLimit);
                                cmdAnalog.Parameters.AddWithValue("@maxVal", analog.MaximumLimit);
                                cmdAnalog.Parameters.AddWithValue("@PrimaryVal", analog.ChannelRatioPrimary);
                                cmdAnalog.Parameters.AddWithValue("@SecondaryVal", analog.ChannelRatioSecondary);
                                cmdAnalog.Parameters.AddWithValue("@ChannelType", analog.DataPrimarySecondary);

                                cmdAnalog.ExecuteNonQuery();
                            }
                        }

                        // Insert into DigitalData
                        using (SqlCommand cmdDigital = new SqlCommand(query2, con, transaction))
                        {
                            foreach (var digital in Digital)
                            {
                                cmdDigital.Parameters.Clear();
                                cmdDigital.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                                cmdDigital.Parameters.AddWithValue("@DigitalIndex", digital.ChannelNumber);
                                cmdDigital.Parameters.AddWithValue("@ChannelID", digital.ChannelId);
                                cmdDigital.Parameters.AddWithValue("@Phase", digital.PhaseId);
                                cmdDigital.Parameters.AddWithValue("@CCBM", digital.Ccbm);
                                cmdDigital.Parameters.AddWithValue("@InitialState", digital.NormalState);

                                cmdDigital.ExecuteNonQuery();
                            }
                        }

                        // Insert into CFG
                        using (SqlCommand cmdcfg = new SqlCommand(query3, con, transaction))
                        {
                            

                            cmdcfg.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                            cmdcfg.Parameters.AddWithValue("@Station", Comtrade.Station);
                            cmdcfg.Parameters.AddWithValue("@DeviceID", Comtrade.DeviceId);
                            cmdcfg.Parameters.AddWithValue("@CfgVersion", Comtrade.CfgVersion);
                            cmdcfg.Parameters.AddWithValue("@Frequency", Comtrade.LineFrequency);
                            cmdcfg.Parameters.AddWithValue("@SampleRate", Comtrade.SampleRate);
                            cmdcfg.Parameters.AddWithValue("@SampleCountHz", Comtrade.SampleRateCount);
                            cmdcfg.Parameters.AddWithValue("@LastSampleCount", Comtrade.LastSampleRate);
                            cmdcfg.Parameters.AddWithValue("@FirstSampleTime",Comtrade.FirstTimeStamp);
                            cmdcfg.Parameters.AddWithValue("@TriggerTime", Comtrade.TriggerTimeStamp);
                            cmdcfg.Parameters.AddWithValue("@DataType", Comtrade.DataType);
                            cmdcfg.Parameters.AddWithValue("@TimeMultiplier", Comtrade.TimeMultiplier);
                            cmdcfg.Parameters.AddWithValue("@LocalTime", Comtrade.TimeCode);
                            cmdcfg.Parameters.AddWithValue("@UTCTime", Comtrade.LocalCode);
                            cmdcfg.Parameters.AddWithValue("@TimeQualityIndicatorCode", Comtrade.TimeQualityIndicatorCode);
                            cmdcfg.Parameters.AddWithValue("@LeapSecond", Comtrade.LeapSecondIndicator);

                            cmdcfg.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();
                        MessageBox.Show("Completed");
                    }
                }


                //if (string.Equals(Comtrade.DataType, "BINARY", StringComparison.OrdinalIgnoreCase))
                //{
                //    MessageBox.Show("Insertion Started");

                //    int k = 0;

                //    for (int i = 0; i < timeArray.Count; i++)
                //    {

                //        for (int j = 0; j < Comtrade1.AnalogSignalCount; j++)
                //        {



                //            using (SqlCommand cmdAnalogDat = new SqlCommand(query4, con))
                //            {
                //                cmdAnalogDat.Parameters.AddWithValue("@ComtradeIndex", ComtradeIndex);
                //                cmdAnalogDat.Parameters.AddWithValue("@ChannelIndex", Analog[j].ChannelIndexNumber);
                //                cmdAnalogDat.Parameters.AddWithValue("@DatIndex", i);
                //                cmdAnalogDat.Parameters.AddWithValue("@Time", timeArray[i]);
                //                cmdAnalogDat.Parameters.AddWithValue("@Value", valueArray[k + j]);

                //                cmdAnalogDat.ExecuteNonQuery();
                //            }



                //        }
                //        k = k + Comtrade1.AnalogSignalCount;
                //    }
                   // MessageBox.Show("Binary Completed");

                }
            }
        }
    }






