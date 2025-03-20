

namespace Expen;

public partial class ViewTransaction : ContentView
{
    public ViewTransaction(clsTransaction Transaction, bool IsGroupe = false)
    {
        InitializeComponent();
        _Transaction = Transaction;

        if(IsGroupe)
            LoadGroupe();
        else
            Load();
    }
    async void LoadGroupe()
    {
        lblDescription.IsVisible = false;
        frmWallet.IsVisible = false;
        await _Transaction.LoadCategory();
        string incomOrExpen = "+ ";
        if (_Transaction.Category != null)
        {
            imgCategory.Source = _Transaction.Category.imgURL;
            lblCategory.Text = _Transaction.Category.Name;
            frmCategory.Background = new SolidColorBrush(Color.FromArgb(_Transaction.Category.color));
            if (_Transaction.Category.Type == 1)            
                lblPrice.TextColor = Colors.Blue;
            
            else if (_Transaction.Category.Type == 0)
            {
                incomOrExpen = "- ";
                lblPrice.TextColor = Colors.Red;
            }
        }
        lblPrice.Text = incomOrExpen + _Transaction.Amount.ToString("0.00");
    }    

    clsTransaction _Transaction;
    async void Load()
	{
        await _Transaction.LoadCategory();
        string incomOrExpen = "+ ";
        if (_Transaction.Category != null)
        {
            imgCategory.Source = _Transaction.Category.imgURL;
            lblCategory.Text = _Transaction.Category.Name;
            lblDescription.Text = _Transaction.Category.Description;
            frmCategory.Background = new SolidColorBrush(Color.FromArgb(_Transaction.Category.color));
            if(_Transaction.Category.Type == 1)            
                lblPrice.TextColor = Colors.Blue;
            
            else if (_Transaction.Category.Type == 0)
            {
                incomOrExpen = "- ";
                lblPrice.TextColor = Colors.Red;
            }
            else if (_Transaction.Category.Type == 2)            
                lblPrice.TextColor = Colors.Green;
            
        }

        await _Transaction.LoadWallet();

        if (_Transaction.wallet != null)
        {
            frmWallet.Background = new SolidColorBrush(Color.FromArgb(_Transaction.wallet.color));
            lblWalletName.Text = _Transaction.wallet.Name;
            WalletIcon.Source = _Transaction.wallet.imgURL;
        }

        lblPrice.Text = incomOrExpen + _Transaction.Amount.ToString("0.00");
        Click();
    }
    void Click()
    {
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped += async (s, e) =>
        {
            clsTransaction t = new clsTransaction(_Transaction);
            if (_Transaction.Type != 2)
            {                
                await Navigation.PushAsync(new PageTransactionAddEdit(t));
            }
            else
                await Navigation.PushAsync(new PageTransfer(t));
            
        };
        frm.GestureRecognizers.Add(click);
    }
}