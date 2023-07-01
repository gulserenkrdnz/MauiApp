using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp2Odev.Models;
using Microsoft.Maui.Controls;
using Firebase.Database;
using Firebase.Database.Query;

namespace MauiApp2Odev.Views
{
    [Microsoft.Maui.Controls.Xaml.XamlCompilation(Microsoft.Maui.Controls.Xaml.XamlCompilationOptions.Compile)]
    public partial class YapilacakDetay : Microsoft.Maui.Controls.ContentPage
    {
        private FirebaseClient client;
        private Gorev seciliGorev;

        public event EventHandler<Gorev> GorevGuncellendi; 

        public YapilacakDetay(Gorev seciliGorev)
        {
            InitializeComponent();
            client = new FirebaseClient("https://todolist-195f8-default-rtdb.firebaseio.com/");
            this.seciliGorev = seciliGorev;

            if (seciliGorev != null)
            {
                txtGorevDetay.Text = seciliGorev.GorevDetayi;
                datePicker.Date = seciliGorev.Tarih.Date;
                timePicker.Time = seciliGorev.Tarih.TimeOfDay;
            }
        }

        private async void Kaydet_Clicked(object sender, EventArgs e)
        {
            if (seciliGorev != null)
            {
                seciliGorev.GorevDetayi = txtGorevDetay.Text;
                seciliGorev.Tarih = datePicker.Date + timePicker.Time;

                try
                {
                    var altBilgiler = await client.Child("gorevler").Child(seciliGorev.Id).Child("AltBilgiler").OnceAsync<AltBilgi>();
                    foreach (var altBilgi in altBilgiler)
                    {
                        await client.Child("gorevler").Child(seciliGorev.Id).Child("AltBilgiler").Child(altBilgi.Key).DeleteAsync();
                    }

                    foreach (var altBilgi in seciliGorev.AltBilgiler)
                    {
                        await client.Child("gorevler").Child(seciliGorev.Id).Child("AltBilgiler").PostAsync(altBilgi);
                    }

               
                    var detayAltBilgi = new AltBilgi { Bilgi = seciliGorev.GorevDetayi };
                    await client.Child("gorevler").Child(seciliGorev.Id).Child("AltBilgiler").PostAsync(detayAltBilgi);

                    await DisplayAlert("Baþarý", "Görev detaylarý kaydedildi", "Tamam");
                    GorevGuncellendi?.Invoke(this, seciliGorev); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Görev detaylarýný kaydederken hata oluþtu: {ex.Message}");
                    await DisplayAlert("Hata", "Görev detaylarýný kaydederken bir hata oluþtu", "Tamam");
                }
            }
        }

        private async void Guncelle_Clicked(object sender, EventArgs e)
        {
            
            seciliGorev.GorevDetayi = txtGorevDetay.Text;
          
            seciliGorev.Tarih = DateTime.Now;

            
            GorevGuncellendi?.Invoke(this, seciliGorev);

            await DisplayAlert("Bilgi", "Görev güncellendi.", "Tamam");
        }

        private async void Geri_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

            var yapilacaklarPage = Application.Current.MainPage?.Navigation.NavigationStack.FirstOrDefault(p => p is Yapilacaklar) as Yapilacaklar;
            yapilacaklarPage?.LoadGorevler();
        }

    }
}
