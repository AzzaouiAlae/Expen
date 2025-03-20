namespace Expen.View;

public partial class ViewEditeDateTime : ContentView
{
	public ViewEditeDateTime(DateTime date)
	{
		InitializeComponent();
		_date = date;
        pikDate.Date = _date;
        pikTime.Time = _date.TimeOfDay;
    }

	DateTime _date;

    public event Action<DateTime>? DateChanged;

    public Action? Cancel;
    private void btnOk_Clicked(object sender, EventArgs e)
    {
        DateOnly DO = new DateOnly(pikDate.Date.Year, pikDate.Date.Month, pikDate.Date.Day);
        TimeOnly to = new TimeOnly(pikTime.Time.Hours, pikTime.Time.Minutes, pikTime.Time.Seconds);        
        _date = new DateTime(DO, to);
        DateChanged?.Invoke(_date);
        Cancel?.Invoke();
    }
    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        Cancel?.Invoke();
    }
}