namespace Expen;

public partial class ViewWalletSelector : ContentView
{
	public ViewWalletSelector()
	{
		InitializeComponent();
	}    
    public VerticalStackLayout sl { get { return SL; } set { SL = value; } } 

    List<clsWallet>? Wallets;
	public async void LoadWallets()
	{
		vLayout.Children.Clear();
		Wallets = await clsWallet.GetAll();
		if (Wallets == null) return;

		foreach(clsWallet w in Wallets)
		{
			ViewSelectedWallet sw = new(w);
			sw.Mode = ViewSelectedWallet.enMode.Select;
            vLayout.Children.Add(sw);
        }
    }
}