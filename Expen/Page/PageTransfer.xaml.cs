using System;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace Expen;

public partial class PageTransfer : ContentPage
{
    public PageTransfer()
    {
        InitializeComponent();
        Load();
        Click();
    }
    public PageTransfer(clsTransaction t)
    {
        InitializeComponent();
        Load();
        Click();
        Load(t);
    }
    void Load()
    {
        transaction.Type = 2;
        LoadDateTime();
    }   

    clsTransaction transaction = new();
    private void btnEditDateTime_Clicked(object sender, EventArgs e)
    {
        myBorder.IsEnabled = false;
        View.ViewEditeDateTime vDT = new(transaction.Date);
        StackLayout sl = new() {
            Background = new SolidColorBrush(Color.FromArgb("#AAA")),
            Opacity = 0.5 };
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped += (s, e) => {
            myBorder.IsEnabled = true;
            myGrid.Remove(vDT);
            myGrid.Remove(sl); };
        sl.GestureRecognizers.Add(click);

        vDT.Cancel = () => {
            myBorder.IsEnabled = true;
            myGrid.Remove(vDT);
            myGrid.Remove(sl); };
        vDT.VerticalOptions = LayoutOptions.Center;
        vDT.DateChanged += (DT) => {
            transaction.Date = DT;
            lblDateTime.Text = DT.ToString("dd MMM yyyy hh:mm:ss"); };

        myGrid.Add(sl);
        myGrid.Add(vDT);
    }
    private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckNumber((Entry)sender, e);

        if (float.TryParse(((Entry)sender).Text, out float num))
        {
            transaction.Amount = num;
        }
    }
    void CheckNumber(Entry txt, TextChangedEventArgs e)
    {
        if (txt.Text == null) return;
        if (txt.Text == "") return;
        float num = 0;

        if (!float.TryParse(txt.Text, out num))
            txt.Text = e.OldTextValue;
    }
    
    void Click()
    {
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped +=  (s, e) =>
        {
            Navigation.PopAsync();
        };
        lblBack.GestureRecognizers.Add(click);

        TapGestureRecognizer click1 = new TapGestureRecognizer();
        click1.Tapped += (s, e) =>
        {
            Navigation.PopToRootAsync();
        };
        lblCancel.GestureRecognizers.Add(click1);
    }
    void LoadDateTime()
    {
        DateTime dt = DateTime.Now;
        transaction.Date = dt;
        lblDateTime.Text = dt.ToString("dd MMM yyyy hh:mm:ss");
    }
    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        transaction.WalletID = ViewWalletSelected1.Wallet.ID; 
        transaction.ToWalletID = ViewWalletSelected2.Wallet.ID; 

        transaction.Type = 2;
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
    async void Load(clsTransaction t)
    {        
        transaction = t;
        await transaction.LoadWallet();
        await transaction.LoadCategory();

        await Task.Delay(50);
        btnDelete.IsVisible = true;        
        lblDateTime.Text = transaction.Date.ToString("dd MMM yyyy hh:mm:ss");

        await Task.Delay(50);

        if(transaction.wallet != null)
            ViewWalletSelected1.Wallet = transaction.wallet;

        if (transaction.ToWallet != null)
            ViewWalletSelected2.Wallet = transaction.ToWallet;
        
        txtAmount.Text = transaction.Amount.ToString("0.00");
           
    }
    private void ViewWalletSelected1_Loaded(object sender, EventArgs e)
    {
        ViewWalletSelected1.LoadGrid(myGrid, 1, 1);
        ViewWalletSelected2.LoadGrid(myGrid, 1, 1);
    }
    
}