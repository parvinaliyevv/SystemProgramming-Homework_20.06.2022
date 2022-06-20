namespace TaskManager.ViewModels;

public class MainViewModel
{
    public ObservableCollection<Process> Processes { get; set; }
    public Process? SelectedProcess { get; set; }

    public RelayCommand CreateProcessCommand { get; set; }
    public RelayCommand EndProcessCommand { get; set; }

    public string ProcessTitleValue { get; set; }


    public MainViewModel()
    {
        Processes = new(Process.GetProcesses());
        SelectedProcess = null;

        CreateProcessCommand = new(sender => CreateProcess(), sender => !string.IsNullOrWhiteSpace(ProcessTitleValue));
        EndProcessCommand = new(sender => EndProcess(), sender => SelectedProcess is not null);

        ProcessTitleValue = string.Empty;
    }


    public void CreateProcess()
    {
        try
        {
            Processes.Add(Process.Start(ProcessTitleValue));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
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
