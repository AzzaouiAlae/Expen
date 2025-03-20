namespace Expen
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new(new AppShell());

            window.Width = 350;
            window.Height = 650;

            return window;
        }
    }
}