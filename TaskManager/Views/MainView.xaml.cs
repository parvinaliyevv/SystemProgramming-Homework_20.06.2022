namespace TaskManager.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();

        DataContext = new MainViewModel();
    }

    private void Windows_Loaded(object sender, RoutedEventArgs e)
    {
        Icon = ImageService.GetImageFromByteArray(TaskManager.Resources.Images.Icon);
    }
}
