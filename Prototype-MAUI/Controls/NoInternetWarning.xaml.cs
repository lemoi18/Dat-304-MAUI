namespace MauiApp8.Controls
{
    public partial class NoInternetWarning : ContentView
    {
        public NoInternetWarning()
        {
            InitializeComponent();

            Connectivity.ConnectivityChanged += OnConnectivityChanged;
            CheckConnectivity();
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckConnectivity();
        }

        private void CheckConnectivity()
        {
            bool isConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            NoInternetWarningLabel.IsVisible = !isConnected;
            NoInternetWarningLabel.HeightRequest = isConnected ? 0 : 30;
        }

    }
}
