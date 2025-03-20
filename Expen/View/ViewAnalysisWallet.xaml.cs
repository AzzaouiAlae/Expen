namespace Expen;

public partial class ViewAnalysisWallet : ContentView
{
	public ViewAnalysisWallet()
	{
		InitializeComponent();
	}

    double _Income;

    double _Expense;

    public clsWallet Wallet = new();
    void SetObject()
    {
        double Max = 0;
        if(Max < _Income)
            Max = _Income;

        if(Max < _Expense)
            Max = _Expense;

        if(Max < Wallet.Balance)
            Max = Wallet.Balance;

        double num = 150 / Max;

        boxRed.HeightRequest = num * _Expense;
        boxBlue.HeightRequest = num * _Income;
        boxGreen.HeightRequest = num * Wallet.Balance;


        if (_Income < 1000)
            lblBlue.Text = _Income.ToString("0.0");
        else if (_Income > 1000000)
            lblBlue.Text = (_Income / 1000000).ToString("0.0") + "M";
        else if (_Income > 1000)
            lblBlue.Text = (_Income / 1000).ToString("0.0") + "K";


        if (_Expense < 1000)
            lblRed.Text = _Expense.ToString("0.0");
        else if (_Expense > 1000000)
            lblRed.Text = (_Expense / 1000000).ToString("0.0") + "M";
        else if (_Expense > 1000)
            lblRed.Text = (_Expense / 1000).ToString("0.0") + "K";


        if (Wallet.Balance < 1000)
            lblGreen.Text = Wallet.Balance.ToString("0.0");
        else if (Wallet.Balance > 1000000)
            lblGreen.Text = (Wallet.Balance / 1000000).ToString("0.0") + "M";
        else if (Wallet.Balance > 1000)
            lblGreen.Text = (Wallet.Balance / 1000).ToString("0.0") + "K";
    }

    public DateTime date = DateTime.Now;
    public double Income
    {
        get { return _Income; }
        set
        {
            if (_Income != value)
            {
                _Income = value;
            }
        }
    }
    public double Expense
    {
        get { return _Expense; }
        set
        {
            if (_Expense != value)
            {
                _Expense = value;                
            }
        }
    }
    async public void Load()
    {
        double TotalIncome = 0;
        double TotalExpense = 0;
        Income = 0;
        Expense = 0;        
        List<clsTransaction>? transactions = await clsTransaction.GetTransactonsInMonth(date);

        if (transactions != null && transactions.Count> 0)
        {
            foreach (var t in transactions)
            {
                if (t.WalletID != Wallet.ID) continue;
                await t.LoadCategory();
                if (t.Type == 1)
                    TotalIncome += t.Amount;
                else if (t.Type == 0)
                    TotalExpense += t.Amount;
            }
        }
        Income = TotalIncome;
        Expense = TotalExpense;
        SetObject();
    }
}