using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchedulingApp
{
    public static class TimeZoneHelper
    {
        private static readonly TimeZoneInfo BusinessTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public static TimeZoneInfo UserTimeZone
        {
            get
            {
                return TimeZoneInfo.Local;
            }
        }

        public static string GetUserLocation()
        {
            string timeZoneId = UserTimeZone.Id;

            // Map timezone to office locations
            if (timeZoneId == "Eastern Standard Time")
                return "New York";
            else if (timeZoneId == "Mountain Standard Time")
                return "Phoenix";
            else if (timeZoneId == "GMT Standard Time")
                return "London";
            else
                return "Other Location";
        }

        public static bool HasUpcomingAppointment(out string appointmentInfo)
        {
            appointmentInfo = string.Empty;
            DateTime localNow = DateTime.Now;
            DateTime localFifteenMinutes = localNow.AddMinutes(15);

            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    string query = @"
                        SELECT a.appointmentId, a.start, c.customerName, a.type
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId
                        WHERE a.start BETWEEN @startTime AND @endTime";

                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@startTime", localNow.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@endTime", localFifteenMinutes.ToUniversalTime());

                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime appointmentTime = TimeZoneInfo.ConvertTimeFromUtc(
                                    reader.GetDateTime("start"),
                                    UserTimeZone);

                                appointmentInfo = $"Upcoming appointment at {appointmentTime.ToShortTimeString()}\n" +
                                                $"Customer: {reader.GetString("customerName")}\n" +
                                                $"Type: {reader.GetString("type")}";
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking upcoming appointments: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        // Convert UTC times to local time
        public static DateTime ToLocalTime(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, UserTimeZone);
        }

        public static bool IsWithinBusinessHours(DateTime localTime)
        {
            DateTime businessTime = TimeZoneInfo.ConvertTime(localTime, UserTimeZone, BusinessTimeZone);

            // Check if it's weekend
            if (businessTime.DayOfWeek == DayOfWeek.Saturday || businessTime.DayOfWeek == DayOfWeek.Sunday)
                return false;

            // Check if time is between 9 AM and 5 PM EST
            TimeSpan businessHourStart = new TimeSpan(9, 0, 0);
            TimeSpan businessHourEnd = new TimeSpan(17, 0, 0);
            TimeSpan currentTime = businessTime.TimeOfDay;

            return currentTime >= businessHourStart && currentTime <= businessHourEnd;
        }

        public static bool HasOverlappingAppointment(DateTime start, DateTime end, int appointmentId = 0)
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    string query = @"
                SELECT COUNT(*) FROM appointment 
                WHERE ((start BETWEEN @start AND @end)
                OR (end BETWEEN @start AND @end)
                OR (@start BETWEEN start AND end))
                AND appointmentId != @appointmentId";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@start", start.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@end", end.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);

                        connection.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking overlapping appointments: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Return true on error to prevent scheduling
            }
        }

    }
}
