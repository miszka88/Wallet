using Autofac;
using System.Collections.ObjectModel;
using Wallet.Domain.Models;
using Wallet.Domain.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace Wallet
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IAccountDataService _accountDataService;
        private readonly ICategoryService _categoryService;

        private ObservableCollection<GroupedUserAccount> GroupedUserAccounts { get; set; } = new ObservableCollection<GroupedUserAccount>();
        private ObservableCollection<GroupedMoneyTransaction> GroupedAccountTransactions { get; set; } = new ObservableCollection<GroupedMoneyTransaction>();

        public MainPage()
        {
            this.InitializeComponent();

            _accountDataService = App.Container.Resolve<IAccountDataService>();
            _categoryService = App.Container.Resolve<ICategoryService>();
        }

        private async void AccountsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedUserAccount = (UserAccountObject)e.ClickedItem;

            if (selectedUserAccount != null) { GroupedAccountTransactions.Clear(); }
            else { return; }

            GroupedAccountTransactions = await _accountDataService.GetGroupedTransactionsByAccountId(selectedUserAccount.UserAccount.Id);

            AccountTransactionsList.Source = GroupedAccountTransactions;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            GroupedUserAccounts = await _accountDataService.GetGroupedUserAccounts();

            UserAccountsList.Source = GroupedUserAccounts;
        }
    }
}
