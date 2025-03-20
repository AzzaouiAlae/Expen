namespace Expen;

public partial class PageWalletDetails : ContentPage
{
	public PageWalletDetails(clsWallet W)
	{
		InitializeComponent();
        Wallet = W;
        Load();
    }
    void Load()
    {
        Click();
        loadWallet();
        LoadDate();
        LoadTransactions();
        ReLoadAnalysis();
    }
	void loadWallet()
	{
        imgWallet.Source = Wallet.imgURL;
        lblNameWallet.Text = Wallet.Name;
        lblDateWallet.Text = "Date Added: " + Wallet.Date.ToString("dd/MM/yyyy");
        brImg.Background = new SolidColorBrush(Color.FromArgb(Wallet.color));
        lblTotalBadget.Text = Wallet.Balance.ToString("0.00") ;
    }

    clsWallet Wallet;

    DateTime date = DateTime.Now;
    void Click()
	{
        TapGestureRecognizer lblBackClick = new TapGestureRecognizer();
        lblBackClick.Tapped += async (s, e) =>
        {
            await Navigation.PopAsync();
        };
        lblBack.GestureRecognizers.Add(lblBackClick);


        TapGestureRecognizer BackClick = new TapGestureRecognizer();
        BackClick.Tapped += (s, e) =>
        {
            date = date.AddMonths(-1);
            LoadTransactions();
            LoadDate();
            ReLoadAnalysis();
        };
        lblBackDate.GestureRecognizers.Add(BackClick);

        TapGestureRecognizer NextClick = new TapGestureRecognizer();
        NextClick.Tapped += (s, e) =>
        {
            date = date.AddMonths(1);
            LoadTransactions();
            LoadDate();
            ReLoadAnalysis();
        };
        lblNextDate.GestureRecognizers.Add(NextClick);        
    }
    void LoadDate()
    {
        lblDate.Text = date.ToString("MMMM yyyy");

        if (lblDate.Text == DateTime.Now.ToString("MMMM yyyy"))
            lblNextDate.IsVisible = false;
        else
            lblNextDate.IsVisible = true;
    }
    async void LoadTransactions()
    {
        List<clsTransaction>? Transactions = await clsTransaction.GetTransactonsInMonth(date);
        vLayoutTransaction.Children.Clear();
        if (Transactions == null) return;
        if (Transactions.Count <= 0) return;

        foreach (var t in Transactions)
        {
            if (t.WalletID != Wallet.ID) continue;
            ViewTransaction vt = new(t);
            vLayoutTransaction.Children.Add(vt);
        }
    }
    private async void btnEditWallet_Clicked(object sender, EventArgs e)
    {
        clsWallet W = new clsWallet(Wallet);
        await Navigation.PushAsync(new PageWalletAddEdit(W));
    }    
    void ReLoadAnalysis()
    {
        vAnalysis.date = date;
        vAnalysis.Wallet = Wallet;
        vAnalysis.Load();
    }
}