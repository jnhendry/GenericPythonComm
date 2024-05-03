using System.Text.Json;
using GenericComms;
var response = await PythonCommunications<WeatherApiResponse>.GetConnection("http://localhost:30238/")
                                                             .PerformPostAsync("Weather", new WeatherApiRequest());
if (response.HasValues)
    Console.Out.WriteLine(JsonSerializer.Serialize(response.Response));




