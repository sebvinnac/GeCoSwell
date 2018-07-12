namespace GeCoSwell
{
    partial class MesureOsci
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
            this.b_Mesure = new System.Windows.Forms.Button();
            this.b_save_point = new System.Windows.Forms.Button();
            this.b_stop = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // b_Mesure
            // 
            this.b_Mesure.Location = new System.Drawing.Point(12, 605);
            this.b_Mesure.Name = "b_Mesure";
            this.b_Mesure.Size = new System.Drawing.Size(75, 23);
            this.b_Mesure.TabIndex = 4;
            this.b_Mesure.Text = "Mesure";
            this.b_Mesure.UseVisualStyleBackColor = true;
            this.b_Mesure.Click += new System.EventHandler(this.B_Mesure_Click);
            // 
            // b_save_point
            // 
            this.b_save_point.Enabled = false;
            this.b_save_point.Location = new System.Drawing.Point(174, 605);
            this.b_save_point.Name = "b_save_point";
            this.b_save_point.Size = new System.Drawing.Size(75, 23);
            this.b_save_point.TabIndex = 6;
            this.b_save_point.Text = "Sauvegarder point";
            this.b_save_point.UseVisualStyleBackColor = true;
            this.b_save_point.Click += new System.EventHandler(this.B_save_point_Click);
            // 
            // b_stop
            // 
            this.b_stop.Location = new System.Drawing.Point(93, 605);
            this.b_stop.Name = "b_stop";
            this.b_stop.Size = new System.Drawing.Size(75, 23);
            this.b_stop.TabIndex = 8;
            this.b_stop.Text = "Stop";
            this.b_stop.UseVisualStyleBackColor = true;
            this.b_stop.Click += new System.EventHandler(this.B_stop_click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(416, 605);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MesureOsci
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 640);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.b_stop);
            this.Controls.Add(this.b_save_point);
            this.Controls.Add(this.b_Mesure);
            this.Name = "MesureOsci";
            this.Text = "Mesure";
            this.Load += new System.EventHandler(this.MesureOsci_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button b_Mesure;
        private System.Windows.Forms.Button b_save_point;
        private System.Windows.Forms.Button b_stop;
        private System.Windows.Forms.Button button1;
    }
}