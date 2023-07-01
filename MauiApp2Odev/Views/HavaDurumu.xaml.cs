using MauiApp2Odev.Models;
using System.Collections.ObjectModel;
using System.Text.Json;
using static MauiApp2Odev.Models.SehirHavaDurumu;
namespace MauiApp2Odev.Views;

public partial class HavaDurumu : ContentPage
{

    public HavaDurumu()
    {
        InitializeComponent();
        string fileName = Path.Combine(FileSystem.Current.AppDataDirectory, "hdata.json");

        if (File.Exists(fileName))
        {
            string data = File.ReadAllText(fileName);
            Sehirler = JsonSerializer.Deserialize<ObservableCollection<SehirHavaDurumu>>(data);
        }

        listCity.ItemsSource = Sehirler;
    }

    ObservableCollection<SehirHavaDurumu> Sehirler = new ObservableCollection<SehirHavaDurumu>();

    private async void EkleClicked(object sender, EventArgs e)
    {
        string sehir = await DisplayPromptAsync("Þehir:", "Þehir ismi", "OK", "Cancel");
        sehir = sehir.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        sehir = sehir.Replace('Ç', 'C');
        sehir = sehir.Replace('Ð', 'G');
        sehir = sehir.Replace('Ý', 'I');
        sehir = sehir.Replace('Ö', 'O');
        sehir = sehir.Replace('Ü', 'U');
        sehir = sehir.Replace('Þ', 'S');

        Sehirler.Add(new SehirHavaDurumu() { Name = sehir });

        string fileName = Path.Combine(FileSystem.Current.AppDataDirectory, "hdata.json");
        string data = JsonSerializer.Serialize(Sehirler);
        File.WriteAllText(fileName, data);
    }

    private void YukleClicked(object sender, EventArgs e)
    {
        refreshView.IsRefreshing = false;
    }

    private async void SilClicked(object sender, EventArgs e)
    {
        var b = sender as ImageButton;
        if (b != null)
        {
            var t = Sehirler.First(o => o.Name == b.CommandParameter.ToString());
            var yes = await DisplayAlert("Silinsin mi?", "Silmeyi onayla", "OK", "Cancel");
            if (yes)
            {
                Sehirler.Remove(t);

                string fileName = Path.Combine(FileSystem.Current.AppDataDirectory, "hdata.json");
                string data = JsonSerializer.Serialize(Sehirler);
                File.WriteAllText(fileName, data);
            }
        }
    }

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        string fileName = Path.Combine(FileSystem.Current.AppDataDirectory, "hdata.json");
        string data = JsonSerializer.Serialize(Sehirler);
        File.WriteAllText(fileName, data);
    }
}
