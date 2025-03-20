

namespace Expen;

public partial class ViewWallet : ContentView
{
	public enum enParent { enHomePage, enWalletPage }
	public enParent parent { get; set; } = enParent.enHomePage;
    public ViewWallet()
	{
		InitializeComponent();
		Click();
    }
    void Click()
    {
        TapGestureRecognizer click = new TapGestureRecognizer();        
        click.Tapped += async (s, e) =>
        {
            clsWallet W = new clsWallet(Wallet);
            if (parent == enParent.enHomePage)
                await Navigation.PushAsync(new PageWalletAddEdit(W));            
            else            
                await Navigation.PushAsync(new PageWalletDetails(W));
        };       
        frm.GestureRecognizers.Add(click);
    }
    public ViewWallet(clsWallet w)
    {
        InitializeComponent();
		Wallet = w;
        Load();
    }

	clsWallet Wallet = new clsWallet();
    void Load()
	{
		Type = Wallet.Type;
        Name = Wallet.Name;
        Balance = Wallet.Balance.ToString("0.00");

		if (Wallet.imgURL != null && Wallet.imgURL != "")
            WalletImg.Source = Wallet.imgURL;

		if (Wallet.color == "" || Wallet.color == null)
			frm.Background = new SolidColorBrush(Color.FromArgb("#fff"));
		else
			frm.Background = new SolidColorBrush(Color.FromArgb(Wallet.color));
        Click();
    }
	public string Type
	{
		get { return lblType.Text; }
		set { lblType.Text = value; }
	}
    public string Name 
	{ 
		get { return lblName.Text; }
		set { lblName.Text = value; }
	}
	public string Balance
	{
		get { return lblBalance.Text; }
		set { lblBalance.Text = value; }
	}
}