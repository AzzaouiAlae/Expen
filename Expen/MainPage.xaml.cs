


namespace Expen
{

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Load();
        }
        void LoadClickToImg()
        {
            TapGestureRecognizer Click1 = new ();
            Click1.Tapped += (s, e) =>
            {
                Index--;
            };
            img_left.GestureRecognizers.Add(Click1);

            TapGestureRecognizer Click2 = new ();
            Click2.Tapped += (s, e) =>
            {
                Index++;
            };
            img_Right.GestureRecognizers.Add(Click2);
        }       
        async Task LoadFram()
        {
            frm1.HeightRequest = Size;
            frm1.WidthRequest = Size;
            frm1.CornerRadius = (float)Size;
            frm1.Margin = new(0, -Size * 0.838);

            frm2.HeightRequest = Size;
            frm2.WidthRequest = Size;
            frm2.CornerRadius = (float)Size;
            frm2.Margin = new(Size * 0.179);                        
        }

        double Size = 0;
        async Task Load()
        {
            LoadClickToImg();

            while (true)
            {
                await Task.Delay(30);
                if (absLayout.Height <= 0)
                    continue;

                imgClip.Rect = new Rect(0, 0, img.Width, img.Height);

                await Task.Delay(100);

                if (absLayout.Height < absLayout.Width)
                    Size = absLayout.Width;
                else
                    Size = absLayout.Height;

                LoadFram();
                
                break;
            }
            bool Result = await clsProfile.isProfileExists();
            if (Result)
                await Shell.Current.GoToAsync("//PageHome");
        }

        int _index = 0;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;

                switch (_index)
                {
                    case 0:
                        lbl1.IsVisible = true;
                        lbl2.IsVisible = false;
                        img.Source = "photoa.png";
                        img_left.IsVisible = false;

                        frm1.Margin = new(0, -Size * 0.83);
                        frm2.Margin = new(Size * 0.17);
                        break;

                    case 1:
                        lbl1.IsVisible = false;
                        lbl2.IsVisible = true;
                        lbl3.IsVisible = false;
                        img.Source = "photob.png";
                        img_left.IsVisible = true;

                        frm1.Margin = new(absLayout.Width * .50, -Size * 0.83);
                        frm2.Margin = new(absLayout.Width * .50, Size * 0.18);
                        break;

                    case 2:                        
                        lbl2.IsVisible = false;
                        lbl3.IsVisible = true;
                        lbl4.IsVisible = false;
                        img.Source = "photoc.png";
                        img_Right.IsVisible = true;

                        frm1.Margin = new(absLayout.Width * .30, -Size * 0.83);
                        frm2.Margin = new(Size * 0.17);
                        break;

                    case 3:
                        lbl3.IsVisible = false;
                        lbl4.IsVisible = true;
                        img.Source = "photod.png";
                        img_Right.IsVisible = false;

                        frm1.Margin = new(absLayout.Width * .25, absLayout.Height - Size - (absLayout.Height / 2.8));
                        frm2.Margin = new(0, absLayout.Height * 0.65);
                        break;
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            bool Result = await clsProfile.isProfileExists();
            if (Result)
                await Shell.Current.GoToAsync("//PageHome");
            else
                await Shell.Current.GoToAsync("//PageStrat");
        }
    }
}