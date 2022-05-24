namespace UnattachedRebarApp___DataGridView
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Display = new System.Windows.Forms.Button();
            this.Select_in_Model = new System.Windows.Forms.Button();
            this.Create_separate_for_all_owners = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Create_for_selected = new System.Windows.Forms.Button();
            this.One_report_for_all_owners = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightGreen;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(515, 394);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            // 
            // Display
            // 
            this.Display.Location = new System.Drawing.Point(12, 415);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(75, 23);
            this.Display.TabIndex = 1;
            this.Display.Text = "Display";
            this.Display.UseVisualStyleBackColor = true;
            this.Display.Click += new System.EventHandler(this.Display_Click);
            // 
            // Select_in_Model
            // 
            this.Select_in_Model.Location = new System.Drawing.Point(93, 415);
            this.Select_in_Model.Name = "Select_in_Model";
            this.Select_in_Model.Size = new System.Drawing.Size(95, 23);
            this.Select_in_Model.TabIndex = 3;
            this.Select_in_Model.Text = "Select in Model";
            this.Select_in_Model.UseVisualStyleBackColor = true;
            this.Select_in_Model.Click += new System.EventHandler(this.Select_in_Model_Click);
            // 
            // Create_separate_for_all_owners
            // 
            this.Create_separate_for_all_owners.Location = new System.Drawing.Point(735, 86);
            this.Create_separate_for_all_owners.Name = "Create_separate_for_all_owners";
            this.Create_separate_for_all_owners.Size = new System.Drawing.Size(136, 49);
            this.Create_separate_for_all_owners.TabIndex = 4;
            this.Create_separate_for_all_owners.Text = "Create seperate for each owner";
            this.Create_separate_for_all_owners.UseVisualStyleBackColor = true;
            this.Create_separate_for_all_owners.Click += new System.EventHandler(this.Create_separate_for_all_owners_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(533, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(196, 394);
            this.listBox1.TabIndex = 5;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // Create_for_selected
            // 
            this.Create_for_selected.Location = new System.Drawing.Point(735, 141);
            this.Create_for_selected.Name = "Create_for_selected";
            this.Create_for_selected.Size = new System.Drawing.Size(136, 49);
            this.Create_for_selected.TabIndex = 6;
            this.Create_for_selected.Text = "Create for selected owners";
            this.Create_for_selected.UseVisualStyleBackColor = true;
            this.Create_for_selected.Click += new System.EventHandler(this.Create_for_selected_Click);
            // 
            // One_report_for_all_owners
            // 
            this.One_report_for_all_owners.Location = new System.Drawing.Point(735, 31);
            this.One_report_for_all_owners.Name = "One_report_for_all_owners";
            this.One_report_for_all_owners.Size = new System.Drawing.Size(136, 49);
            this.One_report_for_all_owners.TabIndex = 7;
            this.One_report_for_all_owners.Text = "Create one for all owners";
            this.One_report_for_all_owners.UseVisualStyleBackColor = true;
            this.One_report_for_all_owners.Click += new System.EventHandler(this.One_report_for_all_owners_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(735, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "REPORTS:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 454);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.One_report_for_all_owners);
            this.Controls.Add(this.Create_for_selected);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Create_separate_for_all_owners);
            this.Controls.Add(this.Select_in_Model);
            this.Controls.Add(this.Display);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "UnattachedRebarApp";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button Display;
        private System.Windows.Forms.Button Select_in_Model;
        private System.Windows.Forms.Button Create_separate_for_all_owners;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Create_for_selected;
        private System.Windows.Forms.Button One_report_for_all_owners;
        private System.Windows.Forms.Label label1;
    }
}

