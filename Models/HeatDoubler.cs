using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfRibbonApplication1.Models
{
    public class HeatDoubler
    {
        public string Name {set; get;}
        public double X, Y, Z;
        public string Xstr {set; get;}
        public string Ystr {set; get;}
        public string Zstr {set; get;}

        public HeatDoubler() { }
        public HeatDoubler(string Name, double X, double Y, double Z)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Xstr = X.ToString();
            this.Ystr = Y.ToString();
            this.Zstr = Z.ToString();
        }
    }
}
