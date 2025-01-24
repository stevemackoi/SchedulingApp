using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using SchedulingApp.Properties;

namespace SchedulingApp
{
    public partial class LoginForm : Form
    {
        private CultureInfo currentCulture;

        public LoginForm()
        {
            InitializeComponent();
            InitializeLanguageSettings();
            DisplayTimeZoneInfo();
        }

        private void InitializeLanguageSettings()
        {
            // Detect system language, but allow manual override
            currentCulture = CultureInfo.CurrentUICulture.Name.StartsWith("es")
                ? new CultureInfo("es-ES")
                : new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = currentCulture;
            UpdateUILanguage();
        }

        private void DisplayTimeZoneInfo()
        {
            string location = TimeZoneHelper.GetUserLocation();
            string timeZone = TimeZoneHelper.UserTimeZone.DisplayName;
            lblLocation.Text = $"{Resources.LocationLabel} {location}\n{timeZone}";
        }

        private void UpdateUILanguage()
        {
            try
            {
                // Update form text
                this.Text = Resources.LoginFormTitle;
                lblUsername.Text = Resources.Username;
                lblPassword.Text = Resources.Password;
                btnLogin.Text = Resources.LoginButton;
                btnToggleLanguage.Text = Resources.LanguageButton;

                // Update location label while preserving the current location info
                string location = TimeZoneHelper.GetUserLocation();
                string timeZone = TimeZoneHelper.UserTimeZone.DisplayName;
                lblLocation.Text = $"{Resources.LocationLabel} {location}\n{timeZone}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading language resources: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnToggleLanguage_Click(object sender, EventArgs e)
        {
            // Manual language toggle remains unchanged
            currentCulture = currentCulture.Name.StartsWith("es")
                ? new CultureInfo("en-US")
                : new CultureInfo("es-ES");

            Thread.CurrentThread.CurrentUICulture = currentCulture;
            UpdateUILanguage();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "test" && password == "test")
            {
                LogLogin(username, true);
                HandleSuccessfulLogin(username);
            }
            else
            {
                LogLogin(username, false);
                MessageBox.Show(Resources.InvalidLogin,
                    Resources.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void HandleSuccessfulLogin(string username)
        {
            // Check for upcoming appointments
            string appointmentInfo;
            if (TimeZoneHelper.HasUpcomingAppointment(out appointmentInfo))
            {
                MessageBox.Show(appointmentInfo,
                    Resources.UpcomingAppointment,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            // Show main form
            Hide();
            var mainForm = new MainForm();
            mainForm.FormClosed += (s, args) => Close();
            mainForm.Show();
        }

        private void LogLogin(string username, bool success)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t{username}\t" +
                    $"{(success ? "Success" : "Failed")}\t{TimeZoneHelper.GetUserLocation()}\n";
                File.AppendAllText("Login_History.txt", logEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to log file: {ex.Message}",
                    Resources.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}