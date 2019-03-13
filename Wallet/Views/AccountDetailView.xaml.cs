using Autofac;
using Wallet.Domain.Services;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Wallet.Views
{
    public sealed partial class AccountDetailView : Page
    {
        private readonly IAccountDataService _accountDataService;
        public AccountDetailView()
        {
            _accountDataService = App.Container.Resolve<IAccountDataService>();

            InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            long accountId = (long)e.Parameter;
            AccountDetailList.Source = await _accountDataService.GetTransactionsByAccountId(accountId);

            var backStack = Frame.BackStack;
            var backStackCount = backStack.Count;

            if (backStackCount > 0)
            {
                var mainPageEntry = backStack[backStackCount - 1];
                backStack.RemoveAt(backStackCount - 1);

                var modifiedEntry = new PageStackEntry(mainPageEntry.SourcePageType, accountId, mainPageEntry.NavigationTransitionInfo);
                backStack.Add(modifiedEntry);
            }

            SystemNavigationManager snm = SystemNavigationManager.GetForCurrentView();
            snm.BackRequested += DetailPage_BackRequested;
            snm.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= DetailPage_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private bool ShouldGoToWideState()
        {
            return Window.Current.Bounds.Width >= 720;
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (ShouldGoToWideState())
            {
                Window.Current.SizeChanged -= Window_SizeChanged;

                NavigateBackForWideState(useTransition: false);
            }
        }

        private void NavigateBackForWideState(bool useTransition)
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;

            if (useTransition)
            {
                Frame.GoBack(new EntranceNavigationTransitionInfo());
            }
            else
            {
                Frame.GoBack(new SuppressNavigationTransitionInfo());
            }
        }

        private void DetailPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            OnBackRequested();
        }

        private void OnBackRequested()
        {
            Frame.GoBack(new DrillInNavigationTransitionInfo());
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ShouldGoToWideState())
            {
                NavigateBackForWideState(useTransition: true);
            }
            else
            {
                FindName("AccountDetails");
            }

            Window.Current.SizeChanged += Window_SizeChanged;
        }
    }
}
