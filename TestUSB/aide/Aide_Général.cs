using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeCoSwell.aide
{
    public partial class Aide_Général : Form
    {
        public GroupBox Gb_aide;
        private Consigne consigne;

        public Aide_Général(GroupBox gb)
        {

            InitializeComponent();
            GroupBox Gb_aide = (GroupBox)CloneObject(gb);
            this.Controls.Add(Gb_aide);
        }
        

        private object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();


            Object p = t.InvokeMember("", System.Reflection.
                BindingFlags.CreateInstance, null, o, null);


            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(p, pi.GetValue(o, null), null);
                }
            }
            return p;
        }
    }
}
