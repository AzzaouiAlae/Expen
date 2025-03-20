using SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Expen;

public partial class PageCategoryAddNew : ContentPage
{
	public PageCategoryAddNew()
	{
		InitializeComponent();
        Click();
    }
    FileResult? photo;
    void Click()
    {
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped += async (s, e) => {
            await Navigation.PopAsync();
        };
        lblBack.GestureRecognizers.Add(click);

        TapGestureRecognizer click1 = new TapGestureRecognizer();
        click1.Tapped += async (s, e) =>
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                    imgCategory.Source = photo.FullPath;
            }
        };
        imgCategory.GestureRecognizers.Add(click1);
    }

    clsCategories Category = new clsCategories();
    private void btnExpense_Clicked(object sender, EventArgs e)
    {
        Category.Type = 0;
        btnExpense.Background = Color.FromArgb("#D2BBF2");
        btnIncome.Background = Color.FromArgb("#FFF");
    }
    private void btnIncome_Clicked(object sender, EventArgs e)
    {
        Category.Type = 1;
        btnExpense.Background = Color.FromArgb("#FFF");
        btnIncome.Background = Color.FromArgb("#D2BBF2");
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        Category.color = ((Button)sender).AutomationId;
        foreach (var item in hLayout.Children)
        {
            ((Button)item).BorderWidth = 5;
            ((Button)item).BorderColor = Color.FromArgb(Category.color);
        }

        frm.Background = new SolidColorBrush(Color.FromArgb(Category.color));

        if (Category.color == "#fff")
            ((Button)sender).BorderWidth = 0;
        else
        {
            ((Button)sender).BorderWidth = 5;
            ((Button)sender).BorderColor = Color.FromArgb("#fff");
        }
    }
    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        Category.Name = txtName.Text;
        Category.Description = txtDescription.Text;

        string newURL = FileSystem.AppDataDirectory + "\\" + DateTime.Now.ToString("mm-ss") + photo.FileName;
        File.Copy(photo.FullPath, newURL);
        Category.imgURL = newURL;

        if (await Category.Save()) {
            await DisplayAlert("Saved", "Category Saved successfully", "OK");
            await Navigation.PopAsync();
        }
        else {
            await DisplayAlert("Failed", "failed", "OK");
        }
    }
}