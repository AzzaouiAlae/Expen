
using System.Text;

namespace Expen;
public enum DateType { Day , Week , Month , Year }
public partial class PageReport : ContentPage
{
    DateTime date = DateTime.Now;

    DateType dateType = DateType.Month;
    public PageReport()
    {
        InitializeComponent();
        Click();
        Load();
        Refresh = LoadExpenseAndIncome;
        Refresh += LoadExpenseByCategory;
        Refresh += LoadIncomeByCategory;
    }
    void Load()
    {
        LoadDate();      

        List<string> SelectBy = new List<string>();
        SelectBy.Add("Day");
        SelectBy.Add("Week");
        SelectBy.Add("Month");
        SelectBy.Add("Year");

        PickerBy.ItemsSource = SelectBy;
        PickerBy.SelectedIndex = 2;

        LoadExpenseAndIncome();
        LoadExpenseByCategory();
        LoadIncomeByCategory();
    }
    string DateToString(DateTime date)
    {
        switch (dateType)
        {
            case DateType.Day:
                return date.ToString("dd MM yyyy");
            case DateType.Week:
                return date.ToString("dd MM yyyy");
            case DateType.Month:
                return date.ToString("MMM yyyy");
            case DateType.Year:
                return date.ToString("yyyy");
        }
        return "";
    }
    void LoadDate()
    {
        lblDate.Text = DateToString(date);

        if (lblDate.Text == DateToString(DateTime.Now))
            lblNextDate.IsVisible = false;
        else
            lblNextDate.IsVisible = true;        
    }
    void LoadExpenseAndIncome()
    {
        ViewReportAnalysis.MaxExpense = 0;
        for (int i = 0; i < hsLayoutExpense.Children.Count; i++)
        {            
            ((ViewReportAnalysis)hsLayoutExpense.Children[i]).AnalysisBy = (enAnalysisBy)dateType;
            ((ViewReportAnalysis)hsLayoutExpense.Children[i]).Amount = 0;
        }
        for (int i = 0; i < hsLayoutExpense.Children.Count; i++)
        {
            ((ViewReportAnalysis)hsLayoutExpense.Children[i]).Date = SubDate(i);
        }


        ViewReportAnalysis.MaxIncome = 0;
        for (int i = 0; i < hsLayoutIncome.Children.Count; i++)
        {
            ((ViewReportAnalysis)hsLayoutIncome.Children[i]).AnalysisBy = (enAnalysisBy)dateType;
            ((ViewReportAnalysis)hsLayoutIncome.Children[i]).Amount = 0;
        }
        for (int i = 0; i < hsLayoutIncome.Children.Count; i++)
        {
            ((ViewReportAnalysis)hsLayoutIncome.Children[i]).Date = SubDate(i);
        }
    }
    DateTime SubDate(int i)
    {
        switch(dateType)
        {
            case DateType.Day:
                return date.AddDays(-(hsLayoutExpense.Children.Count - i - 1));
            case DateType.Week:
                return date.AddDays(-( (hsLayoutExpense.Children.Count - i - 1) * 7 ) - 1);
            case DateType.Month:
                return date.AddMonths(-(hsLayoutExpense.Children.Count - i - 1));
            case DateType.Year:
                return date.AddYears(-(hsLayoutExpense.Children.Count - i - 1));
        }
        return date;
    }
    private void btnExpense_Clicked(object sender, EventArgs e)
    {
        btnIncome.TextColor = Color.FromArgb("#888");
        brIncome.Stroke = new SolidColorBrush(Color.FromArgb("#888"));
        lblIncomeChart.IsVisible = false;
        hsLayoutIncome.IsVisible = false;
        lblIncomeCategory.IsVisible = false;
        vLayoutIncomeCategory.IsVisible = false;

        btnExpense.TextColor = Color.FromArgb("#000");
        brExpense.Stroke = new SolidColorBrush(Color.FromArgb("#8458A4"));
        lblExpenseChart.IsVisible = true;
        hsLayoutExpense.IsVisible = true;
        lblExpenseCategory.IsVisible = true;
        vLayoutExpenseCategory.IsVisible = true;
    }
    private void btnIncome_Clicked(object sender, EventArgs e)
    {
        btnExpense.TextColor = Color.FromArgb("#888");
        brExpense.Stroke = new SolidColorBrush(Color.FromArgb("#888"));
        lblExpenseChart.IsVisible = false;
        hsLayoutExpense.IsVisible = false;
        lblExpenseCategory.IsVisible = false;
        vLayoutExpenseCategory.IsVisible = false;

        btnIncome.TextColor = Color.FromArgb("#000");
        brIncome.Stroke = new SolidColorBrush(Color.FromArgb("#8458A4"));
        lblIncomeChart.IsVisible = true;
        hsLayoutIncome.IsVisible = true;
        lblIncomeCategory.IsVisible = true;
        vLayoutIncomeCategory.IsVisible = true;
    }
    private void PickerBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        dateType = (DateType)PickerBy.SelectedIndex;
        lblDate.Text = DateToString(date);
        LoadExpenseAndIncome();
        LoadExpenseByCategory();
        LoadIncomeByCategory();
    }
    void Click()
    {
        TapGestureRecognizer BackClick = new TapGestureRecognizer();
        BackClick.Tapped += (s, e) =>
        {
            switch (dateType)
            {
                case DateType.Day:
                    date = date.AddDays(-1); break;
                case DateType.Week:
                    date = date.AddDays(-7); break;
                case DateType.Month:
                    date = date.AddMonths(-1); break;
                case DateType.Year:
                    date = date.AddYears(-1); break;
            }
            LoadDate();
            LoadExpenseAndIncome();
            LoadExpenseByCategory();
            LoadIncomeByCategory();
        };
        lblBackDate.GestureRecognizers.Add(BackClick);

        TapGestureRecognizer NextClick = new TapGestureRecognizer();
        NextClick.Tapped += (s, e) =>
        {
            switch (dateType)
            {
                case DateType.Day:
                    date = date.AddDays(1); break;
                case DateType.Week:
                    date = date.AddDays(7); break;
                case DateType.Month:
                    date = date.AddMonths(1); break;
                case DateType.Year:
                    date = date.AddYears(1); break;
            }
            LoadDate();
            LoadExpenseAndIncome();
            LoadExpenseByCategory();
            LoadIncomeByCategory();
        };
        lblNextDate.GestureRecognizers.Add(NextClick);
    }
    async void LoadExpenseByCategory()
    {
        DateTime start = date;
        DateTime end = date;
        switch (dateType)
        {
            case DateType.Day:
                start = new DateTime(date.Year, date.Month, date.Day);
                end = start.AddDays(1).AddSeconds(-1);
                break;
            case DateType.Week:
                DateTime dt = new DateTime(date.Year, date.Month, date.Day);
                start = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
                end = dt.AddDays(-(int)dt.DayOfWeek + 8).AddSeconds(-1);
                break;
            case DateType.Month:
                start = new DateTime(date.Year, date.Month, 1);
                end = start.AddMonths(1).AddSeconds(-1);
                break;
            case DateType.Year:
                start = new DateTime(date.Year, 1, 1);
                end = start.AddYears(1).AddSeconds(-1);
                break;
        }
        vLayoutExpenseCategory.Clear();
        var List = await clsTransaction.GetExpensesGroupByCategory(start, end);
        if(List != null)
        {            
            foreach (var item in List) 
            {
                ViewTransaction vt = new(item, true);
                vLayoutExpenseCategory.Children.Add(vt);
            }
        }
    }
    async void LoadIncomeByCategory()
    {
        DateTime start = date;
        DateTime end = date;
        switch (dateType)
        {
            case DateType.Day:
                start = new DateTime(date.Year, date.Month, date.Day);
                end = start.AddDays(1).AddSeconds(-1);
                break;
            case DateType.Week:
                DateTime dt = new DateTime(date.Year, date.Month, date.Day);
                start = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
                end = dt.AddDays(-(int)dt.DayOfWeek + 8).AddSeconds(-1);
                break;
            case DateType.Month:
                start = new DateTime(date.Year, date.Month, 1);
                end = start.AddMonths(1).AddSeconds(-1);
                break;
            case DateType.Year:
                start = new DateTime(date.Year, 1, 1);
                end = start.AddYears(1).AddSeconds(-1);
                break;
        }
        vLayoutIncomeCategory.Clear();
        var List = await clsTransaction.GetIncomeGroupByCategory(start, end);
        if (List != null)
        {
            foreach (var item in List)
            {
                ViewTransaction vt = new(item, true);
                vLayoutIncomeCategory.Children.Add(vt);
            }
        }
    }

    public static Action? Refresh;
}