import React from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faSun, faCloudRain, faSnowflake, faWind } from '@fortawesome/free-solid-svg-icons'
import '../Styles/WeatherDisplay.css'

const WeatherDisplay = ({ data }) => {
    if (!data) return null
    let icon;
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

    const weatherIcon = getWeatherIcon();

    return (
        <div className="weather-display-container">
            {/* Left column */}
            <div className="left-column">
                <div className="main-display">
                    <div className="main-info">
                        <h2 className="display-title">
                            Weather for {data.city}, {data.state}
                        </h2>
                        <p className='display-subtitle'><strong>Temperature:</strong> {data.temperature} °{data.unitMeasure}</p>
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
                    <ul>
                        {data.precipitation.map((p, idx) => (
                        <li key={idx}>{p.type} — {p.probability * 100}%</li>
                        ))}
                    </ul>
                    </div>
                )}
                </div>
            </div>

            {/* Right column */}
            <div className="forecast-display">
                <strong>12-Month Avg Highs:</strong>
                <div className="forecast-list">
                    {data.rolling12MonthTemps.map((temp, idx) => (
                    <p key={idx}>{temp}°{data.unitMeasure}</p>
                    ))}
                </div>
            </div>
        </div>
    )
}

export default WeatherDisplay