using System;
using System.Net;
using System.Net.Http;


namespace TumorDiagnoser.Model
{
    // This is a file based storage
    public static class Database
    {
        private const string CredentialsFile = @"Database\credentials.txt";
        private const string AppointmentsFile = @"Database\appointments.txt";
        private const string ImagesFile = @"Database\images.txt";
        private const string UndiagnosedAppointmentsFile = @"Database\undiagnosedAppointments.txt";
        private const string DiagnosisFile = @"Database\diagnosis.txt";

        public static string GetPassword(string username)
        {
            foreach (var line in File.ReadAllLines(CredentialsFile))
            {
                if (line.StartsWith(username, StringComparison.OrdinalIgnoreCase))
                {
                    return line.Substring(username.Length + 1);
                }
            }

            return string.Empty;
        }

        public static bool IsSlotAvailable(string dateTime)
        {
            string date = dateTime.Split(' ')[0];
            string time = dateTime.Split(" ")[1];
            foreach (var line in File.ReadAllLines(AppointmentsFile))
            {
                if (line.StartsWith(date) && line.EndsWith(time))
                    return false;
            }
            return true;
        }

        public static void BookSlot(string username, string dateTime)
        {
            File.AppendAllText(AppointmentsFile, $"{username}{dateTime}\n");
        }

        public static bool UploadImages(string username, string appointmentId, string imageUrl)
        {
            if (!IsUrlAccessible(imageUrl))
                return false;
            File.AppendAllText(ImagesFile, $"{username},{appointmentId},{imageUrl}\n");
            File.AppendAllText(UndiagnosedAppointmentsFile, $"{appointmentId}\n");
            return true;
        }

        private static bool IsUrlAccessible(string imageUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.Send(new HttpRequestMessage(HttpMethod.Head, imageUrl));

                return response.IsSuccessStatusCode;
            }
        }

        public static List<string> GetUndiagnosedAppointments()
        {
            List<string> undiagnosedAppointments = File.ReadAllLines(UndiagnosedAppointmentsFile).ToList();
            return undiagnosedAppointments;
        }

        public static bool UpdateDiagnosis(string username, string appointmentId, string tumorDetail, string diagnosis)
        {
            File.AppendAllText(DiagnosisFile, $"{username},{appointmentId},{tumorDetail},{diagnosis}");
            var undiagnosedAppointments = File.ReadAllLines(UndiagnosedAppointmentsFile).Where(x => !x.Contains(appointmentId));
            File.Delete(UndiagnosedAppointmentsFile);
            File.WriteAllLines(UndiagnosedAppointmentsFile, undiagnosedAppointments);
            return true;
        }

        public static List<string> GetLatestDiagnosis(string username)
        {
            List<string> results = new List<string>();
            foreach (var line in File.ReadAllLines(DiagnosisFile))
            {
                if (line.StartsWith(username))
                {
                    List<string> currentResults = line.Split(',').ToList();
                    results.Append(username);
                    results.Append(results[2]);
                    results.Append(results[3]);
                }
            }
            return results;
        }
    }
}
