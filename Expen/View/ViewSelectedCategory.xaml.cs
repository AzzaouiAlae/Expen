namespace Expen;

public partial class ViewSelectedCategory : ContentView
{
    public enum enMode { Show, Select }

    public enMode Mode = enMode.Show;
    public ViewCategorySelector.enMode SelectorMode { 
        get{ return vCategorySelector.Mode; } 
        set { vCategorySelector.Mode = value; Load(); } }
    public ViewSelectedCategory()
    {
        InitializeComponent();
        Click();
        vStackLayout();
        Load();
    }
    public ViewSelectedCategory(clsCategories c)
    {
        InitializeComponent();
        Click();
        Category = c;
    }
    public void vStackLayout()
    {
        TapGestureRecognizer slClick = new();
        slClick.Tapped += (s, e) =>
        {
            if (myGrid != null)
                myGrid.Remove(vCategorySelector);
        };
        vCategorySelector.sl.GestureRecognizers.Add(slClick);

    }
    public void Click()
    {
        TapGestureRecognizer brClick = new();
        brClick.Tapped += (s, e) =>
        {
            if (Mode == enMode.Show)
            {
                if (myGrid != null)
                {                    
                    AcLoadCategory = LoadCategory;
                    vCategorySelector.LoadCategories();
                    myGrid.AddWithSpan(vCategorySelector, 0, 0, Row, Col);                    
                }
            }
            else
                AcLoadCategory?.Invoke(Category);
        };
        br.GestureRecognizers.Add(brClick);
    }
    public clsCategories Category
    {
        get { return _Category; }
        set
        {
            _Category = value;
            img.Source = _Category.imgURL;
            lblName.Text = _Category.Name;
            br.Background = Color.FromArgb(_Category.color);
        }
    }
    void LoadCategory(clsCategories c)
    {
        Category = c;
        if (myGrid != null)
            myGrid.Remove(vCategorySelector);
        CategoryChange?.Invoke(Category);
    }
    public void LoadGrid(Grid g, int row, int col)
    {
        myGrid = g;
        Row = row;
        Col = col;
    }
    async void Load()
    {
        List<clsCategories>? Categories = await clsCategories.GetAllByType((int)SelectorMode);
        if (Categories != null && Categories.Count > 0)        
            Category = Categories[0];        
    }

    clsCategories _Category = new();   

    Grid? myGrid;

    int Row = 1;

    int Col = 1;    

    static ViewCategorySelector vCategorySelector = new();    

    static Action<clsCategories>? AcLoadCategory;

    public event Action<clsCategories>? CategoryChange;
}