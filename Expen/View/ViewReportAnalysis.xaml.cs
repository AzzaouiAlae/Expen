namespace Expen;
public enum enAnalysisType { Expense, Income }
public enum enAnalysisBy { Day, Week, Month, Year }
public partial class ViewReportAnalysis : ContentView
{
    void SetReload()
    {
        if (AnalysisType == enAnalysisType.Expense)
            ReLoadExpense += Load;
        else if (AnalysisType == enAnalysisType.Income)
            ReLoadIncome += Load;
    }
    public ViewReportAnalysis()
    {
        InitializeComponent();        
    }

    float _Amount;

    static float maxIncome = 0;

    static float maxExpense = 0;

    DateTime _date = DateTime.Now;

    enAnalysisType _AnalysisType;
    public enAnalysisType AnalysisType 
    {
        get
        {
            return _AnalysisType;
        }
        set
        {
            _AnalysisType = value;
            SetReload();
        }
    }
    public enAnalysisBy AnalysisBy { get; set; } = enAnalysisBy.Month;
    public float Amount { get { return _Amount; } set {  _Amount = value; } }
    static public float MaxExpense { get { return maxExpense; } set { maxExpense = value; } }    
    static public float MaxIncome { get { return maxIncome; } set { maxIncome = value;  } }
    public DateTime Date
    {
        get
        {
            return _date;
        }
        set
        {
            _date = value;
            GetAmount();
        }
    }
    void SetLblAmount()
    {
        if (_Amount < 1000)
            lblAmount.Text = _Amount.ToString("0.0");
        else if (_Amount >= 1000000)
            lblAmount.Text = (_Amount / 1000000).ToString("0.0") + "M";
        else if (_Amount >= 1000)
            lblAmount.Text = (_Amount / 1000).ToString("0.0") + "K";
    }
    void SetBrHeight()
    {       
        if (AnalysisType == enAnalysisType.Expense)
        {
            br.Background = new SolidColorBrush(Color.FromArgb("#F66"));

            if (MaxExpense < _Amount)
            {
                MaxExpense = _Amount;
                ReLoadExpense?.Invoke();
            }

            if (_Amount > 0)
                br.HeightRequest = (150 / MaxExpense) * _Amount;
            else
                br.HeightRequest = 0;
        }
        else if (AnalysisType == enAnalysisType.Income)
        {
            br.Background = new SolidColorBrush(Color.FromArgb("#66F"));

            if (MaxIncome < _Amount)
            {
                MaxIncome = _Amount;
                ReLoadIncome?.Invoke();
            }

            if (_Amount > 0)
                br.HeightRequest = (150 / MaxIncome) * _Amount;
            else
                br.HeightRequest = 0;
        }        
    }
    void SetLblDate()
    {
        switch(AnalysisBy)
        {
            case enAnalysisBy.Day:
                lblDate.Text = Date.ToString("ddd dd");
                break;            
            case enAnalysisBy.Week:
                DateTime dt = new DateTime(Date.Year, Date.Month, Date.Day);
                var firstDayOfWeek = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
                var lastDayOfWeek = dt.AddDays(-(int)dt.DayOfWeek + 8).AddSeconds(-1);
                lblDate.Text = firstDayOfWeek.ToString("dd MMM");
                lblDate.Text += lastDayOfWeek.ToString("\ndd MMM");
                break;
            case enAnalysisBy.Month:
                lblDate.Text = Date.ToString("MMMM\nyyyy");
                break;
            case enAnalysisBy.Year:
                lblDate.Text = Date.ToString("yyyy");
                break;
        }
    }
    async void GetAmount()
    {
        _Amount = await clsTransaction.GetSumAmountByDate(Date, (int)AnalysisBy, (int)AnalysisType);
        Load();
    }
    void Load()
    {
        SetLblAmount();
        SetBrHeight();
        SetLblDate();
    }

    static Action? ReLoadExpense;

    static Action? ReLoadIncome;
}