namespace GeCoSwell.aide
{
    partial class Etalon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Etalon));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.l_apropos = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GeCoSwell.Properties.Resources.aide_etalon_can;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(299, 161);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // l_apropos
            // 
            this.l_apropos.Location = new System.Drawing.Point(327, 12);
            this.l_apropos.Name = "l_apropos";
            this.l_apropos.Size = new System.Drawing.Size(352, 34);
            this.l_apropos.TabIndex = 24;
            this.l_apropos.Text = "Étalon\r\npermet d\'étaloner les CANs\r\n";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 114);
            this.label1.TabIndex = 25;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(348, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(352, 114);
            this.label2.TabIndex = 25;
            this.label2.Text = "10bits\r\nvaleur max : la valeur max doit représenter cette valeur sur un 10bits\r\n(" +
    "en général 1023)\r\n\r\nvaleur min : la valeur min doit représenter cette valeur sur" +
    " un 10bits\r\n(en général 0)";
            // 
            // Etalon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 354);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.l_apropos);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Etalon";
            this.Text = "Étalon";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label l_apropos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}