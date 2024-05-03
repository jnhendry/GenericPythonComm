using System.Text.Json.Serialization;

public record Coord
{
    public float Lon { get; set; }
    public float Lat { get; set; }
}

public record WeatherInfo
{
    public int Id { get; set; }
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}

public record MainInfo
{
    public float Temp { get; set; }
    public float Feels_Like { get; set; }
    public float Temp_Min { get; set; }
    public float Temp_Max { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
    public int Sea_Level { get; set; }
    public int Grnd_Level { get; set; }
}

public record WindInfo
{
    public float Speed { get; set; }
    public int Deg { get; set; }
    public float Gust { get; set; }
}

public record RainInfo
{
    [JsonPropertyName("1h")]
    public float RainVolume { get; set; }
}

public record CloudInfo
{
    public int All { get; set; }
}

public record SysInfo
{
    public int Type { get; set; }
    public int Id { get; set; }
    public string Country { get; set; }
    public long Sunrise { get; set; }
    public long Sunset { get; set; }
}

public record WeatherApiResponse
{
    public Coord Coord { get; set; }
    public List<WeatherInfo> Weather { get; set; }
    public string Base { get; set; }
    public MainInfo Main { get; set; }
    public int Visibility { get; set; }
    public WindInfo Wind { get; set; }
    public RainInfo Rain { get; set; }
    public CloudInfo Clouds { get; set; }
    public long Dt { get; set; }
    public SysInfo Sys { get; set; }
    public int Timezone { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Cod { get; set; }
}
