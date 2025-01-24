namespace SchedulingApp
{
    partial class CalendarView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button ViewToggleButton;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.appointmentGrid = new System.Windows.Forms.DataGridView();
            this.selectedDateLabel = new System.Windows.Forms.Label();
            this.ViewToggleButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // monthCalendar
            // 
            this.monthCalendar.Location = new System.Drawing.Point(18, 18);
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 0;
            this.monthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_DateChanged);
            // 
            // appointmentGrid
            // 
            this.appointmentGrid.AllowUserToAddRows = false;
            this.appointmentGrid.AllowUserToDeleteRows = false;
            this.appointmentGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appointmentGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.appointmentGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.appointmentGrid.Location = new System.Drawing.Point(18, 250);
            this.appointmentGrid.Name = "appointmentGrid";
            this.appointmentGrid.ReadOnly = true;
            this.appointmentGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.appointmentGrid.Size = new System.Drawing.Size(764, 288);
            this.appointmentGrid.TabIndex = 1;
            this.appointmentGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.appointmentGrid_CellDoubleClick);
            // 
            // selectedDateLabel
            // 
            this.selectedDateLabel.AutoSize = true;
            this.selectedDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedDateLabel.Location = new System.Drawing.Point(18, 220);
            this.selectedDateLabel.Name = "selectedDateLabel";
            this.selectedDateLabel.Size = new System.Drawing.Size(126, 20);
            this.selectedDateLabel.TabIndex = 2;
            this.selectedDateLabel.Text = "Selected Date: ";
            // 
            // ViewToggleButton
            // 
            this.ViewToggleButton.Location = new System.Drawing.Point(350, 190);
            this.ViewToggleButton.Name = "ViewToggleButton";
            this.ViewToggleButton.Size = new System.Drawing.Size(120, 25);
            this.ViewToggleButton.TabIndex = 3;
            this.ViewToggleButton.Text = "Toggle Day/Month";
            this.ViewToggleButton.UseVisualStyleBackColor = true;
            this.ViewToggleButton.Click += new System.EventHandler(this.ViewToggleButton_Click);
            // 
            // CalendarView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.Controls.Add(this.ViewToggleButton);
            this.Controls.Add(this.selectedDateLabel);
            this.Controls.Add(this.appointmentGrid);
            this.Controls.Add(this.monthCalendar);
            this.Name = "CalendarView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calendar View";
            ((System.ComponentModel.ISupportInitialize)(this.appointmentGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar;
        private System.Windows.Forms.DataGridView appointmentGrid;
        private System.Windows.Forms.Label selectedDateLabel;
    }
}