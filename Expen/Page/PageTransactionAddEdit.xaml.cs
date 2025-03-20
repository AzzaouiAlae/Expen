

namespace Expen;

public partial class PageTransactionAddEdit : ContentPage
{
    public PageTransactionAddEdit()
    {
        InitializeComponent();
        Click();
        Load();
    }
    public PageTransactionAddEdit(clsTransaction T)
	{
		InitializeComponent();
        transaction = T;
        Click();
        LoadObj();
    }            
    void LoadDateTime()
    {
        DateTime dt = DateTime.Now;
        transaction.Date = dt;
        lblDateTime.Text = dt.ToString("dd MMM yyyy hh:mm:ss");
    }
	public enum enMode
	{
		enAdd,
		enUpdate	
	}
    public enum enTransaction
    {
        enExpense,
        enIncome,
		enTransfer
    }
	public enMode Mode { get; set; }    
    public enTransaction Transaction {
        get
        {
            return _Transaction;
        }
        set
        {
            _Transaction = value;
        }
    }
	void Click()
	{
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped += async (s, e) =>
        {
            await Navigation.PopAsync();
        };
        lblBack.GestureRecognizers.Add(click);

        TapGestureRecognizer click1 = new TapGestureRecognizer();
        click1.Tapped += async (s, e) =>
        {
            await Navigation.PopToRootAsync();
        };
        lblCancel.GestureRecognizers.Add(click1);

        TapGestureRecognizer click2 = new TapGestureRecognizer();
        click2.Tapped += async (s, e) =>
        {
            await Navigation.PushAsync(new PageWalletAddEdit());
        };
        frmNewWallet.GestureRecognizers.Add(click2);

        TapGestureRecognizer click3 = new TapGestureRecognizer();
        click3.Tapped += async (s, e) =>
        {
            await Navigation.PushAsync(new PageCategoryAddNew());
        };
        frmNewCategory.GestureRecognizers.Add(click3);
    }
    void Load()
    {
        string title = "";
        switch(Mode)
        {
            case enMode.enAdd:
                title = "Add New ";
                break;
            case enMode.enUpdate:
                title = "Update ";
                break;
        }
        switch(Transaction)
        {
            case enTransaction.enExpense:
                title += "Expense";
                break;
            case enTransaction.enIncome:
                title += "Income";
                break;
            case enTransaction.enTransfer:
                title += "Transfer";
                break;
        }
        lblTitle.Text = title;    
        LoadDateTime();       
    }
    async void LoadObj()
    {
        btnDelete.IsVisible = true;
        await transaction.LoadWallet();
        await transaction.LoadCategory();
        Mode = enMode.enUpdate;
        Transaction = (enTransaction)transaction.Type;

        await Task.Delay(50);
        Load();

        await Task.Delay(50);
        if(transaction.wallet !=  null)        
            vSelecteW.Wallet = transaction.wallet;

        if(transaction.Category != null)
            vSelectedCategory.Category = transaction.Category;

        txtAmount.Text = transaction.Amount.ToString("0.00");
    }
    private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(txtAmount.Text == null) return;
        if(txtAmount.Text == "") return;
        float num = 0;

        if (!float.TryParse(txtAmount.Text, out num))
            txtAmount.Text = e.OldTextValue;
        transaction.Amount = num;
    }     
     
    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        transaction.WalletID = vSelecteW.Wallet.ID;
        transaction.CategoryID = vSelectedCategory.Category.ID; 
        transaction.Type = (byte)_Transaction;
        if (await transaction.Save())
        {
            await DisplayAlert("Saved", "Transaction Saved successfully", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            string error = "failed";
            if (clsTransaction.Log != "")
                error = clsTransaction.Log;

            await DisplayAlert("Failed", error, "OK");
        }
    }
    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        await clsTransaction.Delete(transaction.ID);
        await Navigation.PopAsync();
    }
    private void btnEditDateTime_Clicked(object sender, EventArgs e)
    {
        myBorder.IsEnabled = false;
        View.ViewEditeDateTime vDT = new(transaction.Date);
        StackLayout sl = new()
        {
            Background = new SolidColorBrush(Color.FromArgb("#AAA")),
            Opacity = 0.5
        };
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped +=  (s, e) =>
        {
            myBorder.IsEnabled = true;
            myGrid.Remove(vDT);
            myGrid.Remove(sl);
        };
        sl.GestureRecognizers.Add(click);
        
        vDT.Cancel = () => { 
            myBorder.IsEnabled = true; 
            myGrid.Remove(vDT);
            myGrid.Remove(sl);
        };
        vDT.VerticalOptions = LayoutOptions.Center;

        vDT.DateChanged += (DT) =>
        {
            transaction.Date = DT;
            lblDateTime.Text = DT.ToString("dd MMM yyyy hh:mm:ss");
        };
        
        myGrid.Add(sl);

        myGrid.Add(vDT);
        
    }
    private void ViewSelectedWallet_Loaded(object sender, EventArgs e)
    {
        vSelecteW.LoadGrid(myGrid, 1, 1);         
    }    

    static enTransaction _Transaction;   


    public clsTransaction transaction = new();

    private void ViewSelectedCategory_Loaded(object sender, EventArgs e)
    {
        vSelectedCategory.LoadGrid(myGrid, 1, 1);
        vSelectedCategory.SelectorMode = (ViewCategorySelector.enMode)_Transaction;
    }
}