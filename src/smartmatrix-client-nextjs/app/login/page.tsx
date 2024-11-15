'use client';

import { useEffect, useState } from 'react';
//import axios from 'axios';

const Login = () => {
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        setMessage('');
      }, []);

    const handleLogin = async () => {
        try
        {
            window.open('https://localhost:5001/auth/google/login', '_self');
        }
        catch(err)
        {
            setError('An error occurred');
            console.error(err);
        }
    };

    return (
        <div>
            <button onClick={handleLogin}>Login with Google</button>
            {message && <p>{message}</p>}
            {error && <p style={{ color: 'red' }}>{error}</p>}
        </div>
    );
};

export default Login;