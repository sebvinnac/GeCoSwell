using System.Windows.Forms;

namespace Gestion_Objet
{
    class CCheckBox : CheckBox
    {
        public CCheckBox(string name, int xpos,int ypos,bool check,string text, bool visible =true)
        {
            this.AutoSize = true;
            this.Location = new System.Drawing.Point(xpos, ypos);
            this.Name = name;
            this.Text = text;
            this.UseVisualStyleBackColor = true;
            this.Visible = visible;
        }
    }
}
