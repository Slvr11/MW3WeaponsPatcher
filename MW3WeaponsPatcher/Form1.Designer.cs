namespace MW3WeaponsPatcher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mw3Status = new System.Windows.Forms.Label();
            this.ak47PatchButton = new System.Windows.Forms.Button();
            this.deaglePatchButton = new System.Windows.Forms.Button();
            this.deagleInfo = new System.Windows.Forms.Label();
            this.ak47Camo = new System.Windows.Forms.CheckBox();
            this.ak47CamoTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.m16PatchButton = new System.Windows.Forms.Button();
            this.uspPatchButton = new System.Windows.Forms.Button();
            this.f2000PatchButton = new System.Windows.Forms.Button();
            this.m16Camo = new System.Windows.Forms.CheckBox();
            this.m4PatchButton = new System.Windows.Forms.Button();
            this.m16CamoTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.augPatchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mw3Status
            // 
            this.mw3Status.AutoSize = true;
            this.mw3Status.BackColor = System.Drawing.Color.Transparent;
            this.mw3Status.ForeColor = System.Drawing.Color.Red;
            this.mw3Status.Location = new System.Drawing.Point(99, 9);
            this.mw3Status.Name = "mw3Status";
            this.mw3Status.Size = new System.Drawing.Size(88, 13);
            this.mw3Status.TabIndex = 0;
            this.mw3Status.Text = "MW3 is not open";
            this.mw3Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ak47PatchButton
            // 
            this.ak47PatchButton.Enabled = false;
            this.ak47PatchButton.Location = new System.Drawing.Point(12, 59);
            this.ak47PatchButton.Name = "ak47PatchButton";
            this.ak47PatchButton.Size = new System.Drawing.Size(134, 23);
            this.ak47PatchButton.TabIndex = 1;
            this.ak47PatchButton.Text = "Patch Classic AK-47";
            this.ak47PatchButton.UseVisualStyleBackColor = true;
            this.ak47PatchButton.Click += new System.EventHandler(this.ak47PatchButton_Click);
            // 
            // deaglePatchButton
            // 
            this.deaglePatchButton.Enabled = false;
            this.deaglePatchButton.Location = new System.Drawing.Point(12, 207);
            this.deaglePatchButton.Name = "deaglePatchButton";
            this.deaglePatchButton.Size = new System.Drawing.Size(146, 23);
            this.deaglePatchButton.TabIndex = 2;
            this.deaglePatchButton.Text = "Patch Classic Desert Eagle";
            this.deaglePatchButton.UseVisualStyleBackColor = true;
            this.deaglePatchButton.Visible = false;
            this.deaglePatchButton.Click += new System.EventHandler(this.deaglePatchButton_Click);
            // 
            // deagleInfo
            // 
            this.deagleInfo.AutoSize = true;
            this.deagleInfo.Location = new System.Drawing.Point(164, 207);
            this.deagleInfo.Name = "deagleInfo";
            this.deagleInfo.Size = new System.Drawing.Size(88, 39);
            this.deagleInfo.TabIndex = 3;
            this.deagleInfo.Text = "*Only works on \r\nLockdown, all\r\nother maps crash";
            this.deagleInfo.Visible = false;
            // 
            // ak47Camo
            // 
            this.ak47Camo.AutoSize = true;
            this.ak47Camo.Location = new System.Drawing.Point(152, 63);
            this.ak47Camo.Name = "ak47Camo";
            this.ak47Camo.Size = new System.Drawing.Size(132, 17);
            this.ak47Camo.TabIndex = 4;
            this.ak47Camo.Text = "Replace Classic Camo";
            this.ak47Camo.UseVisualStyleBackColor = true;
            // 
            // m16PatchButton
            // 
            this.m16PatchButton.Enabled = false;
            this.m16PatchButton.Location = new System.Drawing.Point(12, 88);
            this.m16PatchButton.Name = "m16PatchButton";
            this.m16PatchButton.Size = new System.Drawing.Size(133, 23);
            this.m16PatchButton.TabIndex = 5;
            this.m16PatchButton.Text = "Patch MW2 M16";
            this.m16PatchButton.UseVisualStyleBackColor = true;
            this.m16PatchButton.Click += new System.EventHandler(this.m16PatchButton_Click);
            // 
            // uspPatchButton
            // 
            this.uspPatchButton.Enabled = false;
            this.uspPatchButton.Location = new System.Drawing.Point(12, 178);
            this.uspPatchButton.Name = "uspPatchButton";
            this.uspPatchButton.Size = new System.Drawing.Size(133, 23);
            this.uspPatchButton.TabIndex = 6;
            this.uspPatchButton.Text = "Patch Classic USP .45";
            this.uspPatchButton.UseVisualStyleBackColor = true;
            this.uspPatchButton.Click += new System.EventHandler(this.uspPatchButton_Click);
            // 
            // f2000PatchButton
            // 
            this.f2000PatchButton.Enabled = false;
            this.f2000PatchButton.Location = new System.Drawing.Point(12, 147);
            this.f2000PatchButton.Name = "f2000PatchButton";
            this.f2000PatchButton.Size = new System.Drawing.Size(132, 23);
            this.f2000PatchButton.TabIndex = 7;
            this.f2000PatchButton.Text = "Patch F2000 (as MP5)";
            this.f2000PatchButton.UseVisualStyleBackColor = true;
            this.f2000PatchButton.Click += new System.EventHandler(this.f2000PatchButton_Click);
            // 
            // m16Camo
            // 
            this.m16Camo.AutoSize = true;
            this.m16Camo.Location = new System.Drawing.Point(152, 92);
            this.m16Camo.Name = "m16Camo";
            this.m16Camo.Size = new System.Drawing.Size(132, 17);
            this.m16Camo.TabIndex = 8;
            this.m16Camo.Text = "Replace Classic Camo";
            this.m16Camo.UseVisualStyleBackColor = true;
            // 
            // m4PatchButton
            // 
            this.m4PatchButton.Enabled = false;
            this.m4PatchButton.Location = new System.Drawing.Point(12, 30);
            this.m4PatchButton.Name = "m4PatchButton";
            this.m4PatchButton.Size = new System.Drawing.Size(133, 23);
            this.m4PatchButton.TabIndex = 9;
            this.m4PatchButton.Text = "Remove M4A1 Grip";
            this.m4PatchButton.UseVisualStyleBackColor = true;
            this.m4PatchButton.Click += new System.EventHandler(this.m4PatchButton_Click);
            // 
            // augPatchButton
            // 
            this.augPatchButton.Enabled = false;
            this.augPatchButton.Location = new System.Drawing.Point(13, 117);
            this.augPatchButton.Name = "augPatchButton";
            this.augPatchButton.Size = new System.Drawing.Size(133, 23);
            this.augPatchButton.TabIndex = 10;
            this.augPatchButton.Text = "Patch AUG (as L86)";
            this.augPatchButton.UseVisualStyleBackColor = true;
            this.augPatchButton.Click += new System.EventHandler(this.augPatchButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.augPatchButton);
            this.Controls.Add(this.m4PatchButton);
            this.Controls.Add(this.m16Camo);
            this.Controls.Add(this.f2000PatchButton);
            this.Controls.Add(this.uspPatchButton);
            this.Controls.Add(this.m16PatchButton);
            this.Controls.Add(this.ak47Camo);
            this.Controls.Add(this.deagleInfo);
            this.Controls.Add(this.deaglePatchButton);
            this.Controls.Add(this.ak47PatchButton);
            this.Controls.Add(this.mw3Status);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "TeknoMW3 Weapons Patcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mw3Status;
        private System.Windows.Forms.Button ak47PatchButton;
        private System.Windows.Forms.Button deaglePatchButton;
        private System.Windows.Forms.Label deagleInfo;
        private System.Windows.Forms.CheckBox ak47Camo;
        private System.Windows.Forms.ToolTip ak47CamoTooltip;
        private System.Windows.Forms.Button m16PatchButton;
        private System.Windows.Forms.Button uspPatchButton;
        private System.Windows.Forms.Button f2000PatchButton;
        private System.Windows.Forms.CheckBox m16Camo;
        private System.Windows.Forms.Button m4PatchButton;
        private System.Windows.Forms.ToolTip m16CamoTooltip;
        private System.Windows.Forms.Button augPatchButton;
    }
}

