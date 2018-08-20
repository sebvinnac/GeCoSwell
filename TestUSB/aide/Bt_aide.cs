using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeCoSwell
{
    class Bt_aide : Button
    {
        public Bt_aide(int xpos, int ypos)
        {
            this.Image = global::GeCoSwell.Properties.Resources.icons8_information_16;
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Size = new System.Drawing.Size(20, 20);
            this.UseVisualStyleBackColor = true;
            //this.aide_frequence.Click += new System.EventHandler(this.Bt_aide2_Click);

        }
    }
}
