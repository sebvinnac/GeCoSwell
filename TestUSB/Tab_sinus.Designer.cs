namespace GeCoSwell
{
    partial class Tab_sinus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tab_sinus));
            this.tb_val_max = new System.Windows.Forms.TextBox();
            this.tb_val_min = new System.Windows.Forms.TextBox();
            this.l_nb_val = new System.Windows.Forms.Label();
            this.l_val_max = new System.Windows.Forms.Label();
            this.l_val_min = new System.Windows.Forms.Label();
            this.l_explication = new System.Windows.Forms.Label();
            this.explication_min_max = new System.Windows.Forms.Label();
            this.b_generer = new System.Windows.Forms.Button();
            this.l_nb_bits = new System.Windows.Forms.Label();
            this.tb_nb_bits = new System.Windows.Forms.TextBox();
            this.cb_nb_val = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // tb_val_max
            // 
            this.tb_val_max.Location = new System.Drawing.Point(120, 89);
            this.tb_val_max.Name = "tb_val_max";
            this.tb_val_max.Size = new System.Drawing.Size(38, 20);
            this.tb_val_max.TabIndex = 1;
            this.tb_val_max.Text = "1023";
            this.tb_val_max.Validating += new System.ComponentModel.CancelEventHandler(this.Verif_textbox_TextChanged);
            // 
            // tb_val_min
            // 
            this.tb_val_min.Location = new System.Drawing.Point(120, 115);
            this.tb_val_min.Name = "tb_val_min";
            this.tb_val_min.Size = new System.Drawing.Size(38, 20);
            this.tb_val_min.TabIndex = 1;
            this.tb_val_min.Text = "0";
            this.tb_val_min.Validating += new System.ComponentModel.CancelEventHandler(this.Verif_textbox_TextChanged);
            // 
            // l_nb_val
            // 
            this.l_nb_val.AutoSize = true;
            this.l_nb_val.Location = new System.Drawing.Point(24, 25);
            this.l_nb_val.Name = "l_nb_val";
            this.l_nb_val.Size = new System.Drawing.Size(97, 13);
            this.l_nb_val.TabIndex = 2;
            this.l_nb_val.Text = "Nombre de valeur :";
            // 
            // l_val_max
            // 
            this.l_val_max.AutoSize = true;
            this.l_val_max.Location = new System.Drawing.Point(56, 92);
            this.l_val_max.Name = "l_val_max";
            this.l_val_max.Size = new System.Drawing.Size(65, 13);
            this.l_val_max.TabIndex = 3;
            this.l_val_max.Text = "Valeur max :";
            // 
            // l_val_min
            // 
            this.l_val_min.AutoSize = true;
            this.l_val_min.Location = new System.Drawing.Point(59, 118);
            this.l_val_min.Name = "l_val_min";
            this.l_val_min.Size = new System.Drawing.Size(62, 13);
            this.l_val_min.TabIndex = 3;
            this.l_val_min.Text = "Valeur min :";
            // 
            // l_explication
            // 
            this.l_explication.Location = new System.Drawing.Point(12, 216);
            this.l_explication.Name = "l_explication";
            this.l_explication.Size = new System.Drawing.Size(260, 37);
            this.l_explication.TabIndex = 4;
            this.l_explication.Text = "Permet de générer une table à utiliser avec Quartus.  Cela aura la forme d\'un fic" +
    "hier texte .";
            // 
            // explication_min_max
            // 
            this.explication_min_max.Location = new System.Drawing.Point(164, 80);
            this.explication_min_max.Name = "explication_min_max";
            this.explication_min_max.Size = new System.Drawing.Size(95, 66);
            this.explication_min_max.TabIndex = 5;
            this.explication_min_max.Text = "Valeur maximum et minimum que prendra le sinus. (note : max =        2^( nb bit) " +
    "-1 )";
            // 
            // b_generer
            // 
            this.b_generer.Location = new System.Drawing.Point(93, 170);
            this.b_generer.Name = "b_generer";
            this.b_generer.Size = new System.Drawing.Size(75, 23);
            this.b_generer.TabIndex = 6;
            this.b_generer.Text = "Générer";
            this.b_generer.UseVisualStyleBackColor = true;
            this.b_generer.Click += new System.EventHandler(this.B_generer_Click);
            // 
            // l_nb_bits
            // 
            this.l_nb_bits.AutoSize = true;
            this.l_nb_bits.Location = new System.Drawing.Point(42, 60);
            this.l_nb_bits.Name = "l_nb_bits";
            this.l_nb_bits.Size = new System.Drawing.Size(79, 13);
            this.l_nb_bits.TabIndex = 7;
            this.l_nb_bits.Text = "Nombre de bit :";
            // 
            // tb_nb_bits
            // 
            this.tb_nb_bits.Enabled = false;
            this.tb_nb_bits.Location = new System.Drawing.Point(120, 57);
            this.tb_nb_bits.Name = "tb_nb_bits";
            this.tb_nb_bits.Size = new System.Drawing.Size(38, 20);
            this.tb_nb_bits.TabIndex = 8;
            this.tb_nb_bits.Text = "10";
            this.tb_nb_bits.Validating += new System.ComponentModel.CancelEventHandler(this.Verif_textbox_TextChanged);
            // 
            // cb_nb_val
            // 
            this.cb_nb_val.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_nb_val.FormattingEnabled = true;
            this.cb_nb_val.Items.AddRange(new object[] {
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096",
            "8192",
            "16384",
            "32768",
            "65536"});
            this.cb_nb_val.Location = new System.Drawing.Point(120, 22);
            this.cb_nb_val.Name = "cb_nb_val";
            this.cb_nb_val.Size = new System.Drawing.Size(65, 21);
            this.cb_nb_val.TabIndex = 9;
            // 
            // Tab_sinus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.cb_nb_val);
            this.Controls.Add(this.tb_nb_bits);
            this.Controls.Add(this.l_nb_bits);
            this.Controls.Add(this.b_generer);
            this.Controls.Add(this.explication_min_max);
            this.Controls.Add(this.l_explication);
            this.Controls.Add(this.l_val_min);
            this.Controls.Add(this.l_val_max);
            this.Controls.Add(this.l_nb_val);
            this.Controls.Add(this.tb_val_min);
            this.Controls.Add(this.tb_val_max);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Tab_sinus";
            this.Text = "Génération table de sinus";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_val_max;
        private System.Windows.Forms.TextBox tb_val_min;
        private System.Windows.Forms.Label l_nb_val;
        private System.Windows.Forms.Label l_val_max;
        private System.Windows.Forms.Label l_val_min;
        private System.Windows.Forms.Label l_explication;
        private System.Windows.Forms.Label explication_min_max;
        private System.Windows.Forms.Button b_generer;
        private System.Windows.Forms.Label l_nb_bits;
        private System.Windows.Forms.TextBox tb_nb_bits;
        private System.Windows.Forms.ComboBox cb_nb_val;
    }
}