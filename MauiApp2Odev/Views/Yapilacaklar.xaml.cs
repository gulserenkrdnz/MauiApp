using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;
using MauiApp2Odev.Models;

namespace MauiApp2Odev.Views
{
    public partial class Yapilacaklar : ContentPage
    {
        static FirebaseClient client = new FirebaseClient("https://todolist-195f8-default-rtdb.firebaseio.com/");

        private List<Gorev> gorevlerListesi; 
        private Gorev seciliGorev; 

        public Yapilacaklar()
        {
            InitializeComponent();
            gorevlerListesi = new List<Gorev>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadGorevler();
        }
        public async Task LoadGorevler()
        {
            try
            {
                var gorevler = await client.Child("gorevler").OnceAsync<Gorev>();

                gorevlerListesi = gorevler.Select(x => new Gorev
                {
                    Id = x.Key,
                    GorevAdi = x.Object.GorevAdi
                }).ToList();

                lstGorevler.ItemsSource = gorevlerListesi;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Görevleri yüklerken hata oluþtu: {ex.Message}");
            }
        }



        private async void Ekle_Clicked(object sender, EventArgs e)
        {
            string gorevAdi = txtGorev.Text;

            if (!string.IsNullOrEmpty(gorevAdi))
            {
                try
                {
                    var gorev = new Gorev { GorevAdi = gorevAdi };
                    await client.Child("gorevler").PostAsync(gorev);
                    await LoadGorevler();
                    txtGorev.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Görev eklerken hata oluþtu: {ex.Message}");
                }
            }
        }

        private async void Sil_Clicked(object sender, EventArgs e)
        {
            // Seçili görevleri silme iþlemi
            var seciliGorevler = gorevlerListesi.Where(gorev => gorev.IsSelected).ToList();

            if (seciliGorevler.Count > 0)
            {
                try
                {
                    foreach (var gorev in seciliGorevler)
                    {
                        await client.Child("gorevler").Child(gorev.Id).DeleteAsync();
                        gorevlerListesi.Remove(gorev);
                    }

                    await LoadGorevler();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Görev silerken hata oluþtu: {ex.Message}");
                }
            }
        }

        private async void GorevDetay_Clicked(object sender, EventArgs e)
        {
            seciliGorev = (Gorev)((Button)sender).BindingContext;
            if (seciliGorev != null)
            {
               
                YapilacakDetay detaySayfasi = new YapilacakDetay(seciliGorev);
                detaySayfasi.GorevGuncellendi += DetaySayfasi_GorevGuncellendi; 
                await Navigation.PushAsync(detaySayfasi);
            }
        }

        private async void DetaySayfasi_GorevGuncellendi(object sender, Gorev e)
        {
           
            if (seciliGorev != null && e != null)
            {
                seciliGorev.GorevDetayi = e.GorevDetayi;
                seciliGorev.Tarih = e.Tarih;
                await LoadGorevler();
            }
        }

        private void lstGorevler_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            var seciliGorev = (Gorev)e.SelectedItem;
            if (seciliGorev != null)
            {
                seciliGorev.IsSelected = !seciliGorev.IsSelected;
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}
