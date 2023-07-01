using MauiApp2Odev.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiApp2Odev.Models
{
    public class SehirHavaDurumu
    {
        public string Name { get; set; }
        public string Source => $"https://www.mgm.gov.tr/sunum/tahmin-klasik-5070.aspx?m={Name}&basla=1&bitir=5&rC=111&rZ=fff";
    }
}
