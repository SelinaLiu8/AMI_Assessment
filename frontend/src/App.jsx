import { useState } from 'react'
import WeatherForm from './Components/WeatherForm'
import WeatherDisplay from './Components/WeatherDisplay'
import LoadingScreen from './Components/LoadingScreen'
import LandingScreen from './Components/LandingScreen'
import Header from './Components/Header'
import '../src/Styles/App.css'

function App() {
  const [showForm, setShowForm] = useState(false)
  const [weatherData, setWeatherData] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const handleFormSubmit = async (formData) => {
    setShowForm(false);
    setLoading(true)
    setError('')
    setWeatherData(null)

    console.log('form data:', formData);


    try {
      const response = await fetch('http://localhost:5262/api/Weather', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
      })

      if (!response.ok) {
        if (response.status === 400 || response.status === 500 || response.status === 404) {
          throw new Error('Location not found. Please check the city, state, and zip code.');
        }

        throw new Error(`Error ${response.status}: ${response.statusText}`);
      }

      const data = await response.json()
      console.log("data:", data)
      setWeatherData(data)
    } catch (err) {
      console.error(err)
      setError(err.message || 'Failed to fetch weather data. Please try again.');
    } finally {
      setLoading(false)
    }
  }
  return (
    <>
      {!weatherData && !loading && (
        <LandingScreen onSearchClick={() => setShowForm(true)} />
      )}

      {showForm && !loading && (
        <WeatherForm 
          onSubmit={handleFormSubmit} 
          onClose={() => setShowForm(false)} 
        />
      )}

      {loading && <LoadingScreen />}
      {error && <p className='error-text'>{error}</p>}
      {weatherData && 
        <>
          <Header onSearchClick={() => setShowForm(true)} />
          <WeatherDisplay data={weatherData} />
        </>}
    </>
  )
}

export default App
