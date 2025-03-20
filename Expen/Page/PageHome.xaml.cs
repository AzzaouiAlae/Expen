namespace Expen;

public partial class PageHome : ContentPage
{
	public PageHome()
	{
		InitializeComponent();
        Profile = new clsProfile(1, () => LoadProfile());
        Load();
        Click();
    }
    void AddDateLabel(string date)
    {
        Label l = new Label()
        {
            TextColor = Color.FromArgb("#777"),
            FontFamily = "BeVietnamProMedium",
            Margin = new Thickness (0, 20, 0, 0),
            FontSize = 12,
            Text = date
        };
        vLayoutTransactions.Children.Add(l);
    }
    async void LoadTransactions()
    {
        vLayoutTransactions.Children.Clear();
        List<clsTransaction>? transaction = await clsTransaction.GetLast10Obj();
        await clsCategories.FillDefault();

        if (transaction == null) return;
        if (transaction.Count <= 0) return;
        string Date = "";
        foreach (clsTransaction t in transaction)
        {
            if(Date != t.Date.ToString("dd MMMM, yyyy"))
            {
                Date = t.Date.ToString("dd MMMM, yyyy");
                AddDateLabel(Date);
            }
            t.Saved += (T) => LoadTransactions();
            t.Saved += (T) => refreshWallet();
            t.Saved += (T) => LoadAnalysis();
            ViewTransaction vt = new ViewTransaction(t);
            vLayoutTransactions.Children.Add(vt);
        }        
    }
    void LoadAnalysis()
    {
        vAnalysis1.Date = DateTime.Now;
        vAnalysis2.Date = DateTime.Now.AddMonths(-1);
    }
    void Load()
    {
        refreshWallet();
        LoadTransactions();
        LoadAnalysis();
        LoadProfile(); 
    }
    async void refreshWallet()
    {
        hLayoutWallets.Children.Clear();
        List<clsWallet>? wallets = await clsWallet.GetAll();
        if (wallets == null) return;
        if (wallets.Count <= 0) return;
        foreach (clsWallet w in wallets)
        {
            w.Saved += (w) => refreshWallet();
            ViewWallet vw = new ViewWallet(w);
            hLayoutWallets.Children.Add(vw);
        }
        lblWalletCount.Text = $"({hLayoutWallets.Children.Count})";
    }
    void Click()
    {
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped += async (s, e) =>
        {
            await Navigation.PushAsync(new PageStrat());
        };
        imgProfile.GestureRecognizers.Add(click);        
    }

    clsProfile Profile;
    void LoadProfile()
    {
        if (File.Exists(Profile.imgURL))
            imgProfile.Source = Profile.imgURL;

        txtName.Text = Profile.Name;
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var p = new PageWalletAddEdit();
        p.wallet.Saved += (w) => refreshWallet();
        await Navigation.PushAsync(p);
    }
    public async void Hello()
    {
        await DisplayAlert("", "Hello", "Ok");
    }
}