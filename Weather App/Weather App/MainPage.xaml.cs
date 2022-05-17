using Weather_App.ViewModel;
using Xamarin.Forms;
namespace Weather_App
{
    public partial class MainPage : ContentPage
    {
        MainViewModel mainViewModel;
        public MainPage()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();
            BindingContext = mainViewModel;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //mainViewModel.LoadData();
        }
    }
}
