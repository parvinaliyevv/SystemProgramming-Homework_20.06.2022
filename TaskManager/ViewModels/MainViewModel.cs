namespace TaskManager.ViewModels;

public class MainViewModel: DependencyObject
{
    public ObservableCollection<Process> Processes
    {
        get { return (ObservableCollection<Process>)GetValue(ProcessesProperty); }
        set { SetValue(ProcessesProperty, value); }
    }
    public static readonly DependencyProperty ProcessesProperty =
        DependencyProperty.Register("Processes", typeof(ObservableCollection<Process>), typeof(MainViewModel));

    public Process? SelectedProcess { get; set; }

    public RelayCommand SearchProcessCommand { get; set; }
    public RelayCommand CreateProcessCommand { get; set; }
    public RelayCommand EndProcessCommand { get; set; }

    public string SearchProcessValue
    {
        get { return (string)GetValue(SearchProcessValueProperty); }
        set { SetValue(SearchProcessValueProperty, value); }
    }
    public static readonly DependencyProperty SearchProcessValueProperty =
        DependencyProperty.Register("SearchProcessValue", typeof(string), typeof(MainViewModel));

    public string ProcessTitleValue
    {
        get { return (string)GetValue(ProcessTitleValueProperty); }
        set { SetValue(ProcessTitleValueProperty, value); }
    }
    public static readonly DependencyProperty ProcessTitleValueProperty =
        DependencyProperty.Register("ProcessTitleValue", typeof(string), typeof(MainViewModel));


    public MainViewModel()
    {
        Processes = new(Process.GetProcesses());
        SelectedProcess = null;

        SearchProcessCommand = new(sender => SearchProcess());
        CreateProcessCommand = new(sender => CreateProcess(), sender => !string.IsNullOrWhiteSpace(ProcessTitleValue));
        EndProcessCommand = new(sender => EndProcess(), sender => SelectedProcess is not null);

        ProcessTitleValue = string.Empty;


        var refresher = new DispatcherTimer();

        refresher.Interval = TimeSpan.FromSeconds(5);
        refresher.Tick += delegate (object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchProcessValue))
                Processes = new(Process.GetProcesses());
        };

        refresher.Start();
    }


    public void SearchProcess()
    {
        if (string.IsNullOrWhiteSpace(SearchProcessValue)) Processes = new(Process.GetProcesses());
        else
        {
            var newItems = new ObservableCollection<Process>();

            foreach (var item in Process.GetProcesses())
                if (item.ProcessName.Contains(SearchProcessValue)) newItems.Add(item);

            Processes = newItems;
        }
    }

    public void CreateProcess()
    {
        try
        {
            Processes.Add(Process.Start(ProcessTitleValue));
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            ProcessTitleValue = string.Empty;
        }
    }

    public void EndProcess()
    {
        try
        {
            SelectedProcess?.Kill();
            Processes.Remove(SelectedProcess);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
