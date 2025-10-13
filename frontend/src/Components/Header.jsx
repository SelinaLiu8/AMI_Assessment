import React from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faSearch } from '@fortawesome/free-solid-svg-icons'
import '../Styles/Header.css'

const Header = ({onSearchClick}) => {
  return (
    <div className='header-container'>
        <h2 className='header-title'>AMI Weather App</h2>
        <button className="search-btn" onClick={onSearchClick}>
          <FontAwesomeIcon icon={faSearch} />
        </button>
    </div>
  )
}

export default Header