using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace MauiApp2Odev.Views
{
    public partial class Ayarlar : ContentPage
    {
        public Ayarlar()
        {
            InitializeComponent();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var switchControl = (Switch)sender;
            if (switchControl.IsToggled)
            {
             
                Application.Current.UserAppTheme = AppTheme.Dark;
                
            }
            else
            {
               
                Application.Current.UserAppTheme = AppTheme.Light;
            }
        }
    }
}
