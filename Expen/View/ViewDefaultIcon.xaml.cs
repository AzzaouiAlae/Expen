namespace Expen;

public partial class ViewDefaultIcon : ContentView
{
	public ViewDefaultIcon()
	{
		InitializeComponent();
	}

    public event Action<ImageSource>? IconChanged;

    public Action? Cancel;

    private void btnOk_Clicked(object sender, EventArgs e)
    {        
        IconChanged?.Invoke(((ImageButton)sender).Source);
        Cancel?.Invoke();
    }

    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        Cancel?.Invoke();
    }
}