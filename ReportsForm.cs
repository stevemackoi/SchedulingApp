using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchedulingApp
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            LoadReportData();
        }

        private void LoadReportData()
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    LoadAppointmentTypesByMonth(connection);
                    LoadUserSchedules(connection);
                    LoadCustomerAppointmentCounts(connection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reports: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAppointmentTypesByMonth(MySqlConnection connection)
        {
            string query = @"
                SELECT 
                    MONTH(start) as Month,
                    type,
                    COUNT(*) as Count
                FROM appointment
                GROUP BY MONTH(start), type
                ORDER BY MONTH(start), type";

            var dt = new DataTable();
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                adapter.Fill(dt);
            }

            // Use lambda to group and format the data
            var report = dt.AsEnumerable()
                .GroupBy(row => row.Field<int>("Month"))
                .Select(group => new
                {
                    Month = new DateTime(2024, group.Key, 1).ToString("MMMM"),
                    Types = group.Select(row => new
                    {
                        Type = row.Field<string>("type"),
                        Count = row.Field<long>("Count")
                    })
                });

            foreach (var month in report)
            {
                txtTypesByMonth.AppendText($"\r\n{month.Month}:\r\n");
                foreach (var type in month.Types)
                {
                    txtTypesByMonth.AppendText($"  {type.Type}: {type.Count}\r\n");
                }
            }
        }

        private void LoadUserSchedules(MySqlConnection connection)
        {
            string query = @"
                SELECT 
                    u.userName,
                    a.start,
                    a.end,
                    c.customerName,
                    a.type
                FROM appointment a
                JOIN user u ON a.userId = u.userId
                JOIN customer c ON a.customerId = c.customerId
                ORDER BY u.userName, a.start";

            var dt = new DataTable();
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                adapter.Fill(dt);
            }

            // Use lambda to group and format the data
            var schedules = dt.AsEnumerable()
                .GroupBy(row => row.Field<string>("userName"))
                .Select(group => new
                {
                    UserName = group.Key,
                    Appointments = group.Select(row => new
                    {
                        Start = TimeZoneHelper.ToLocalTime(row.Field<DateTime>("start")),
                        End = TimeZoneHelper.ToLocalTime(row.Field<DateTime>("end")),
                        Customer = row.Field<string>("customerName"),
                        Type = row.Field<string>("type")
                    }).OrderBy(a => a.Start)
                });

            foreach (var user in schedules)
            {
                txtUserSchedules.AppendText($"\r\n{user.UserName}:\r\n");
                foreach (var apt in user.Appointments)
                {
                    txtUserSchedules.AppendText(
                        $"  {apt.Start:g} - {apt.End:t}\n" +
                        $"  Customer: {apt.Customer}\n" +
                        $"  Type: {apt.Type}\r\n\n");
                }
            }
        }

        private void LoadCustomerAppointmentCounts(MySqlConnection connection)
        {
            string query = @"
                SELECT 
                    c.customerName,
                    COUNT(*) as AppointmentCount
                FROM customer c
                LEFT JOIN appointment a ON c.customerId = a.customerId
                GROUP BY c.customerId, c.customerName
                ORDER BY COUNT(*) DESC";

            var dt = new DataTable();
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                adapter.Fill(dt);
            }

            // Use lambda to format the data
            var customerCounts = dt.AsEnumerable()
                .Select(row => new
                {
                    Customer = row.Field<string>("customerName"),
                    Count = row.Field<long>("AppointmentCount")
                })
                .OrderByDescending(x => x.Count);

            foreach (var customer in customerCounts)
            {
                txtCustomerCounts.AppendText(
                    $"{customer.Customer}: {customer.Count} appointment(s)\r\n");
            }
        }
    }
}
