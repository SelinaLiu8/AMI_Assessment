import React from 'react'

const WeatherDisplay = ({ data }) => {
    if (!data) return null
    return (
        <div className="weather-display">
            <h2>Weather for {data.city}, {data.state}</h2>
            <p><strong>Temperature:</strong> {data.temperature} °F</p>
            <p><strong>Cloud Coverage:</strong> {data.cloudCoverage * 100}%</p>
            <p>
                <strong>Wind:</strong> {data.windSpeed} mph ({data.windDirection}°)
            </p>
            <p>
                <strong>12-Month Avg Highs:</strong> {data.rolling12MonthTemps.join(', ')}
            </p>
            {data.precipitation && data.precipitation.length > 0 && (
                <div>
                <strong>Precipitation:</strong>
                <ul>
                    {data.precipitation.map((p, idx) => (
                    <li key={idx}>
                        {p.type} — {p.probability * 100}%
                    </li>
                    ))}
                </ul>
                </div>
            )}
        </div>
    )
}

export default WeatherDisplay