namespace Zeiterfassung.Pages;

partial class Home
{
    public string UUID { get; set; } = "";

    public string ServerUrl
    {
        get => serverUrl;
        set
        {
            serverUrl = value;

            if (!initialized)
                return;

            localStorage.SetItemAsStringAsync("SERVER_URL", serverUrl).AsTask().ContinueWith(async x =>
            {
                await x;

                if (!string.IsNullOrWhiteSpace(serverUrl))
                {
                    api.Url = serverUrl;
                    serverOnline = await api.ServerAvailable();
                    StateHasChanged();
                }
            });
        }
    }
    private bool serverOnline = false;
    private string serverUrl = "";

    public bool ServerReachable => !string.IsNullOrEmpty(ServerUrl) && serverOnline;
    public bool RegisterDisabled => !string.IsNullOrWhiteSpace(UUID) || !ServerReachable;

    public bool AlarmOn { get; set; }

    private bool initialized = false;

    protected override async void OnInitialized()
    {
        base.OnInitialized();

        ServerUrl = await localStorage.GetItemAsStringAsync("SERVER_URL") ?? "";
        UUID = await localStorage.GetItemAsStringAsync("UUID") ?? "";

        if (!string.IsNullOrWhiteSpace(ServerUrl))
        {
            api.Url = ServerUrl;
            serverOnline = await api.ServerAvailable();
        }

        if (ServerReachable)
        {
            var isRunning = await api.IsRunning(UUID);
            Console.WriteLine("IsRunning: " + isRunning);
            AlarmOn = isRunning;
        }

        Console.WriteLine("ServerReachable: " + ServerReachable);
        Console.WriteLine("Loaded: " + UUID);

        initialized = true;
        StateHasChanged();
    }

    private async void Register()
    {
        UUID = await api.Register(CancellationToken.None) ?? "";
        await localStorage.SetItemAsStringAsync("UUID", UUID);
        Console.WriteLine("Registered: " + UUID);
        StateHasChanged();
    }

    public async void OnToggledChanged(bool toggled)
    {
        // Because variable is not two-way bound, we need to update it ourselves.
        AlarmOn = toggled;
        var cancellationToken = CancellationToken.None;

        if (AlarmOn)
        {
            Console.WriteLine("START:" + UUID);
            await api.Start(UUID, cancellationToken);
        }
        else
        {
            Console.WriteLine("STOP:" + UUID);
            await api.Stop(UUID, cancellationToken);
        }
    }
}
