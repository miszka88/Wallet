using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wallet.Views
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void TxtPassword_PasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
        {
            BtnLogIn.IsEnabled = IsFormValid();
        }

        private void TxtUserName_MyTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            BtnLogIn.IsEnabled = IsFormValid();
        }

        private bool IsFormValid()
        {
            return TxtUserName.IsValid && TxtPassword.Password.Length > 0;
        }
    }
}
