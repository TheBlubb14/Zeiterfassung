using System.Net.Http.Json;

public class ApiClient(HttpClient http)
{
    private string url = "";

    public string Url
    {
        get => url;
        set
        {
            url = value.TrimEnd('/');
            Console.WriteLine("Set url to: " + url);
        }
    }

    public async Task<bool> ServerAvailable()
    {
        if (string.IsNullOrWhiteSpace(Url))
        {
            Console.WriteLine("Server unavailable");
            return false;
        }

        try
        {
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            _ = await http.GetStringAsync($"{Url}/User/Ping", cts.Token);
            Console.WriteLine("Server available");
            return true;
        }
        catch
        {
            Console.WriteLine("Server unavailable");
            return false;
        }
    }

    public async Task<string?> Register(CancellationToken cancellationToken = default)
    {
        using var response = await http.PostAsync($"{Url}/User/Register", content: null, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<string>(cancellationToken);
    }

    public async Task<DateTime> Start(string uuid, CancellationToken cancellationToken = default)
    {
        http.DefaultRequestHeaders.Remove("X-Api-Key");
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", uuid);
        using var response = await http.PostAsync($"{Url}/Tracking/Start", content: null, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DateTime>(cancellationToken);
    }

    public async Task<DateTime> Stop(string uuid, CancellationToken cancellationToken = default)
    {
        http.DefaultRequestHeaders.Remove("X-Api-Key");
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", uuid);
        using var response = await http.PostAsync($"{Url}/Tracking/Stop", content: null, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DateTime>(cancellationToken);
    }

    public async Task<bool> IsRunning(string uuid, CancellationToken cancellationToken = default)
    {
        http.DefaultRequestHeaders.Remove("X-Api-Key");
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", uuid);
        return bool.TryParse(await http.GetStringAsync($"{Url}/Tracking/IsRunning", cancellationToken), out var result) && result;
    }

    public Task<List<Tracking>?> GetAll(string uuid, CancellationToken cancellationToken = default)
    {
        http.DefaultRequestHeaders.Remove("X-Api-Key");
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", uuid);
        return http.GetFromJsonAsync<List<Tracking>>($"{Url}/Tracking/GetAll", cancellationToken);
    }

    public Task<List<Tracking>?> GetAll(string uuid, DateOnly? from, DateOnly? to, CancellationToken cancellationToken = default)
    {
        http.DefaultRequestHeaders.Remove("X-Api-Key");
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", uuid);
        return http.GetFromJsonAsync<List<Tracking>>($"{Url}/Tracking/GetAll/{from}/{to}", cancellationToken);
    }
}
