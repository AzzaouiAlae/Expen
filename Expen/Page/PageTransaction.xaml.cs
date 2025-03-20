namespace Expen;

public partial class PageTransaction : ContentPage
{
	public PageTransaction()
	{
		InitializeComponent();
        Click();
    }

    void Click()
    {        
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped += async (s, e) =>
        {
            await Navigation.PushAsync(new PageTransactionAddEdit() 
            { Mode = PageTransactionAddEdit.enMode.enAdd, Transaction = PageTransactionAddEdit.enTransaction.enExpense });
        };
        frmExpense.GestureRecognizers.Add(click);

        TapGestureRecognizer click2 = new TapGestureRecognizer();
        click2.Tapped += async (s, e) =>
        {
            await Navigation.PushAsync(new PageTransactionAddEdit() 
            { Mode = PageTransactionAddEdit.enMode.enAdd, Transaction = PageTransactionAddEdit.enTransaction.enIncome }); 
        };
        frmIncome.GestureRecognizers.Add(click2);

        TapGestureRecognizer click3 = new TapGestureRecognizer();
        click3.Tapped += async (s, e) =>
        {
            await Navigation.PushAsync(new PageTransfer());            
        };
        frmTransfer.GestureRecognizers.Add(click3);
    }
}