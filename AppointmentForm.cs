using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Linq;

namespace SchedulingApp
{
    public partial class AppointmentForm : Form
    {
        private BindingList<Appointment> appointments;
        private Appointment selectedAppointment;

        public AppointmentForm()
        {
            InitializeComponent();
            LoadAppointments();
            LoadCustomers();
            SetupTypeComboBox();
            ConfigureControls();
        }

        private void ConfigureControls()
        {
            // Set time format for time pickers
            dtpStartTime.Format = DateTimePickerFormat.Time;
            dtpStartTime.ShowUpDown = true;
            dtpEndTime.Format = DateTimePickerFormat.Time;
            dtpEndTime.ShowUpDown = true;
        }

        private void LoadAppointments()
        {
            try
            {
                appointments = new BindingList<Appointment>();
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(@"
                        SELECT a.appointmentId, a.customerId, c.customerName, 
                               a.type, a.start, a.end
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                appointments.Add(new Appointment
                                {
                                    AppointmentId = reader.GetInt32("appointmentId"),
                                    CustomerId = reader.GetInt32("customerId"),
                                    CustomerName = reader.GetString("customerName"),
                                    Type = reader.GetString("type"),
                                    Start = reader.GetDateTime("start"),
                                    End = reader.GetDateTime("end")
                                });
                            }
                        }
                    }
                }
                appointmentGridView.DataSource = appointments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}");
            }
        }

        private void LoadCustomers()
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT customerId, customerName FROM customer", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cboCustomer.Items.Add(new CustomerItem
                                {
                                    Id = reader.GetInt32("customerId"),
                                    Name = reader.GetString("customerName")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}");
            }
        }

        private void SetupTypeComboBox()
        {
            cboType.Items.AddRange(new string[] {
                "Initial Consultation",
                "Follow-up",
                "Status Update",
                "General Meeting",
                "Project Review"
            });
        }

        private void ValidateAppointment(DateTime start, DateTime end)
        {
            // Convert to EST for business hours check
            TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estStart = TimeZoneInfo.ConvertTime(start, estZone);
            DateTime estEnd = TimeZoneInfo.ConvertTime(end, estZone);

            // Check business hours (9 AM - 5 PM EST)
            if (estStart.Hour < 9 || estEnd.Hour >= 17)
                throw new Exception("Appointments must be scheduled between 9 AM and 5 PM EST");

            // Check business days
            if (estStart.DayOfWeek == DayOfWeek.Saturday || estStart.DayOfWeek == DayOfWeek.Sunday)
                throw new Exception("Appointments must be scheduled Monday through Friday");

            // Check for overlapping appointments
            var overlapping = appointments.Any(a =>
                a.AppointmentId != (selectedAppointment?.AppointmentId ?? 0) &&
                ((start >= a.Start && start < a.End) ||
                 (end > a.Start && end <= a.End) ||
                 (start <= a.Start && end >= a.End)));

            if (overlapping)
                throw new Exception("This appointment overlaps with an existing appointment");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboCustomer.SelectedItem == null)
                    throw new Exception("Please select a customer");

                if (string.IsNullOrEmpty(cboType.Text))
                    throw new Exception("Please select an appointment type");

                var startDate = dtpDate.Value.Date + dtpStartTime.Value.TimeOfDay;
                var endDate = dtpDate.Value.Date + dtpEndTime.Value.TimeOfDay;

                ValidateAppointment(startDate, endDate);

                var customerId = ((CustomerItem)cboCustomer.SelectedItem).Id;

                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(@"
                        INSERT INTO appointment (customerId, type, start, end, createDate, createdBy)
                        VALUES (@customerId, @type, @start, @end, NOW(), 'test')", conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@type", cboType.Text);
                        cmd.Parameters.AddWithValue("@start", startDate.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@end", endDate.ToUniversalTime());
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Appointment added successfully!");
                LoadAppointments();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedAppointment == null)
                    throw new Exception("Please select an appointment to update");

                if (cboCustomer.SelectedItem == null)
                    throw new Exception("Please select a customer");

                if (string.IsNullOrEmpty(cboType.Text))
                    throw new Exception("Please select an appointment type");

                var startDate = dtpDate.Value.Date + dtpStartTime.Value.TimeOfDay;
                var endDate = dtpDate.Value.Date + dtpEndTime.Value.TimeOfDay;

                ValidateAppointment(startDate, endDate);

                var customerId = ((CustomerItem)cboCustomer.SelectedItem).Id;

                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(@"
                        UPDATE appointment 
                        SET customerId = @customerId,
                            type = @type,
                            start = @start,
                            end = @end,
                            lastUpdate = NOW(),
                            lastUpdateBy = 'test'
                        WHERE appointmentId = @appointmentId", conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@type", cboType.Text);
                        cmd.Parameters.AddWithValue("@start", startDate.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@end", endDate.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@appointmentId", selectedAppointment.AppointmentId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Appointment updated successfully!");
                LoadAppointments();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedAppointment == null)
                    throw new Exception("Please select an appointment to delete");

                if (MessageBox.Show("Are you sure you want to delete this appointment?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (var conn = DBConnection.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand(
                            "DELETE FROM appointment WHERE appointmentId = @appointmentId", conn))
                        {
                            cmd.Parameters.AddWithValue("@appointmentId", selectedAppointment.AppointmentId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Appointment deleted successfully!");
                    LoadAppointments();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void appointmentGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (appointmentGridView.SelectedRows.Count > 0)
            {
                selectedAppointment = (Appointment)appointmentGridView.SelectedRows[0].DataBoundItem;

                // Find and select the customer in combo box
                foreach (CustomerItem item in cboCustomer.Items)
                {
                    if (item.Id == selectedAppointment.CustomerId)
                    {
                        cboCustomer.SelectedItem = item;
                        break;
                    }
                }

                cboType.Text = selectedAppointment.Type;
                dtpDate.Value = selectedAppointment.Start.Date;
                dtpStartTime.Value = selectedAppointment.Start;
                dtpEndTime.Value = selectedAppointment.End;
            }
        }

        private void ClearFields()
        {
            cboCustomer.SelectedIndex = -1;
            cboType.SelectedIndex = -1;
            dtpDate.Value = DateTime.Now;
            dtpStartTime.Value = DateTime.Now;
            dtpEndTime.Value = DateTime.Now;
            selectedAppointment = null;
        }
    }

    public class CustomerItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
