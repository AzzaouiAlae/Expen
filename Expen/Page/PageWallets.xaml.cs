

namespace Expen;

public partial class PageWallets : ContentPage
{
	public PageWallets()
	{
		InitializeComponent();
        Load();
        Refresh = RefreshWallet;
        Refresh += LoadTransactions;
    }

    DateTime date = DateTime.Now;
    async void RefreshWallet()
    {
        hLayout.Children.Clear();
        List<clsWallet>? wallets = await clsWallet.GetAll();
        if (wallets == null) return;
        if (wallets.Count <= 0) return;
        foreach (clsWallet w in wallets)
        {
            ViewWallet vw = new ViewWallet(w);
            vw.parent = ViewWallet.enParent.enWalletPage;
            hLayout.Children.Add(vw);
        }
        lblTotalBadget.Text = wallets.Sum((w) => w.Balance).ToString("0.00");
        lblWalletCount.Text = $"({hLayout.Children.Count})";
    }
    void LoadDate()
    {
        lblDate.Text = date.ToString("MMMM yyyy");

        if(lblDate.Text == DateTime.Now.ToString("MMMM yyyy"))
            lblNext.IsVisible = false;
        else
            lblNext.IsVisible = true;
    }
    async void LoadTransactions()
    {
        List<clsTransaction>? Transactions = await clsTransaction.GetTransactonsInMonth(date);
        vLayoutTransaction.Children.Clear();
        if (Transactions == null) return;
        if (Transactions.Count <= 0) return;
        foreach ( var t in Transactions )
        {
            ViewTransaction vt = new(t);
            vLayoutTransaction.Children.Add(vt);
        }
    }
    void Load()
    {
        RefreshWallet();
        LoadDate();
        LoadTransactions();
        Click();
    }
    void Click()
    {
        TapGestureRecognizer BackClick = new TapGestureRecognizer();
        BackClick.Tapped += (s, e) =>
        {
            date = date.AddMonths(-1);
            LoadTransactions();
            LoadDate();
        };
        lblBack.GestureRecognizers.Add(BackClick);

        TapGestureRecognizer NextClick = new TapGestureRecognizer();
        NextClick.Tapped += (s, e) =>
        {
            date = date.AddMonths(1);
            LoadTransactions();
            LoadDate();
        };
        lblNext.GestureRecognizers.Add(NextClick);
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageWalletAddEdit());
    }
    private async void btnAddTranaction_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageTransaction());
    }
    public static Action? Refresh { get; set; }
}