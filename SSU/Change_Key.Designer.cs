namespace SSU
{
    partial class Change_Key
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
            this.label1 = new System.Windows.Forms.Label();
            this.key_box = new System.Windows.Forms.TextBox();
            this.win_key = new System.Windows.Forms.CheckBox();
            this.ctrl_key = new System.Windows.Forms.CheckBox();
            this.alt_key = new System.Windows.Forms.CheckBox();
            this.shift_key = new System.Windows.Forms.CheckBox();
            this.cancel = new System.Windows.Forms.Button();
            this.Done = new System.Windows.Forms.Button();
            this.Set = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Key";
            // 
            // key_box
            // 
            this.key_box.Location = new System.Drawing.Point(60, 6);
            this.key_box.MaxLength = 1;
            this.key_box.Name = "key_box";
            this.key_box.ReadOnly = true;
            this.key_box.Size = new System.Drawing.Size(80, 29);
            this.key_box.TabIndex = 0;
            // 
            // win_key
            // 
            this.win_key.AutoSize = true;
            this.win_key.Location = new System.Drawing.Point(16, 36);
            this.win_key.Name = "win_key";
            this.win_key.Size = new System.Drawing.Size(144, 28);
            this.win_key.TabIndex = 2;
            this.win_key.Text = "Windows Key";
            this.win_key.UseVisualStyleBackColor = true;
            // 
            // ctrl_key
            // 
            this.ctrl_key.AutoSize = true;
            this.ctrl_key.Location = new System.Drawing.Point(16, 70);
            this.ctrl_key.Name = "ctrl_key";
            this.ctrl_key.Size = new System.Drawing.Size(56, 28);
            this.ctrl_key.TabIndex = 4;
            this.ctrl_key.Text = "Ctrl";
            this.ctrl_key.UseVisualStyleBackColor = true;
            // 
            // alt_key
            // 
            this.alt_key.AutoSize = true;
            this.alt_key.Location = new System.Drawing.Point(166, 36);
            this.alt_key.Name = "alt_key";
            this.alt_key.Size = new System.Drawing.Size(50, 28);
            this.alt_key.TabIndex = 3;
            this.alt_key.Text = "Alt";
            this.alt_key.UseVisualStyleBackColor = true;
            // 
            // shift_key
            // 
            this.shift_key.AutoSize = true;
            this.shift_key.Location = new System.Drawing.Point(166, 70);
            this.shift_key.Name = "shift_key";
            this.shift_key.Size = new System.Drawing.Size(64, 28);
            this.shift_key.TabIndex = 5;
            this.shift_key.Text = "Shift";
            this.shift_key.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(12, 104);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(91, 31);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Done
            // 
            this.Done.Location = new System.Drawing.Point(109, 104);
            this.Done.Name = "Done";
            this.Done.Size = new System.Drawing.Size(91, 31);
            this.Done.TabIndex = 7;
            this.Done.Text = "Done";
            this.Done.UseVisualStyleBackColor = true;
            this.Done.Click += new System.EventHandler(this.Done_Click);
            // 
            // Set
            // 
            this.Set.Location = new System.Drawing.Point(146, 6);
            this.Set.Name = "Set";
            this.Set.Size = new System.Drawing.Size(91, 31);
            this.Set.TabIndex = 1;
            this.Set.Text = "Set";
            this.Set.UseVisualStyleBackColor = true;
            this.Set.Click += new System.EventHandler(this.Set_Click);
            // 
            // Change_Key
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(249, 152);
            this.Controls.Add(this.Set);
            this.Controls.Add(this.Done);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.shift_key);
            this.Controls.Add(this.alt_key);
            this.Controls.Add(this.ctrl_key);
            this.Controls.Add(this.win_key);
            this.Controls.Add(this.key_box);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Change_Key";
            this.ShowIcon = false;
            this.Text = "Change_Key";
            this.Load += new System.EventHandler(this.Change_Key_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Key_in);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox key_box;
        private System.Windows.Forms.CheckBox win_key;
        private System.Windows.Forms.CheckBox ctrl_key;
        private System.Windows.Forms.CheckBox alt_key;
        private System.Windows.Forms.CheckBox shift_key;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button Done;
        private System.Windows.Forms.Button Set;
    }
}