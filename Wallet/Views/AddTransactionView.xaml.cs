using Autofac;
using Wallet.Domain.Models;
using Wallet.Domain.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wallet.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class AddTransactionView : Page
    {
        private readonly ICategoryService _categoryService;
        private readonly IAccountDataService _accountDataService;

        public AddTransactionView()
        {
            this.InitializeComponent();

            _categoryService = App.Container.Resolve<ICategoryService>();
            _accountDataService = App.Container.Resolve<IAccountDataService>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var accountId = e.Parameter;

            if (accountId != null)
            {
                SaveTransactionButton.IsEnabled = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void NewTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            var transaction = new MoneyTransaction();

            _accountDataService.AddTransaction(transaction, 0);
        }
    }
}
