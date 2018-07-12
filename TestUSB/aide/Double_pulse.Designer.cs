namespace GeCoSwell.aide
{
    partial class Double_pulse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Double_pulse));
            this.pb_aide_dp = new System.Windows.Forms.PictureBox();
            this.l_apropos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_aide_dp)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_aide_dp
            // 
            this.pb_aide_dp.Image = global::GeCoSwell.Properties.Resources.aide_dp;
            this.pb_aide_dp.Location = new System.Drawing.Point(12, 12);
            this.pb_aide_dp.Name = "pb_aide_dp";
            this.pb_aide_dp.Size = new System.Drawing.Size(341, 195);
            this.pb_aide_dp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pb_aide_dp.TabIndex = 0;
            this.pb_aide_dp.TabStop = false;
            // 
            // l_apropos
            // 
            this.l_apropos.Location = new System.Drawing.Point(375, 12);
            this.l_apropos.Name = "l_apropos";
            this.l_apropos.Size = new System.Drawing.Size(352, 218);
            this.l_apropos.TabIndex = 23;
            this.l_apropos.Text = resources.GetString("l_apropos.Text");
            // 
            // Double_pulse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 249);
            this.Controls.Add(this.l_apropos);
            this.Controls.Add(this.pb_aide_dp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Double_pulse";
            this.Text = "Double pulse";
            ((System.ComponentModel.ISupportInitialize)(this.pb_aide_dp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_aide_dp;
        private System.Windows.Forms.Label l_apropos;
    }
}