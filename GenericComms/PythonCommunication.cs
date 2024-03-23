using System.Net.Http.Json;
using System.Text.Json;
namespace GenericComms;
internal class PythonCommunications<T>{
    
    public string Endpoint {get; init;}
    private T? _response = default(T);
    private Exception? _exception = null;

    private PythonCommunications(string endpoint){            
        Endpoint = endpoint;
    }

    public static PythonCommunications<T> GetConnection(string endpoint)
        => new(endpoint);


    public async Task<PythonCommunications<T>> PerformCommandAsync(string method, T? requestBody = default(T))
      => await DoAsync(CallPythonCommandLine, method,requestBody);
    public async Task<PythonCommunications<T>> PerformRequestAsync(string method, T requestBody)
      => await DoAsync(CallPythonCommandLine, method,requestBody);
    public async Task<PythonCommunications<T>> PerformGetAsync(string method, T? requestBody = default(T))
      => await DoAsync(CallPythonRest, method,requestBody, "Get");
    public async Task<PythonCommunications<T>> PerformPostAsync(string method, T requestBody)
      => await DoAsync(CallPythonRest, method,requestBody,"Post");
    

    public T? Response{
        get =>  _response; private set{Response = value;}
    }

    public bool HasValues{
        get => _response is not null;
    }

    public bool IsFaulted{
        get => _exception is not null;
    }
    public string FaultMessage{
        get => _exception?.Message ?? "No fault";
    }


    private async Task<T> CallPythonRest(string method, T? requestBody, string callType)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri(Endpoint);
        JsonContent? JSONRequest = JsonContent.Create(JsonSerializer.Serialize(requestBody));
        HttpResponseMessage response = callType switch{
            "Get" => await client.GetAsync(method),
            "Post" => await client.PostAsync(method, JSONRequest),
            _ => new HttpResponseMessage()
        };
        if (response.IsSuccessStatusCode)
             return await response.Content.ReadFromJsonAsync<T>() ?? throw new HttpRequestException("Response is null!");
        throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
    }

    private Task<T> CallPythonCommandLine(string method, T? requestBody,string callType ="")
    {
        throw new NotImplementedException();
    }

    private async Task<PythonCommunications<T>> DoAsync(Func<string,T?,string,Task<T>> function, string method, T? requestBody, string callType = ""){
        try{
            _response = await function(method,requestBody,callType);
        }catch(Exception ex){
            _exception = ex;
        }
        return this;
    }
}