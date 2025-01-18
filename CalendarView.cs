using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchedulingApp
{
    public partial class CalendarView : Form
    {
        public CalendarView()
        {
            InitializeComponent();
            SetupCalendarView();
        }

        private void SetupCalendarView()
        {
            // Set initial date
            monthCalendar.SelectionStart = DateTime.Today;
            selectedDateLabel.Text = $"Appointments for {DateTime.Today.ToLongDateString()}";
            LoadAppointments(DateTime.Today);
        }

        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            LoadAppointments(e.Start);
            selectedDateLabel.Text = $"Appointments for {e.Start.ToLongDateString()}";
        }

        private void LoadAppointments(DateTime date)
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    string query = @"
                        SELECT 
                            a.appointmentId,
                            c.customerName,
                            a.title,
                            a.description,
                            a.location,
                            a.type,
                            a.start,
                            a.end
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId
                        WHERE DATE(a.start) = @date
                        ORDER BY a.start";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@date", date.Date);

                    DataTable dt = new DataTable();
                    connection.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                    appointmentGrid.DataSource = dt;

                    // Format grid columns
                    if (appointmentGrid.Columns.Count > 0)
                    {
                        appointmentGrid.Columns["appointmentId"].HeaderText = "ID";
                        appointmentGrid.Columns["customerName"].HeaderText = "Customer";
                        appointmentGrid.Columns["title"].HeaderText = "Title";
                        appointmentGrid.Columns["description"].HeaderText = "Description";
                        appointmentGrid.Columns["location"].HeaderText = "Location";
                        appointmentGrid.Columns["type"].HeaderText = "Type";
                        appointmentGrid.Columns["start"].HeaderText = "Start Time";
                        appointmentGrid.Columns["end"].HeaderText = "End Time";

                        // Format datetime columns
                        appointmentGrid.Columns["start"].DefaultCellStyle.Format = "g";
                        appointmentGrid.Columns["end"].DefaultCellStyle.Format = "g";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void appointmentGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int appointmentId = Convert.ToInt32(appointmentGrid.Rows[e.RowIndex].Cells["appointmentId"].Value);
                // TODO: Implement opening appointment details
                MessageBox.Show($"Opening appointment {appointmentId}");
            }
        }
    }
}
