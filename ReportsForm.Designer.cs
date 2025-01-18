namespace SchedulingApp
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTypesByMonth;
        private System.Windows.Forms.TabPage tabUserSchedules;
        private System.Windows.Forms.TabPage tabCustomerCounts;
        private System.Windows.Forms.TextBox txtTypesByMonth;
        private System.Windows.Forms.TextBox txtUserSchedules;
        private System.Windows.Forms.TextBox txtCustomerCounts;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTypesByMonth = new System.Windows.Forms.TabPage();
            this.tabUserSchedules = new System.Windows.Forms.TabPage();
            this.tabCustomerCounts = new System.Windows.Forms.TabPage();
            this.txtTypesByMonth = new System.Windows.Forms.TextBox();
            this.txtUserSchedules = new System.Windows.Forms.TextBox();
            this.txtCustomerCounts = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabTypesByMonth.SuspendLayout();
            this.tabUserSchedules.SuspendLayout();
            this.tabCustomerCounts.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTypesByMonth);
            this.tabControl1.Controls.Add(this.tabUserSchedules);
            this.tabControl1.Controls.Add(this.tabCustomerCounts);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(584, 461);
            this.tabControl1.TabIndex = 0;
            // 
            // tabTypesByMonth
            // 
            this.tabTypesByMonth.Controls.Add(this.txtTypesByMonth);
            this.tabTypesByMonth.Location = new System.Drawing.Point(4, 22);
            this.tabTypesByMonth.Name = "tabTypesByMonth";
            this.tabTypesByMonth.Padding = new System.Windows.Forms.Padding(3);
            this.tabTypesByMonth.Size = new System.Drawing.Size(576, 435);
            this.tabTypesByMonth.TabIndex = 0;
            this.tabTypesByMonth.Text = "Appointment Types by Month";
            this.tabTypesByMonth.UseVisualStyleBackColor = true;
            // 
            // tabUserSchedules
            // 
            this.tabUserSchedules.Controls.Add(this.txtUserSchedules);
            this.tabUserSchedules.Location = new System.Drawing.Point(4, 22);
            this.tabUserSchedules.Name = "tabUserSchedules";
            this.tabUserSchedules.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserSchedules.Size = new System.Drawing.Size(576, 435);
            this.tabUserSchedules.TabIndex = 1;
            this.tabUserSchedules.Text = "User Schedules";
            this.tabUserSchedules.UseVisualStyleBackColor = true;
            // 
            // tabCustomerCounts
            // 
            this.tabCustomerCounts.Controls.Add(this.txtCustomerCounts);
            this.tabCustomerCounts.Location = new System.Drawing.Point(4, 22);
            this.tabCustomerCounts.Name = "tabCustomerCounts";
            this.tabCustomerCounts.Padding = new System.Windows.Forms.Padding(3);
            this.tabCustomerCounts.Size = new System.Drawing.Size(576, 435);
            this.tabCustomerCounts.TabIndex = 2;
            this.tabCustomerCounts.Text = "Customer Appointment Counts";
            this.tabCustomerCounts.UseVisualStyleBackColor = true;
            // 
            // txtTypesByMonth
            // 
            this.txtTypesByMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTypesByMonth.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTypesByMonth.Location = new System.Drawing.Point(3, 3);
            this.txtTypesByMonth.Multiline = true;
            this.txtTypesByMonth.Name = "txtTypesByMonth";
            this.txtTypesByMonth.ReadOnly = true;
            this.txtTypesByMonth.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTypesByMonth.Size = new System.Drawing.Size(570, 429);
            this.txtTypesByMonth.TabIndex = 0;
            // 
            // txtUserSchedules
            // 
            this.txtUserSchedules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserSchedules.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserSchedules.Location = new System.Drawing.Point(3, 3);
            this.txtUserSchedules.Multiline = true;
            this.txtUserSchedules.Name = "txtUserSchedules";
            this.txtUserSchedules.ReadOnly = true;
            this.txtUserSchedules.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUserSchedules.Size = new System.Drawing.Size(570, 429);
            this.txtUserSchedules.TabIndex = 0;
            // 
            // txtCustomerCounts
            // 
            this.txtCustomerCounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCustomerCounts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerCounts.Location = new System.Drawing.Point(3, 3);
            this.txtCustomerCounts.Multiline = true;
            this.txtCustomerCounts.Name = "txtCustomerCounts";
            this.txtCustomerCounts.ReadOnly = true;
            this.txtCustomerCounts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCustomerCounts.Size = new System.Drawing.Size(570, 429);
            this.txtCustomerCounts.TabIndex = 0;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.tabControl1);
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reports";
            this.tabControl1.ResumeLayout(false);
            this.tabTypesByMonth.ResumeLayout(false);
            this.tabTypesByMonth.PerformLayout();
            this.tabUserSchedules.ResumeLayout(false);
            this.tabUserSchedules.PerformLayout();
            this.tabCustomerCounts.ResumeLayout(false);
            this.tabCustomerCounts.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
