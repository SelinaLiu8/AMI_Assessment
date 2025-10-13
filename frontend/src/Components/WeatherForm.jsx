import React, { useState } from 'react'
import '../Styles/WeatherForm.css'

const WeatherForm = ({ onSubmit, onClose }) => {
    const [city, setCity] = useState('');
    const [zip, setZip] = useState('');
    const [state, setState] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        setError('');

        if (!city || !zip || !state) {
        setError('All fields are required.');
        return;
        }
        if (!/^\d{5}$/.test(zip)) {
        setError('Zip Code must be 5 digits.');
        return;
        }
        if (!/^[A-Za-z]{2}$/.test(state)) {
        setError('State must be 2 letters.');
        return;
        }

        onSubmit({ city, zip, state });
    };

    return (
        <div className="popup-overlay">
            <div className='popup-content'>
                <button className="close-btn" onClick={onClose}>×</button>
                 <form className="weather-form" onSubmit={handleSubmit}>
                    <div className='form-field'>
                        <label className='form-label'>City:</label>
                        <input className='form-input' type="text" value={city} onChange={(e) => setCity(e.target.value)} required />
                    </div>
                    <div className='form-field'>
                        <label className='form-label'>State:</label>
                        <input className='form-input' type="text" value={state} onChange={(e) => setState(e.target.value)} required maxLength={2} />
                    </div>
                    <div className='form-field'>
                        <label className='form-label'>Zip Code:</label>
                        <input className='form-input' type="text" value={zip} onChange={(e) => setZip(e.target.value)} required />
                    </div>
                    <button className='submit-btn btn' type="submit">Submit</button>
                    {error && <p className="error-text">{error}</p>}
                </form>
            </div>
        </div>
    )
}

export default WeatherForm