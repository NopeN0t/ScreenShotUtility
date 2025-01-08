﻿namespace SSU
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Key_box = new System.Windows.Forms.TextBox();
            this.Key_set = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Browse_box = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.Play_Sound = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.process_select = new System.Windows.Forms.Button();
            this.Process_box = new System.Windows.Forms.TextBox();
            this.Process_label = new System.Windows.Forms.Label();
            this.process_mode = new System.Windows.Forms.CheckBox();
            this.SC_Sample = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Index = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SC_cap = new System.Windows.Forms.ComboBox();
            this.SC_prefix = new System.Windows.Forms.TextBox();
            this.SC_name = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Key_box
            // 
            this.Key_box.Location = new System.Drawing.Point(166, 15);
            this.Key_box.Margin = new System.Windows.Forms.Padding(2);
            this.Key_box.Name = "Key_box";
            this.Key_box.ReadOnly = true;
            this.Key_box.Size = new System.Drawing.Size(186, 29);
            this.Key_box.TabIndex = 0;
            // 
            // Key_set
            // 
            this.Key_set.Location = new System.Drawing.Point(355, 15);
            this.Key_set.Margin = new System.Windows.Forms.Padding(2);
            this.Key_set.Name = "Key_set";
            this.Key_set.Size = new System.Drawing.Size(111, 30);
            this.Key_set.TabIndex = 1;
            this.Key_set.Text = "Set Key";
            this.Key_set.UseVisualStyleBackColor = true;
            this.Key_set.Click += new System.EventHandler(this.Key_set_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Take ScreenShot";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Save To";
            // 
            // Browse_box
            // 
            this.Browse_box.Location = new System.Drawing.Point(98, 49);
            this.Browse_box.Name = "Browse_box";
            this.Browse_box.Size = new System.Drawing.Size(254, 29);
            this.Browse_box.TabIndex = 2;
            this.Browse_box.TextChanged += new System.EventHandler(this.Browse_box_TextChanged);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(355, 49);
            this.Browse.Margin = new System.Windows.Forms.Padding(2);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(111, 30);
            this.Browse.TabIndex = 3;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_click);
            // 
            // Play_Sound
            // 
            this.Play_Sound.AutoSize = true;
            this.Play_Sound.Location = new System.Drawing.Point(6, 28);
            this.Play_Sound.Name = "Play_Sound";
            this.Play_Sound.Size = new System.Drawing.Size(125, 28);
            this.Play_Sound.TabIndex = 8;
            this.Play_Sound.Text = "Play Sound";
            this.Play_Sound.UseVisualStyleBackColor = true;
            this.Play_Sound.CheckedChanged += new System.EventHandler(this.Play_Sound_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.process_select);
            this.groupBox1.Controls.Add(this.Process_box);
            this.groupBox1.Controls.Add(this.Process_label);
            this.groupBox1.Controls.Add(this.process_mode);
            this.groupBox1.Controls.Add(this.Play_Sound);
            this.groupBox1.Location = new System.Drawing.Point(12, 231);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 103);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // process_select
            // 
            this.process_select.Enabled = false;
            this.process_select.Location = new System.Drawing.Point(317, 25);
            this.process_select.Margin = new System.Windows.Forms.Padding(2);
            this.process_select.Name = "process_select";
            this.process_select.Size = new System.Drawing.Size(111, 30);
            this.process_select.TabIndex = 11;
            this.process_select.Text = "Select";
            this.process_select.UseVisualStyleBackColor = true;
            this.process_select.Click += new System.EventHandler(this.process_select_Click);
            // 
            // Process_box
            // 
            this.Process_box.Enabled = false;
            this.Process_box.Location = new System.Drawing.Point(178, 60);
            this.Process_box.Name = "Process_box";
            this.Process_box.ReadOnly = true;
            this.Process_box.Size = new System.Drawing.Size(250, 29);
            this.Process_box.TabIndex = 12;
            // 
            // Process_label
            // 
            this.Process_label.AutoSize = true;
            this.Process_label.Enabled = false;
            this.Process_label.Location = new System.Drawing.Point(174, 28);
            this.Process_label.Name = "Process_label";
            this.Process_label.Size = new System.Drawing.Size(134, 24);
            this.Process_label.TabIndex = 7;
            this.Process_label.Text = "Process Name";
            // 
            // process_mode
            // 
            this.process_mode.AutoSize = true;
            this.process_mode.Location = new System.Drawing.Point(4, 62);
            this.process_mode.Name = "process_mode";
            this.process_mode.Size = new System.Drawing.Size(168, 28);
            this.process_mode.TabIndex = 9;
            this.process_mode.Text = "Specfic Program";
            this.process_mode.UseVisualStyleBackColor = true;
            this.process_mode.CheckedChanged += new System.EventHandler(this.process_mode_CheckedChanged);
            // 
            // SC_Sample
            // 
            this.SC_Sample.Location = new System.Drawing.Point(153, 84);
            this.SC_Sample.Name = "SC_Sample";
            this.SC_Sample.ReadOnly = true;
            this.SC_Sample.Size = new System.Drawing.Size(313, 29);
            this.SC_Sample.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Output Sample";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Index);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.SC_cap);
            this.groupBox2.Controls.Add(this.SC_prefix);
            this.groupBox2.Controls.Add(this.SC_name);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(12, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(454, 106);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // Index
            // 
            this.Index.AutoSize = true;
            this.Index.Location = new System.Drawing.Point(324, 36);
            this.Index.Name = "Index";
            this.Index.Size = new System.Drawing.Size(120, 48);
            this.Index.TabIndex = 12;
            this.Index.Text = "Image Saved\r\n000000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 24);
            this.label7.TabIndex = 11;
            this.label7.Text = "SC Cap";
            // 
            // SC_cap
            // 
            this.SC_cap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SC_cap.FormattingEnabled = true;
            this.SC_cap.Items.AddRange(new object[] {
            "9",
            "99",
            "999",
            "9999",
            "99999",
            "999999",
            "9999999",
            "99999999",
            "999999999",
            "9999999999",
            "99999999999",
            "999999999999",
            "9999999999999"});
            this.SC_cap.Location = new System.Drawing.Point(109, 57);
            this.SC_cap.Name = "SC_cap";
            this.SC_cap.Size = new System.Drawing.Size(209, 32);
            this.SC_cap.TabIndex = 7;
            this.SC_cap.SelectedIndexChanged += new System.EventHandler(this.SC_cap_SelectedIndexChanged);
            // 
            // SC_prefix
            // 
            this.SC_prefix.Location = new System.Drawing.Point(109, 22);
            this.SC_prefix.Name = "SC_prefix";
            this.SC_prefix.Size = new System.Drawing.Size(100, 29);
            this.SC_prefix.TabIndex = 5;
            this.SC_prefix.TextChanged += new System.EventHandler(this.SC_prefix_TextChanged);
            // 
            // SC_name
            // 
            this.SC_name.Location = new System.Drawing.Point(218, 22);
            this.SC_name.Name = "SC_name";
            this.SC_name.Size = new System.Drawing.Size(100, 29);
            this.SC_name.TabIndex = 6;
            this.SC_name.TextChanged += new System.EventHandler(this.SC_name_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 24);
            this.label6.TabIndex = 4;
            this.label6.Text = "File Name";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "SSU";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyicon_doubleclick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(486, 352);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.SC_Sample);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Browse_box);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.Key_set);
            this.Controls.Add(this.Key_box);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "ScreenShot Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_close);
            this.Resize += new System.EventHandler(this.From_resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Key_box;
        private System.Windows.Forms.Button Key_set;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Browse_box;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.CheckBox Play_Sound;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox SC_Sample;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox SC_cap;
        private System.Windows.Forms.TextBox SC_name;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label Index;
        private System.Windows.Forms.CheckBox process_mode;
        private System.Windows.Forms.Button process_select;
        private System.Windows.Forms.TextBox Process_box;
        private System.Windows.Forms.Label Process_label;
        private System.Windows.Forms.TextBox SC_prefix;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

