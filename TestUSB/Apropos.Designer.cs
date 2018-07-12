namespace GeCoSwell
{
    partial class Apropos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Apropos));
            this.pb_logo_laplace = new System.Windows.Forms.PictureBox();
            this.l_apropos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_logo_laplace)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_logo_laplace
            // 
            this.pb_logo_laplace.Image = global::GeCoSwell.Properties.Resources.logo_laplacegif;
            this.pb_logo_laplace.Location = new System.Drawing.Point(284, 12);
            this.pb_logo_laplace.Name = "pb_logo_laplace";
            this.pb_logo_laplace.Size = new System.Drawing.Size(258, 104);
            this.pb_logo_laplace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_logo_laplace.TabIndex = 21;
            this.pb_logo_laplace.TabStop = false;
            // 
            // l_apropos
            // 
            this.l_apropos.Location = new System.Drawing.Point(12, 9);
            this.l_apropos.Name = "l_apropos";
            this.l_apropos.Size = new System.Drawing.Size(266, 342);
            this.l_apropos.TabIndex = 22;
            this.l_apropos.Text = resources.GetString("l_apropos.Text");
            // 
            // Apropos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 378);
            this.Controls.Add(this.l_apropos);
            this.Controls.Add(this.pb_logo_laplace);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Apropos";
            this.Text = "À propos";
            ((System.ComponentModel.ISupportInitialize)(this.pb_logo_laplace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pb_logo_laplace;
        private System.Windows.Forms.Label l_apropos;
    }
}