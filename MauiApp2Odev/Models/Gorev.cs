using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2Odev.Models
{
    public class Gorev
    {
        public string Id { get; set; }
        public string GorevAdi { get; set; }
        public string GorevDetayi { get; set; }
        public DateTime Tarih { get; set; }
        public List<AltBilgi> AltBilgiler { get; set; }

        public bool IsSelected { get; set; }

        public Gorev()
        {
            AltBilgiler = new List<AltBilgi>();
        }
    }

    public class AltBilgi
    {
        public string Bilgi { get; set; }
    }

}
