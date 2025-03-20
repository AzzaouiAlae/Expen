using System.Data.Common;

namespace Expen;

public partial class ViewTabBar : ContentView
{
	public ViewTabBar()
	{
		InitializeComponent();
        Click();
    }

    int _index;
    public int index
    {
        get { return _index; }
        set
        {
            _index = value;
            switch (_index)
            {
                case 0:
                    imgHome.Source = "icon_home.png";
                    lblHome.TextColor = Color.FromArgb("#8458A4");
                    break;

                case 1:
                    imgWallet.Source = "icon_wallet.png";
                    lblWallet.TextColor = Color.FromArgb("#8458A4");
                    break;

                case 2:
                    imgTransaction.Source = "icon_transaction.png";
                    lblTransaction.TextColor = Color.FromArgb("#8458A4");
                    break;

                case 3:
                    imgReport.Source = "report_color.png";
                    lblReport.TextColor = Color.FromArgb("#8458A4");
                    break;
            }
        }
    }
    void Click()
    {
        TapGestureRecognizer HomeClick = new TapGestureRecognizer();
        HomeClick.Tapped += async (s, e) =>
        {
            await Shell.Current.GoToAsync("//PageHome");
        };
        brHome.GestureRecognizers.Add(HomeClick);

        TapGestureRecognizer WalletClick = new TapGestureRecognizer();
        WalletClick.Tapped += async (s, e) =>
        {
            await Shell.Current.GoToAsync("//PageWallets");            
        };
        brWallet.GestureRecognizers.Add(WalletClick);

        TapGestureRecognizer TransactionClick = new TapGestureRecognizer();
        TransactionClick.Tapped += async (s, e) =>
        {
            await Shell.Current.GoToAsync("//PageTransaction");
        };
        brTransaction.GestureRecognizers.Add(TransactionClick);

        TapGestureRecognizer ReportClick = new TapGestureRecognizer();
        ReportClick.Tapped += async (s, e) =>
        {
            await Shell.Current.GoToAsync("//PageReport");
        };
        brReport.GestureRecognizers.Add(ReportClick);
    }
}