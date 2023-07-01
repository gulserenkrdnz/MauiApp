using Microsoft.Maui.ApplicationModel.DataTransfer;
using MauiApp2Odev.Models;

namespace MauiApp2Odev.Views
{
    public partial class HaberDetay : ContentPage
    {
        Item haber;

        public HaberDetay(Item item)
        {
            InitializeComponent();
            haber = item;
            webView.Source = item.link;
        }

        private async void PaylasClicked(object sender, EventArgs e)
        {
            await Paylas(haber.link, Share.Default);
        }

        public async Task Paylas(string uri, IShare paylasim)
        {
            await paylasim.RequestAsync(new ShareTextRequest
            {
                Uri = uri,
                Title = haber.title
            });
        }
    }
}
