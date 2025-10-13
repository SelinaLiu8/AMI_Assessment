import { useState } from 'react'
import WeatherForm from './Components/WeatherForm'
import WeatherDisplay from './Components/WeatherDisplay'
import LoadingScreen from './Components/LoadingScreen'
import LandingScreen from './Components/LandingScreen'
import './App.css'

function App() {
  const [showForm, setShowForm] = useState(false)
  const [weatherData, setWeatherData] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const handleFormSubmit = async (formData) => {
    setLoading(true)
    setError('')
    setWeatherData(null)

    try {
      const response = await fetch('http://localhost:5262/api/Weather', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
      })

      if (!response.ok) {
        throw new Error(`Error ${response.status}: ${response.statusText}`)
      }

      const data = await response.json()
      setWeatherData(data)
    } catch (err) {
      console.error(err)
      setError('Failed to fetch weather data. Please try again.')
    } finally {
      setLoading(false)
    }
  }
  return (
    <>
      {!weatherData && !loading && (
        <LandingScreen onSearchClick={() => setShowForm(true)} />
      )}

      {showForm && !weatherData && !loading && (
        <WeatherForm 
          onSubmit={handleFormSubmit} 
          onClose={() => setShowForm(false)} 
        />
      )}

      {loading && <LoadingScreen />}
      {error && <p className='error-text'>{error}</p>}
      {weatherData && <WeatherDisplay data={weatherData} />}
    </>
  )
}

export default App
