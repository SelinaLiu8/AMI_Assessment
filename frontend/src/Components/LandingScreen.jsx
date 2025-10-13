import React from 'react'
import WeatherForm from './WeatherForm'

const LandingScreen = ({ onSearchClick }) => {
  return (
    <div>
        <h1>Welcome to the AMI Weather App</h1>
        <p>Start by searching for a location</p>
        <button className='landing-btn btn' onClick={onSearchClick}>Search</button>
    </div>
  )
}

export default LandingScreen