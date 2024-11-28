'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import * as Constants from '../constants/constants';
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
        const returnUrl = urlParams.get('returnUrl');

        console.log('tokensSet: ', tokensSet);
        console.log('onetimeToken: ', onetimeToken);
        console.log('returnUrl: ', returnUrl);

        if (tokensSet) {
            if (onetimeToken && returnUrl) {
                // Exchange the one-time token for tokens
                fetch(`${Constants.API_BASE_URL}${Constants.API_AUTH_STANDARD_RENEW_TOKEN_BY_OTT}?oneTimeToken=${onetimeToken}`)
                    .then(response => response.json())
                    .then(result => {
                        console.log('result: ', result);

                        const succeeded = result.succeeded;
                        const data = result.data;                        
                        if (succeeded && data && data.token && data.token.accessToken && data.token.refreshToken) {
                            // Save tokens
                            localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN, data.token.accessToken);
                            localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN_LifeInMinutes, data.token.accessToken_LifeInMinutes);
                            localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN_Expires, data.token.accessToken_Expires);
                            localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN, data.token.refreshToken);
                            localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN_LifeInMinutes, data.token.refreshToken_LifeInMinutes);
                            localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN_Expires, data.token.refreshToken_Expires);

                            // Redirect to return Url
                            router.push(returnUrl);
                        } else {
                            setError(`Failed to login [${result.statusCode}]: ${result.messages.join(', ')}`);
                        }
                    })
                    .catch(err => {
                        setError('Failed to login: An error occurred while exchanging code for tokens');
                        console.error(err);
                    });
            }
            else {
                setError('Failed to login: Failed to get one-time token and return URL');
            }
        }
    }, [router]);

    const handleStandardLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            // it will redirect to returnUrl if it is provided in the query string, redirect to /main otherwise
            const returnUrl = new URLSearchParams(window.location.search).get('returnUrl') || `${window.location.origin}/main`;
            const response = await fetch(`${Constants.API_BASE_URL}${Constants.API_AUTH_STANDARD_LOGIN}?returnUrl=${encodeURIComponent(returnUrl)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ loginName, password }),
            });

            if (response.ok) {
                const result = await response.json();
                console.log('result: ', result);

                const succeeded = result.succeeded;
                const data = result.data;                        
                if (succeeded && data && data.token && data.token.accessToken && data.token.refreshToken) {
                    // Save tokens
                    localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN, data.token.accessToken);
                    localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN_LifeInMinutes, data.token.accessToken_LifeInMinutes);
                    localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN_Expires, data.token.accessToken_Expires);
                    localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN, data.token.refreshToken);
                    localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN_LifeInMinutes, data.token.refreshToken_LifeInMinutes);
                    localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN_Expires, data.token.refreshToken_Expires);

                    // Redirect to return Url
                    router.push(returnUrl);
                } else {
                    setError(`Failed to login [${result.statusCode}]: ${result.messages.join(', ')}`);
                }                
            } else {
                const errorData = await response.json();
                setError(errorData.message || 'Failed to login: An error occurred during standard login');
            }
        } catch (err) {
            setError('Failed to login: An error occurred during standard login');
            console.error(err);
        }
    };

    const handleGoogleLogin = async () => {
        try {
            // it will redirect to returnUrl if it is provided in the query string, redirect to /main otherwise            
            const originUrl = `${window.location.origin}/login`;
            const returnUrl = new URLSearchParams(window.location.search).get('returnUrl') || `${window.location.origin}/main`;
            window.open(`${Constants.API_BASE_URL}${Constants.API_AUTH_GOOGLE_LOGIN}?originUrl=${encodeURIComponent(originUrl)}&returnUrl=${encodeURIComponent(returnUrl)}`, '_self');
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