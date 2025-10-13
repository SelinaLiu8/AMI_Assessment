import React from 'react'
import '../Styles/LoadingScreen.css'

const LoadingScreen = () => {
  return (
    <div className="loading-overlay">
      <div className="spinner-container">
        <div className="spinner"></div>
      </div>
    </div>
  )
}

export default LoadingScreen