using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarmuKolcsonzo.ViewModels
{
    public class rendelesVM
    {
        public int rendelesId { get; set; }
        public int ugyfelId { get; set; }
        public string ugyfelNev { get; set; }
        public string ugyfelTelefonszam { get; set; }
        public string ugyfelEmail { get; set; }
        public int ugyfelPont { get; set; }
        public int jarmuId { get; set; }
        public string jarmuRendszam { get; set; }
        public int? jarmuFerohely { get; set; }
        public DateTime rendelesDatum { get; set; }

        public rendelesVM(
            int rendelesId, 
            int ugyfelId, 
            string ugyfelNev, 
            string ugyfelTelefonszam, 
            string ugyfelEmail, 
            int ugyfelPont, 
            int jarmuId, 
            string jarmuRendszam, 
            int? jarmuFerohely, 
            DateTime rendelesDatum
            )
        {
            this.rendelesId = rendelesId;
            this.ugyfelId = ugyfelId;
            this.ugyfelNev = ugyfelNev;
            this.ugyfelTelefonszam = ugyfelTelefonszam;
            this.ugyfelEmail = ugyfelEmail;
            this.ugyfelPont = ugyfelPont;
            this.jarmuId = jarmuId;
            this.jarmuRendszam = jarmuRendszam;
            this.jarmuFerohely = jarmuFerohely;
            this.rendelesDatum = rendelesDatum;
        }

        // HACK: rendelesVM létrehozása

        public rendelesVM(string ugyfelNev, string jarmuRendszam, DateTime datum)
        {
            this.ugyfelNev = ugyfelNev;
            this.jarmuRendszam = jarmuRendszam;
            this.rendelesDatum = datum;
        }

        
    }
}
