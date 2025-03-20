namespace Expen;

public partial class PageStrat : ContentPage
{
	public PageStrat()
	{
		InitializeComponent();

        Click();
        Profile = new clsProfile(1, () =>  LoadProfile() );
    }

    clsProfile Profile;

    FileResult? photo;

    async Task LoadProfile()
    {
        if (File.Exists(Profile.imgURL))
            imgProfile.Source = Profile.imgURL;

        txtName.Text = Profile.Name;
    }
    void Click()
    {
        TapGestureRecognizer click = new TapGestureRecognizer();
        click.Tapped += async (s, e) =>
        {
            if(MediaPicker.Default.IsCaptureSupported)
            {
                photo = await MediaPicker.Default.PickPhotoAsync();
                if(photo != null)                
                    imgProfile.Source = photo.FullPath;
            }
        };
        imgProfile.GestureRecognizers.Add(click); 
    }
    private async void btnCreate_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (photo != null)
            {
                if (File.Exists(Profile.imgURL))
                    File.Delete(Profile.imgURL);
                string newURL = FileSystem.AppDataDirectory + "\\" + DateTime.Now.ToString("mm-ss") + photo.FileName;                
                File.Copy(photo.FullPath, newURL);
                Profile.imgURL = newURL;
            }
            Profile.Name = txtName.Text;
            await Profile.Save();
            await Shell.Current.GoToAsync("//PageHome");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }        
    }
}