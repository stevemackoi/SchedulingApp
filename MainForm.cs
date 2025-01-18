using System;
using System.Windows.Forms;

namespace SchedulingApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent(); // Called from Designer.cs
            this.Text = "Scheduling Application";
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm();
            customerForm.ShowDialog();
        }

        private void appointmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppointmentForm appointmentForm = new AppointmentForm();
            appointmentForm.ShowDialog();
        }

        private void calendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CalendarView calendarView = new CalendarView();
            calendarView.ShowDialog();
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }
    }
}
