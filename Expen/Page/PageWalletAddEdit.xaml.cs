using Microsoft.Maui;
using System.Xml.Linq;

namespace Expen;

public partial class PageWalletAddEdit : ContentPage
{
	public PageWalletAddEdit()
	{
        InitializeComponent();        
        wallet.color = "#fff";
        Click();
    }
    public PageWalletAddEdit(clsWallet w)
    {
        InitializeComponent();
        Click();
        loadForm(w);
    }

    FileResult? WalletIconFR;
    void Click()
    {
        TapGestureRecognizer WalletIconClick = new TapGestureRecognizer();
        WalletIconClick.Tapped += (s, e) =>
        {
            ViewDefaultIcon viewDefaultIcon = new ViewDefaultIcon();            
            StackLayout sl = new()
            {
                Background = new SolidColorBrush(Color.FromArgb("#AAA")),
                Opacity = 0.5
            };
            TapGestureRecognizer click = new TapGestureRecognizer();
            click.Tapped += (s, e) =>
            {
                myGrid.Remove(viewDefaultIcon);
                myGrid.Remove(sl);
            };
            sl.GestureRecognizers.Add(click);

            viewDefaultIcon.Cancel = () => {
                myGrid.Remove(viewDefaultIcon);
                myGrid.Remove(sl);
            };

            viewDefaultIcon.VerticalOptions = LayoutOptions.Center;
            viewDefaultIcon.IconChanged += (imgS) =>
            {
                WalletIcon.Source = imgS;

                if (imgS is FileImageSource fileImageSource)
                {
                    wallet.imgURL = fileImageSource.File;
                }
            };
            myGrid.Add(sl, 0,0);

            myGrid.Add(viewDefaultIcon, 0, 0);
        };
        WalletIcon.GestureRecognizers.Add(WalletIconClick);

    }
    public clsWallet wallet { get; set; } = new clsWallet();
    public void loadForm(clsWallet w)
    {
        wallet = w;
        lblTitle.Text = "Update Wallet";
        txtType.Text = wallet.Type;
        txtName.Text = wallet.Name;
        txtBalance.Text = wallet.Balance.ToString("0.00");
        btnDelete.IsVisible = true;
        if (wallet.color != "" && wallet.color != null)
            frm.Background = new SolidColorBrush(Color.FromArgb(wallet.color));
        else
            wallet.color = "#fff";

        if(wallet.imgURL != null && wallet.imgURL != "")
            WalletIcon.Source = wallet.imgURL;

        foreach (var item in hLayout.Children)
        {
            if (((Button)item).AutomationId != wallet.color)
            {
                ((Button)item).BorderWidth = 5;
                ((Button)item).BorderColor = Color.FromArgb(wallet.color);
            }
            else
            {
                if (wallet.color == "#fff")
                    ((Button)item).BorderWidth = 0;
                else
                {
                    ((Button)item).BorderWidth = 5;
                    ((Button)item).BorderColor = Color.FromArgb("#fff");
                }
            }
        }
    }
    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        float.TryParse(txtBalance.Text, out float balance);
        wallet.Type = txtType.Text;
        wallet.Name = txtName.Text;
        wallet.Balance = balance;

        if (WalletIconFR != null)
        {
            string newURL = FileSystem.AppDataDirectory + "\\" + DateTime.Now.ToString("mm-ss") + WalletIconFR.FileName;
            File.Copy(WalletIconFR.FullPath, newURL);
            wallet.imgURL = newURL;
        }

        await wallet.Save();
        await Navigation.PopAsync();
    }
    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        wallet.color = ((Button)sender).AutomationId;
        foreach (var item in hLayout.Children)
        {
            ((Button)item).BorderWidth = 5;
            ((Button)item).BorderColor = Color.FromArgb(wallet.color);
        }

        frm.Background = new SolidColorBrush(Color.FromArgb(wallet.color));

        if (wallet.color == "#fff")        
            ((Button)sender).BorderWidth = 0;
        else
        {
            ((Button)sender).BorderWidth = 5;
            ((Button)sender).BorderColor = Color.FromArgb("#fff");
        }      
    }
    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        await wallet.Delete();
    }
}