using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp2Odev.Models;


namespace MauiApp2Odev.Views
{
    public partial class Haberler : ContentPage
    {
        public Haberler()
        {
            InitializeComponent();
        }

        private void OnAppearing(object sender, EventArgs e)
        {
           
            LoadKategoriler();
        }

        private void LoadKategoriler()
        {
            lstKategoriler.ItemsSource = Models.Kategori.liste;
        }

        private async void LoadHaberler()
        {
            var selectedKategori = lstKategoriler.SelectedItem as Models.Kategori;
            if (selectedKategori != null)
            {
                string jsonData = await Models.Servisler.HaberleriGetir(selectedKategori);
                if (!string.IsNullOrEmpty(jsonData))
                {
                    var root = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Root>(jsonData);
                    lstHaberler.ItemsSource = root.items;
                }
            }
        }

        private void lstKategoriler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadHaberler();
        }

        private void lstHaberler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            var selectedHaber = e.CurrentSelection.FirstOrDefault() as Item;

            if (selectedHaber != null)
            {
         
                var haberDetayPage = new HaberDetay(selectedHaber);

               
                Navigation.PushAsync(haberDetayPage);

               
                lstHaberler.SelectedItem = null;
            }
        }

    }
}
