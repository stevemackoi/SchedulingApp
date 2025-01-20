using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace SchedulingApp
{
    public partial class CustomerForm : Form
    {
        private BindingList<Customer> customers;
        private Customer selectedCustomer;

        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                customers = new BindingList<Customer>();
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(@"
                        SELECT c.customerId, c.customerName, a.address, a.phone, c.active, c.addressId
                        FROM customer c
                        JOIN address a ON c.addressId = a.addressId", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new Customer
                                {
                                    CustomerId = reader.GetInt32("customerId"),
                                    CustomerName = reader.GetString("customerName"),
                                    Address = reader.GetString("address"),
                                    Phone = reader.GetString("phone"),
                                    Active = reader.GetBoolean("active"),
                                    AddressId = reader.GetInt32("addressId")
                                });
                            }
                        }
                    }
                }
                customerGridView.DataSource = customers;
                ConfigureGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}");
            }
        }

        private void ConfigureGridView()
        {
            customerGridView.AutoGenerateColumns = true;
            customerGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customerGridView.MultiSelect = false;
            customerGridView.ReadOnly = true;
            customerGridView.AllowUserToAddRows = false;
        }

        private void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
                throw new Exception("Name is required");

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
                throw new Exception("Address is required");

            if (!Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{3}-\d{4}$"))
                throw new Exception("Phone number must be in format: XXX-XXXX");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateInput();
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Add address first
                            string insertAddressSQL = @"
                                INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)
                                VALUES (@address, '', @cityId, @postalCode, @phone, NOW(), @createdBy, NOW() @lastUpdateBy)";

                            var addressCmd = new MySqlCommand(insertAddressSQL, conn);
                            addressCmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                            // addressCmd.Parameters.AddWithValue("@address2", "test");
                            addressCmd.Parameters.AddWithValue("@cityId", 1);
                            addressCmd.Parameters.AddWithValue("@postalCode", "00000");
                            addressCmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                            addressCmd.Parameters.AddWithValue("@createdBy", "test");
                            addressCmd.Parameters.AddWithValue("@lastUpdateBy", "test");

                            addressCmd.ExecuteNonQuery();

                            // Get the last inserted ID
                            var lastIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn);
                            int addressId = Convert.ToInt32(lastIdCmd.ExecuteScalar());

                            // Then add customer
                            string insertCustomerSQL = @"
                                INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                                VALUES (@customerName, @addressId, @active, NOW(), @createdBy, NOW(), @lastUpdaeBy)";

                            var customerCmd = new MySqlCommand(insertCustomerSQL, conn);
                            customerCmd.Parameters.AddWithValue("@customerName", txtName.Text.Trim());
                            customerCmd.Parameters.AddWithValue("@addressId", addressId);
                            customerCmd.Parameters.AddWithValue("@active", true);
                            customerCmd.Parameters.AddWithValue("@createdBy", "test");
                            customerCmd.Parameters.AddWithValue("@lastUpdateBy", "test");

                            customerCmd.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Customer added successfully!");
                            LoadCustomers();
                            ClearFields();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedCustomer == null)
                {
                    MessageBox.Show("Please select a customer to update");
                    return;
                }

                ValidateInput();
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update address
                            var addressCmd = new MySqlCommand(@"
                                UPDATE address 
                                SET address = @address, 
                                    phone = @phone,
                                    lastUpdate = NOW(),
                                    lastUpdateBy = @user
                                WHERE addressId = @addressId", conn);

                            addressCmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                            addressCmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                            addressCmd.Parameters.AddWithValue("@addressId", selectedCustomer.AddressId);
                            addressCmd.ExecuteNonQuery();

                            // Update customer
                            var customerCmd = new MySqlCommand(@"
                                UPDATE customer 
                                SET customerName = @name,
                                    lastUpdate = NOW(),
                                    lastUpdateBy = @user
                                WHERE customerId = @customerId", conn);

                            customerCmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            customerCmd.Parameters.AddWithValue("@customerId", selectedCustomer.CustomerId);
                            customerCmd.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Customer updated successfully!");
                            LoadCustomers();
                            ClearFields();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedCustomer == null)
                {
                    MessageBox.Show("Please select a customer to delete");
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (var conn = DBConnection.GetConnection())
                    {
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                // First delete customer
                                var customerCmd = new MySqlCommand(
                                    "DELETE FROM customer WHERE customerId = @customerId", conn);
                                customerCmd.Parameters.AddWithValue("@customerId", selectedCustomer.CustomerId);
                                customerCmd.ExecuteNonQuery();

                                // Then delete address
                                var addressCmd = new MySqlCommand(
                                    "DELETE FROM address WHERE addressId = @addressId", conn);
                                addressCmd.Parameters.AddWithValue("@addressId", selectedCustomer.AddressId);
                                addressCmd.ExecuteNonQuery();

                                transaction.Commit();
                                MessageBox.Show("Customer deleted successfully!");
                                LoadCustomers();
                                ClearFields();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}");
            }
        }

        private void customerGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (customerGridView.SelectedRows.Count > 0)
            {
                selectedCustomer = (Customer)customerGridView.SelectedRows[0].DataBoundItem;
                txtName.Text = selectedCustomer.CustomerName;
                txtAddress.Text = selectedCustomer.Address;
                txtPhone.Text = selectedCustomer.Phone;
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            selectedCustomer = null;
        }
    }
}
