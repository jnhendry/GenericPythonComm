import flask
import waitress
import flask_restful
import http.client

# Dummy weather service call to test c# processing
class GetWeather(flask_restful.Resource):
    def post(self):
        config = flask_restful.request.get_json()
        result = calltoweatherservice(config)
        return flask_restful.output_json(result, 200)


def calltoweatherservice(inputData):
    data = {
        "coord": {
            "lon": 10.99,
            "lat": 44.34
        },
        "weather": [
            {
                "id": 501,
                "main": "Rain",
                "description": "moderate rain",
                "icon": "10d"
            }
        ],
        "base": "stations",
        "main": {
            "temp": 298.48,
            "feels_like": 298.74,
            "temp_min": 297.56,
            "temp_max": 300.05,
            "pressure": 1015,
            "humidity": 64,
            "sea_level": 1015,
            "grnd_level": 933
        },
        "visibility": 10000,
        "wind": {
            "speed": 0.62,
            "deg": 349,
            "gust": 1.18
        },
        "rain": {
            "1h": 3.16
        },
        "clouds": {
            "all": 100
        },
        "dt": 1661870592,
        "sys": {
            "type": 2,
            "id": 2075663,
            "country": "IT",
            "sunrise": 1661834187,
            "sunset": 1661882248
        },
        "timezone": 7200,
        "id": 3163858,
        "name": "Zocca",
        "cod": 200
    }

    return data


def main():
    app = flask.Flask("WeatherApi")

    api = flask_restful.Api(app)
    api.add_resource(GetWeather, "/Weather/")
    try:
        waitress.serve(app, host="localhost", port=30238)
    except Exception as e:
        print(e)


if __name__ == "__main__":
    main()
