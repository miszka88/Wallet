using Autofac;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Domain.Models;
using Wallet.Domain.Services;
using Wallet.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Wallet
{
    public sealed partial class MainPage : Page
    {
        private readonly IAccountDataService _accountDataService;
        private readonly ICategoryService _categoryService;

        private UserAccountObject _lastSelectedListItem;
        private MoneyTransactionObject _lastSelectedTransactionItem;
        private ObservableCollection<GroupedUserAccount> GroupedUserAccounts;
        private ObservableCollection<MoneyTransactionObject> TransactionsList;

        public MainPage()
        {
            this.InitializeComponent();

            _accountDataService = App.Container.Resolve<IAccountDataService>();
            _categoryService = App.Container.Resolve<ICategoryService>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            GroupedUserAccounts = await _accountDataService.GetGroupedUserAccounts();
            Accounts.Source = GroupedUserAccounts;

            if (e.Parameter != null)
            {
                _lastSelectedListItem = GroupedUserAccounts.SelectMany(g => g.Where(x => x.UserAccount.Id == (long)e.Parameter)).Select(y => y).Single();
            }

            UpdateForVisualState(AdaptiveStates.CurrentState);
            DisableContentTransitions();

            AccountsList.SelectedItem = _lastSelectedListItem;
        }

        private async void AccountsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (UserAccountObject)e.ClickedItem;
            _lastSelectedListItem = clickedItem;

            TransactionsList = await _accountDataService.GetTransactionsByAccountId(_lastSelectedListItem.UserAccount.Id);
            AccountDetails.Source = TransactionsList;

            if (AdaptiveStates.CurrentState == NarrowState)
            {
                Frame.Navigate(typeof(AccountDetailView), _lastSelectedListItem.UserAccount.Id, new DrillInNavigationTransitionInfo());
            }
            else
            {
                EnableContentTransitions();
            }

            EnableCommandBarActions(clickedItem);
            FillCommandBarAccountName(clickedItem.UserAccount.BankName);
        }

        private void EnableCommandBarActions(UserAccountObject accountObject)
        {
            if (accountObject != null && AccountsList.SelectedIndex > -1)
            {
                AddTransaction.IsEnabled = true;
            }
        }

        private void FillCommandBarAccountName(string bankName)
        {
            if (!string.IsNullOrWhiteSpace(bankName))
            {
                CommandBarAccountName.Text = bankName;
                CommandBarContent.Visibility = Visibility.Visible;
            }
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState = null)
        {
            bool isNarrow = newState == NarrowState;

            if (isNarrow && oldState == DefaultState && _lastSelectedListItem != null)
            {
                Frame.Navigate(typeof(AccountDetailView), _lastSelectedListItem.UserAccount.Id, new SuppressNavigationTransitionInfo());
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(AccountsList, isNarrow);
            if (DetailContentPresenter != null)
            {
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
            }
        }

        private void EnableContentTransitions()
        {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }

        private void DisableContentTransitions()
        {
            if (DetailContentPresenter != null)
            {
                DetailContentPresenter.ContentTransitions.Clear();
            }
        }

        private void AccountDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoneyTransactionObject selectedItem = e.AddedItems.SingleOrDefault() as MoneyTransactionObject;

            if (selectedItem == null)
            {
                return;
            }

            RemoveTransaction.IsEnabled = true;

            ListViewItem listItem = (sender as ListView).ContainerFromItem(selectedItem) as ListViewItem;

            ExtendTransactionDetails(listItem);

            if (_lastSelectedTransactionItem != null)
            {
                listItem = (sender as ListView).ContainerFromItem(_lastSelectedTransactionItem) as ListViewItem;
                CollapseTransactionDetails(listItem);
            }

            _lastSelectedTransactionItem = selectedItem;
        }

        private void ExtendTransactionDetails(ListViewItem listItem)
        {
            listItem.ContentTemplate = (DataTemplate)this.Resources["TransactionExtendedDetails"];
        }

        private void CollapseTransactionDetails(ListViewItem listItem)
        {
            listItem.ContentTemplate = (DataTemplate)this.Resources["TransactionBasicDetails"];
        }

        private async void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = AccountsList.SelectedItem as UserAccountObject;
            var userAccountId = selectedAccount.UserAccount.Id;

            if (userAccountId > 0)
            {
                Frame.Navigate(typeof(AddTransactionView), userAccountId);
            }
            await RefreshAccountsData();
        }

        private async void RemoveTransaction_Click(object sender, RoutedEventArgs e)
        {
            var transactionId = _lastSelectedTransactionItem.MoneyTransaction.Id;

            if (transactionId > 0)
            {
                await _accountDataService.RemoveTransaction(transactionId);
            }
            var item = TransactionsList.Single(x => x.MoneyTransaction.Id == transactionId);
            TransactionsList.Remove(item);
            _lastSelectedTransactionItem = null;
            await RefreshAccountsData();
        }

        private async Task RefreshAccountsData()
        {
            GroupedUserAccounts = await _accountDataService.GetGroupedUserAccounts();
            Accounts.Source = GroupedUserAccounts;
        }
    }
}
