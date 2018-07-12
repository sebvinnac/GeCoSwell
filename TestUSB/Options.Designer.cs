namespace GeCoSwell
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.gB_quartus_stpexe = new System.Windows.Forms.GroupBox();
            this.l_info_quartus = new System.Windows.Forms.Label();
            this.b_Chercherquartus_stp = new System.Windows.Forms.Button();
            this.tb_quartus_stpexe = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.gB_TCL = new System.Windows.Forms.GroupBox();
            this.l_info_TCL = new System.Windows.Forms.Label();
            this.b_ChercherTCL = new System.Windows.Forms.Button();
            this.tb_TCL = new System.Windows.Forms.TextBox();
            this.chb_AutoLoad = new System.Windows.Forms.CheckBox();
            this.b_param_ok = new System.Windows.Forms.Button();
            this.b_param_annuler = new System.Windows.Forms.Button();
            this.chb_ModeDaltonien = new System.Windows.Forms.CheckBox();
            this.chb_valid_expert = new System.Windows.Forms.CheckBox();
            this.gB_quartus_stpexe.SuspendLayout();
            this.gB_TCL.SuspendLayout();
            this.SuspendLayout();
            // 
            // gB_quartus_stpexe
            // 
            this.gB_quartus_stpexe.Controls.Add(this.l_info_quartus);
            this.gB_quartus_stpexe.Controls.Add(this.b_Chercherquartus_stp);
            this.gB_quartus_stpexe.Controls.Add(this.tb_quartus_stpexe);
            this.gB_quartus_stpexe.Location = new System.Drawing.Point(12, 29);
            this.gB_quartus_stpexe.Name = "gB_quartus_stpexe";
            this.gB_quartus_stpexe.Size = new System.Drawing.Size(429, 88);
            this.gB_quartus_stpexe.TabIndex = 0;
            this.gB_quartus_stpexe.TabStop = false;
            this.gB_quartus_stpexe.Text = "Emplacement de Quartus_stp.exe";
            // 
            // l_info_quartus
            // 
            this.l_info_quartus.AutoSize = true;
            this.l_info_quartus.Location = new System.Drawing.Point(12, 53);
            this.l_info_quartus.Name = "l_info_quartus";
            this.l_info_quartus.Size = new System.Drawing.Size(92, 13);
            this.l_info_quartus.TabIndex = 2;
            this.l_info_quartus.Text = "Fichier non trouvé";
            // 
            // b_Chercherquartus_stp
            // 
            this.b_Chercherquartus_stp.Location = new System.Drawing.Point(336, 59);
            this.b_Chercherquartus_stp.Name = "b_Chercherquartus_stp";
            this.b_Chercherquartus_stp.Size = new System.Drawing.Size(87, 23);
            this.b_Chercherquartus_stp.TabIndex = 1;
            this.b_Chercherquartus_stp.Text = "Chercher";
            this.b_Chercherquartus_stp.UseVisualStyleBackColor = true;
            this.b_Chercherquartus_stp.Click += new System.EventHandler(this.B_Chercherquartus_stp_Click);
            // 
            // tb_quartus_stpexe
            // 
            this.tb_quartus_stpexe.Location = new System.Drawing.Point(12, 30);
            this.tb_quartus_stpexe.Name = "tb_quartus_stpexe";
            this.tb_quartus_stpexe.Size = new System.Drawing.Size(411, 20);
            this.tb_quartus_stpexe.TabIndex = 0;
            this.tb_quartus_stpexe.Validating += new System.ComponentModel.CancelEventHandler(this.Tb_fileexist_Validating);
            // 
            // gB_TCL
            // 
            this.gB_TCL.Controls.Add(this.l_info_TCL);
            this.gB_TCL.Controls.Add(this.b_ChercherTCL);
            this.gB_TCL.Controls.Add(this.tb_TCL);
            this.gB_TCL.Location = new System.Drawing.Point(12, 123);
            this.gB_TCL.Name = "gB_TCL";
            this.gB_TCL.Size = new System.Drawing.Size(429, 88);
            this.gB_TCL.TabIndex = 1;
            this.gB_TCL.TabStop = false;
            this.gB_TCL.Text = "Emplacement du script TCL";
            // 
            // l_info_TCL
            // 
            this.l_info_TCL.AutoSize = true;
            this.l_info_TCL.Location = new System.Drawing.Point(12, 53);
            this.l_info_TCL.Name = "l_info_TCL";
            this.l_info_TCL.Size = new System.Drawing.Size(92, 13);
            this.l_info_TCL.TabIndex = 3;
            this.l_info_TCL.Text = "Fichier non trouvé";
            // 
            // b_ChercherTCL
            // 
            this.b_ChercherTCL.Location = new System.Drawing.Point(336, 59);
            this.b_ChercherTCL.Name = "b_ChercherTCL";
            this.b_ChercherTCL.Size = new System.Drawing.Size(87, 23);
            this.b_ChercherTCL.TabIndex = 1;
            this.b_ChercherTCL.Text = "Chercher";
            this.b_ChercherTCL.UseVisualStyleBackColor = true;
            this.b_ChercherTCL.Click += new System.EventHandler(this.B_ChercherTCL_Click);
            // 
            // tb_TCL
            // 
            this.tb_TCL.Location = new System.Drawing.Point(12, 30);
            this.tb_TCL.Name = "tb_TCL";
            this.tb_TCL.Size = new System.Drawing.Size(411, 20);
            this.tb_TCL.TabIndex = 0;
            this.tb_TCL.Validating += new System.ComponentModel.CancelEventHandler(this.Tb_fileexist_Validating);
            // 
            // chb_AutoLoad
            // 
            this.chb_AutoLoad.AutoSize = true;
            this.chb_AutoLoad.Location = new System.Drawing.Point(12, 217);
            this.chb_AutoLoad.Name = "chb_AutoLoad";
            this.chb_AutoLoad.Size = new System.Drawing.Size(239, 17);
            this.chb_AutoLoad.TabIndex = 2;
            this.chb_AutoLoad.Text = "Charger les derniers paramètres au démarage";
            this.chb_AutoLoad.UseVisualStyleBackColor = true;
            // 
            // b_param_ok
            // 
            this.b_param_ok.Location = new System.Drawing.Point(285, 261);
            this.b_param_ok.Name = "b_param_ok";
            this.b_param_ok.Size = new System.Drawing.Size(75, 23);
            this.b_param_ok.TabIndex = 3;
            this.b_param_ok.Text = "Ok";
            this.b_param_ok.UseVisualStyleBackColor = true;
            this.b_param_ok.Click += new System.EventHandler(this.B_param_ok_Click);
            // 
            // b_param_annuler
            // 
            this.b_param_annuler.Location = new System.Drawing.Point(366, 261);
            this.b_param_annuler.Name = "b_param_annuler";
            this.b_param_annuler.Size = new System.Drawing.Size(75, 23);
            this.b_param_annuler.TabIndex = 4;
            this.b_param_annuler.Text = "Annuler";
            this.b_param_annuler.UseVisualStyleBackColor = true;
            this.b_param_annuler.Click += new System.EventHandler(this.B_param_annuler_Click);
            // 
            // chb_ModeDaltonien
            // 
            this.chb_ModeDaltonien.AutoSize = true;
            this.chb_ModeDaltonien.Location = new System.Drawing.Point(12, 240);
            this.chb_ModeDaltonien.Name = "chb_ModeDaltonien";
            this.chb_ModeDaltonien.Size = new System.Drawing.Size(99, 17);
            this.chb_ModeDaltonien.TabIndex = 5;
            this.chb_ModeDaltonien.Text = "Mode daltonien";
            this.chb_ModeDaltonien.UseVisualStyleBackColor = true;
            // 
            // chb_valid_expert
            // 
            this.chb_valid_expert.AutoSize = true;
            this.chb_valid_expert.ForeColor = System.Drawing.Color.Red;
            this.chb_valid_expert.Location = new System.Drawing.Point(12, 261);
            this.chb_valid_expert.Name = "chb_valid_expert";
            this.chb_valid_expert.Size = new System.Drawing.Size(141, 17);
            this.chb_valid_expert.TabIndex = 5;
            this.chb_valid_expert.Text = "Autorisé mode expert /!\\";
            this.chb_valid_expert.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 296);
            this.Controls.Add(this.chb_valid_expert);
            this.Controls.Add(this.chb_ModeDaltonien);
            this.Controls.Add(this.b_param_annuler);
            this.Controls.Add(this.b_param_ok);
            this.Controls.Add(this.chb_AutoLoad);
            this.Controls.Add(this.gB_TCL);
            this.Controls.Add(this.gB_quartus_stpexe);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Options";
            this.Text = "Options";
            this.gB_quartus_stpexe.ResumeLayout(false);
            this.gB_quartus_stpexe.PerformLayout();
            this.gB_TCL.ResumeLayout(false);
            this.gB_TCL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gB_quartus_stpexe;
        private System.Windows.Forms.Button b_Chercherquartus_stp;
        private System.Windows.Forms.TextBox tb_quartus_stpexe;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox gB_TCL;
        private System.Windows.Forms.Button b_ChercherTCL;
        private System.Windows.Forms.TextBox tb_TCL;
        private System.Windows.Forms.CheckBox chb_AutoLoad;
        private System.Windows.Forms.Button b_param_ok;
        private System.Windows.Forms.Button b_param_annuler;
        private System.Windows.Forms.Label l_info_quartus;
        private System.Windows.Forms.Label l_info_TCL;
        private System.Windows.Forms.CheckBox chb_ModeDaltonien;
        private System.Windows.Forms.CheckBox chb_valid_expert;
    }
}