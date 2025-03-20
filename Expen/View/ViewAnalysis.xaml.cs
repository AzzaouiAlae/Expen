namespace Expen;

public partial class ViewAnalysis : ContentView
{
	public ViewAnalysis()
	{
		InitializeComponent();
        Load();
    }
    public bool isHomeAnalysis { get; set; } = false;

    public static double HomeAnalysisMax = 0;

	double _Income;

    double _Expense;

    DateTime _Date;
    void HomeAnalysis()
    {
        if(HomeAnalysisMax < _Income)
            HomeAnalysisMax = _Income;

        if(_Expense > HomeAnalysisMax)
            HomeAnalysisMax = _Expense;
        
        if(_Income != 0)
            boxBlue.HeightRequest = (150 / HomeAnalysisMax) * _Income;

        if (_Expense != 0)
            boxRed.HeightRequest = (150 / HomeAnalysisMax) * _Expense;        
    }
    void SetObject()
    {
        if (isHomeAnalysis)
            HomeAnalysis();

        else if (_Income == _Expense)
        {
            if(_Income == 0)
            {
                boxBlue.HeightRequest = 0;
                boxRed.HeightRequest = 0;
            }
            else
            {
                boxBlue.HeightRequest = 150;
                boxRed.HeightRequest = 150;
            }            
        }
        else if (_Income > _Expense)
        {
            boxBlue.HeightRequest = 150;
            boxRed.HeightRequest = (150 / _Income) * _Expense;
        }
        else
        {
            boxRed.HeightRequest = 150;
            boxBlue.HeightRequest = (150 / _Expense) * _Income;
        }

        if (_Income < 1000)
            lblBlue.Text = _Income.ToString("0.0");

        else if (_Income >= 1000000)
            lblBlue.Text = (_Income / 1000000).ToString("0.0") + "M";

        else if (_Income >= 1000)
            lblBlue.Text = (_Income / 1000).ToString("0.0") + "K";
        


        if (_Expense < 1000)
            lblRed.Text = _Expense.ToString("0.0");

        else if (_Expense >= 1000000)
            lblRed.Text = (_Expense / 1000000).ToString("0.0") + "M";

        else if (_Expense >= 1000)
            lblRed.Text = (_Expense / 1000).ToString("0.0") + "K";        
    }
    public double Income
	{
		get { return _Income; }
		set
		{
			if (_Income != value)
			{
                _Income = value;
                SetObject();
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
                SetObject();
            }
        }
    }
    public DateTime Date 
    { 
        get { return _Date; }
        set
        {
            _Date = value;
            lblMonth.Text = _Date.ToString("MMMM yyyy");
            Load();
        }
    }
    async void Load()
    {
        double TotalIncome = 0;
        double TotalExpense = 0;        
        await Task.Delay(500);
        List<clsTransaction>? transactions = await clsTransaction.GetTransactonsInMonth(_Date);        

        if (transactions == null) return;
        if (transactions.Count <= 0) return;
        foreach (var t in transactions)
        {
            await t.LoadCategory();
            if (t.Category.Type == 1)
                TotalIncome += t.Amount;
            else if (t.Category.Type == 0)
                TotalExpense += t.Amount;
        }
        Income = TotalIncome;
        Expense = TotalExpense;
    }
}