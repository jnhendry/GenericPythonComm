using System.Net.Http.Json;
using System.Text.Json;
namespace GenericComms;
//
// Summary:
//    Generic class to handle calls to and from Python Processes
internal class PythonCommunications<TResult>{
    private const string get = "Get";
    private const string post = "Post";
    private string _endpoint {get; init;}

    private string _targetPath{get; init;}
    private TResult? _response = default;
    private Exception? _exception = null;

    private PythonCommunications(string endpoint) {
        _targetPath = string.Empty;
        _endpoint = endpoint;
    }


    private PythonCommunications(string codeFile, string pythonRuntime) {
        _endpoint = string.Empty;
        pythonRuntime = string.IsNullOrEmpty(pythonRuntime)
                            ? "Python.exe"
                            : pythonRuntime;
        _targetPath = $"{pythonRuntime} {codeFile}";
    }
    //
    // Summary:
    //     Create a new communication object with the target endpoint
    // Parameters:
    //   endpoint:
    //     The rest endpoint hosted from the python runtime
    // Returns:
    //     The task object representing the asynchronous operation.
    public static PythonCommunications<TResult> GetConnection(string endpoint)
        => new(endpoint);

    public static PythonCommunications<TResult> GetConnection(string codeFile,string pythonRuntime = "")
        => new(codeFile, pythonRuntime);

    public TResult? Response{
        get =>  _response; 
    }

    public bool HasValues{
        get => _response is not null;
    }

    public bool IsFaulted{
        get => _exception is not null;
    }
    public string? FaultMessage{
        get => _exception?.Message;
    }

    public async Task<PythonCommunications<TResult>> PerformCommandAsync<T>(string method, T? requestBody = default)
      => await DoAsync(CallPythonCommandLine, method,requestBody);
    public async Task<PythonCommunications<TResult>> PerformRequestAsync<T>(string method, T requestBody)
      => await DoAsync(CallPythonCommandLine, method,requestBody);
    public async Task<PythonCommunications<TResult>> PerformGetAsync<T>(string method, T? requestBody = default)
      => await DoAsync(CallPythonRest, method,requestBody, get);
    public async Task<PythonCommunications<TResult>> PerformPostAsync<T>(string method, T requestBody)
      => await DoAsync(CallPythonRest, method,requestBody, post);
    

    private async Task<TResult> CallPythonRest<T>(string method, T? requestBody, string callType)
    {
        
        HttpClient client = new();
        client.BaseAddress = new Uri(_endpoint);
        HttpResponseMessage response = callType switch{
            get => await client.GetAsync(method),
            post => await client.PostAsync(method, JsonContent.Create(JsonSerializer.Serialize<T>(
                                                                                requestBody is null 
                                                                                           ? throw new ArgumentException("Request body cannot be null") 
                                                                                           : requestBody))),
            _ => new HttpResponseMessage()
        };
        if (response.IsSuccessStatusCode){
            return await response.Content.ReadFromJsonAsync<TResult>() ?? throw new HttpRequestException("Response is null!");
        }
        throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
    }

    private Task<TResult> CallPythonCommandLine<T>(string method, T? requestBody,string callType ="")
    {
        throw new NotImplementedException();
    }

    private async Task<PythonCommunications<TResult>> DoAsync<T>(Func<string,T?,string,Task<TResult>> function, string method, T? requestBody, string callType = ""){
        try{
            _response = await function(method, requestBody, callType);
        }catch(Exception ex){
            _exception = ex;
        }
        return this;
    }
}