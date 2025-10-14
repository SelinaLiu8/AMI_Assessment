import React from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faSun, faCloudRain, faSnowflake, faWind } from '@fortawesome/free-solid-svg-icons'
import '../Styles/WeatherDisplay.css'

const WeatherDisplay = ({ data }) => {
    if (!data) return null
    let icon;
    let message;

    const getWeatherIcon = () => {
        if (data.precipitation?.some(p => p.type.toLowerCase().includes('rain'))) {
            icon = faCloudRain;
        } else if (data.precipitation?.some(p => p.type.toLowerCase().includes('snow'))) {
            icon = faSnowflake;
        } else if (data.windSpeed > 15) {
            icon = faWind;
        } else {
            icon = faSun;
        }
    }

    const getWeatherMessage = () => {
        if (data.unitMeasure === 'C') {
            if (data.temperature >= 35) {
            message = "It’s scorching hot today, stay hydrated! ";
            } else if (data.temperature >= 25) {
            message = "It’s quite warm today, great day to be outside. ";
            } else if (data.temperature >= 15) {
            message = "It’s mild and pleasant today. ";
            } else if (data.temperature >= 5) {
            message = "It’s a bit chilly today, wear a light jacket. ";
            } else {
            message = "It’s really cold today, bundle up! ";
            }
        }

        if (data.unitMeasure === 'F') {
            if (data.temperature >= 95) {
            message = "It’s scorching hot today, stay hydrated! ";
            } else if (data.temperature >= 77) {
            message = "It’s quite warm today, great day to be outside. ";
            } else if (data.temperature >= 59) {
            message = "It’s mild and pleasant today. ";
            } else if (data.temperature >= 41) {
            message = "It’s a bit chilly today, wear a light jacket. ";
            } else {
            message = "It’s really cold today, bundle up! ";
            }
        }

        if (data.precipitation?.some(p => p.type.toLowerCase().includes('rain'))) {
            message += " Don’t forget an umbrella!";
        } else if (data.precipitation?.some(p => p.type.toLowerCase().includes('snow'))) {
            message += " Roads might be slippery, drive carefully!";
        }

        return message.trim();
    };

    getWeatherIcon();
    getWeatherMessage(); 

    return (
        <div className="weather-display-container">
            {/* Left column */}
            <div className="left-column">
                <div className="main-display">
                    <div className="main-info">
                        <h2 className="display-title">
                            {data.city}, {data.state}
                        </h2>
                        <p className='display-subtitle'><strong>Temperature:</strong> {data.temperature} °{data.unitMeasure}</p>
                        <p className='display-text'>{message}</p>
                    </div>
                    <FontAwesomeIcon
                        icon={icon}
                        className={`weather-icon ${
                            icon === faSun
                            ? 'sun'
                            : icon === faCloudRain
                            ? 'rain'
                            : icon === faSnowflake
                            ? 'snow'
                            : 'wind'
                        }`}
                    />
                </div>

                <div className="sub-display">
                <p><strong>Cloud Coverage:</strong> {data.cloudCoverage * 100}%</p>
                <p>
                    <strong>Wind:</strong> {data.windSpeed} mph ({data.windDirection}°)
                </p>
                {data.precipitation?.length > 0 && (
                    <div>
                    <strong>Precipitation:</strong>
                        {data.precipitation.map((p, idx) => (
                        <p key={idx}>{p.type} — {Math.ceil(p.probability * 100)}%</p>
                        ))}
                    </div>
                )}
                </div>
            </div>

            {/* Right column */}
            <div className="forecast-display">
                <h4 className='forecast-title'>12-Month Average Highs:</h4>
                <div className="forecast-list">
                    {data.rolling12MonthTemps.map((temp, idx) => (
                        <div className='forecast-item'>
                            <p>Month {idx + 1}</p>
                            <p key={idx}>{temp}°{data.unitMeasure}</p>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}

export default WeatherDisplay