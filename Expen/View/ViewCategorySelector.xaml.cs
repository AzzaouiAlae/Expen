namespace Expen;

public partial class ViewCategorySelector : ContentView
{
	public enum enMode { Expense, Income, Transfer }
	public enMode Mode { get; set; } = enMode.Expense;
    public ViewCategorySelector()
	{
		InitializeComponent();
	}

	List<clsCategories>? Categories = new List<clsCategories>();
    public VerticalStackLayout sl { get { return SL; } set { SL = value; } }
    public async void LoadCategories()
    {
        vLayout.Children.Clear();
        Categories = await clsCategories.GetAllByType((int)Mode);
        if (Categories == null) return;

        foreach (clsCategories c in Categories)
        {
            ViewSelectedCategory vsc = new(c);
            vsc.Mode = ViewSelectedCategory.enMode.Select;
            vLayout.Children.Add(vsc);
        }
    }
}