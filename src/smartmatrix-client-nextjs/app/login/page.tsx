'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
//import axios from 'axios';

const Login = () => {
    const router = useRouter();

    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    const [loginName, setLoginName] = useState('');
    const [password, setPassword] = useState('');

    useEffect(() => {
        setMessage('');

        const urlParams = new URLSearchParams(window.location.search);
        const tokensSet = urlParams.get('ts');
        const onetimeToken = urlParams.get('ott');

        if (tokensSet && onetimeToken) {
            // Exchange the code for tokens
            fetch(`https://localhost:5001/api/auth/standard/renew-token-by-ott?oneTimeToken=${onetimeToken}`)
                .then(response => response.json())
                .then(data => {

                    console.log(data);

                    if (data.accessToken && data.refreshToken) {                                                
                        // Save tokens to localStorage
                        localStorage.setItem('accessToken', data.accessToken);
                        localStorage.setItem('accessToken_LifeInMinutes', data.accessToken_LifeInMinutes);
                        localStorage.setItem('accessToken_Expires', data.accessToken_Expires);
                        localStorage.setItem('refreshToken', data.refreshToken);
                        localStorage.setItem('refreshToken_LifeInMinutes', data.refreshToken_LifeInMinutes);
                        localStorage.setItem('refreshToken_Expires', data.refreshToken_Expires);                        

                        // Redirect to main page
                        router.push('/main');
                    } else {
                        setError('Failed to exchange code for tokens');
                    }
                })
                .catch(err => {
                    setError('An error occurred while exchanging code for tokens');
                    console.error(err);
                });
        }

    }, [router]);

    const handleStandardLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const returnUrl = new URLSearchParams(window.location.search).get('returnUrl') || `${window.location.origin}/login`;
            const response = await fetch(`https://localhost:5001/api/auth/standard/login?returnUrl=${encodeURIComponent(returnUrl)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ loginName, password }),
            });

            if (response.ok) {
                const data = await response.json();
                // Store tokens
                localStorage.setItem('accessToken', data.accessToken);
                localStorage.setItem('accessToken_LifeInMinutes', data.accessToken_LifeInMinutes);
                localStorage.setItem('accessToken_Expires', data.accessToken_Expires);
                localStorage.setItem('refreshToken', data.refreshToken);
                localStorage.setItem('refreshToken_LifeInMinutes', data.refreshToken_LifeInMinutes);
                localStorage.setItem('refreshToken_Expires', data.refreshToken_Expires);

                // Redirect to main page
                router.push('/main');
            } else {
                const errorData = await response.json();
                setError(errorData.message || 'An error occurred during standard login');
            }
        } catch (err) {
            setError('An error occurred during standard login');
            console.error(err);
        }
    };

    const handleGoogleLogin = async () => {
        try {
            const returnUrl = new URLSearchParams(window.location.search).get('returnUrl') || `${window.location.origin}/login`;
            window.open(`https://localhost:5001/api/auth/google/login?returnUrl=${encodeURIComponent(returnUrl)}`, '_self');
        }
        catch (err) {
            setError('An error occurred during Google login');
            console.error(err);
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="w-full max-w-md p-8 space-y-8 bg-white rounded shadow-md">
                <h2 className="text-2xl font-bold text-center">Login</h2>
                <form className="space-y-6" onSubmit={handleStandardLogin}>
                    <div>
                        <label htmlFor="username" className="block text-sm font-medium text-gray-700">
                            Username
                        </label>
                        <input
                            id="username"
                            name="username"
                            type="text"
                            required
                            value={loginName}
                            onChange={(e) => setLoginName(e.target.value)}
                            className="w-full px-3 py-2 mt-1 border rounded-md shadow-sm focus:outline-none focus:ring focus:ring-indigo-200"
                        />
                    </div>
                    <div>
                        <label htmlFor="password" className="block text-sm font-medium text-gray-700">
                            Password
                        </label>
                        <input
                            id="password"
                            name="password"
                            type="password"
                            required
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="w-full px-3 py-2 mt-1 border rounded-md shadow-sm focus:outline-none focus:ring focus:ring-indigo-200"
                        />
                    </div>
                    <button
                        type="submit"
                        className="w-full px-4 py-2 font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 focus:outline-none focus:ring focus:ring-indigo-200"
                    >
                        Login
                    </button>
                </form>
                <div className="flex items-center justify-center mt-4">
                    <span className="text-sm text-gray-600">or</span>
                </div>
                <button
                    onClick={handleGoogleLogin}
                    className="w-full px-4 py-2 font-medium text-white bg-red-600 rounded-md hover:bg-red-700 focus:outline-none focus:ring focus:ring-red-200"
                >
                    Login with Google
                </button>
                {message && <p className="mt-4 text-green-600">{message}</p>}
                {error && <p className="mt-4 text-red-600">{error}</p>}
            </div>
        </div>
    );
};

export default Login;