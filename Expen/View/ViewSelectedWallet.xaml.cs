namespace Expen;

public partial class ViewSelectedWallet : ContentView
{
    public enum enMode { Show, Select}

    public enMode Mode = enMode.Show;

    clsWallet _Wallet = new();
    public clsWallet Wallet
    {
        get { 
            return _Wallet;
        }
        set { 
            _Wallet = value;
            img.Source = _Wallet.imgURL;
            lblName.Text = _Wallet.Name;
            lblBalance.Text = $"{_Wallet.Balance}";
            br.Background = Color.FromArgb(_Wallet.color);
        }
    }
    public ViewSelectedWallet()
	{
		InitializeComponent();
		Click();
        LoadWallet();
        AddClickDisposeView();
    }
    async public void LoadWallet()
    {
        if(_Wallet.ID == -1)
        {
            var wallets = await clsWallet.GetAll();
            if (wallets != null && wallets.Count > 0)
                Wallet = wallets[0];
        }
        else
        {
            var wallet = await clsWallet.Find(_Wallet.ID);
            if(wallet != null)
                Wallet = wallet;
        }
    }
    public ViewSelectedWallet(clsWallet w)
    {
        InitializeComponent();
        Click();
        Wallet = w;
    }
    void Click()
	{
        TapGestureRecognizer BrClick = new ();
        BrClick.Tapped += (s, e) =>
        {
            if(Mode == enMode.Show)
            {
                if(myGrid != null)
                {
                    SelectWallet = LoadWallet;                    
                    myGrid.AddWithSpan(vWalletSelector, 0, 0, Rows, Column);
                    vWalletSelector.LoadWallets();                    
                }
            }
            else if (Mode == enMode.Select)            
                SelectWallet?.Invoke(Wallet);            
        };
        br.GestureRecognizers.Add(BrClick);
    }
    static Action<clsWallet>? SelectWallet { get; set; }
    void LoadWallet(clsWallet W)
    {
        Wallet = W;
        if (myGrid != null)        
            myGrid.Remove(vWalletSelector);
        WalletChange?.Invoke(Wallet);
    }    

    Grid? myGrid;

    int Rows = 1;

    int Column = 1;
    public void LoadGrid(Grid g, int rows, int column)
    {
        myGrid = g;
        Rows = rows;
        Column = column;
    }

	static ViewWalletSelector vWalletSelector = new();
    void AddClickDisposeView() 
    {        
        TapGestureRecognizer slClick = new();
        slClick.Tapped += (s, e) =>
        {
            if (myGrid != null)            
                myGrid.Remove(vWalletSelector);            
        };
        vWalletSelector.sl.GestureRecognizers.Add(slClick);
    }

    public event Action<clsWallet>? WalletChange;
}