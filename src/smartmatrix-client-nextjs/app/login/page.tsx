'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import axios from 'axios';
import * as Constants from '../constants/constants';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash, faSignInAlt, faSignOutAlt } from '@fortawesome/free-solid-svg-icons';
import { faGoogle } from '@fortawesome/free-brands-svg-icons';
import { saveSecrets, clearSecrets, checkSecrets } from '../utils/authSecretUtils';
import { useDispatch, useSelector } from 'react-redux';
import { AppDispatch, RootState } from '../store'; // Ensure the import path is correct
import { login } from '../store/slices/authSlice'; // Import the login and logout actions
import { logoutThunk } from '../store/thunks/authThunks'; // Import the logoutThunk action


const Login = () => {
    const router = useRouter();
    const dispatch: AppDispatch = useDispatch();
    const { isLoggedIn, userName } = useSelector((state: RootState) => state.auth);

    const [isClient, setIsClient] = useState(false);
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(true); // State for loading    

    const [loginName, setLoginName] = useState('');
    const [password, setPassword] = useState('');
    const [showPassword, setShowPassword] = useState(false); // State for password visibility
    const [rememberMe, setRememberMe] = useState(false); // State for "Remember Me"
    const [isSubmitting, setIsSubmitting] = useState(false); // State for button disabled

    useEffect(() => {
        setLoading(true);
        setIsClient(true);
        setMessage('');

        // get the query string parameters
        const urlParams = new URLSearchParams(window.location.search);
        const tokensSet = urlParams.get('ts');
        const onetimeToken = urlParams.get('ott');
        const returnUrl = urlParams.get('returnUrl');

        if (tokensSet) {
            if (onetimeToken && returnUrl) {
                // Exchange the one-time token for tokens                
                axios.get(`${Constants.API_BASE_URL}${Constants.API_AUTH_STANDARD_RENEW_TOKEN_BY_OTT}`, {
                    params: { oneTimeToken: onetimeToken }
                })
                    .then(response => {
                        const result = response.data;
                        console.log('result: ', result);

                        const succeeded = result.succeeded;
                        const data = result.data;
                        if (succeeded && data) {
                            // Save tokens
                            saveSecrets(data, rememberMe);

                            // Dispatch login action to update AuthState
                            dispatch(login({ loginName: data.loginName, userName: data.userName }));

                            // Redirect to return Url
                            router.push(returnUrl);
                        } else {
                            setError(`Failed to login [${result.statusCode}]: ${result.messages.join(', ')}`);
                        }
                    })
                    .catch(err => {
                        setError('Failed to login: An error occurred while exchanging code for tokens');
                        console.error(err);
                    })
                    .finally(
                        () => setLoading(false)
                    );
            }
            else {
                setError('Failed to login: Failed to get one-time token and return URL');
                setLoading(false);
            }
        } else {
            setLoading(false);
        }
    }, [router, dispatch, rememberMe]);

    const togglePasswordVisibility = () => {
        console.log('showPassword: ', showPassword);
        setShowPassword(!showPassword);
    };

    const handleStandardLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsSubmitting(true);
        try {
            // it will redirect to returnUrl if it is provided in the query string, redirect to /main otherwise
            const returnUrl = new URLSearchParams(window.location.search).get('returnUrl') || `${window.location.origin}/main`;
            const response = await axios.post(`${Constants.API_BASE_URL}${Constants.API_AUTH_STANDARD_LOGIN}?returnUrl=${encodeURIComponent(returnUrl)}`, {
                loginName,
                password,
                returnUrl
            });

            const result = response.data;
            console.log('result: ', result);

            const succeeded = result.succeeded;
            const data = result.data;
            if (succeeded && data) {
                // Save tokens
                saveSecrets(data, rememberMe);

                // Dispatch login action to update AuthState
                dispatch(login({ loginName: data.loginName, userName: data.userName }));

                // Redirect to return Url
                router.push(returnUrl);
            } else {
                setError(`Failed to login [${result.statusCode}]: ${result.messages.join(', ')}`);
            }
        } catch (err) {
            setError('Failed to login: An error occurred during standard login');
            console.error(err);
        } finally {
            // Re-enable the button
            setIsSubmitting(false);
        }
    };

    const handleGoogleLogin = async () => {
        setIsSubmitting(true);
        try {
            // it will redirect to returnUrl if it is provided in the query string, redirect to /main otherwise            
            const originUrl = `${window.location.origin}/login`;
            const returnUrl = new URLSearchParams(window.location.search).get('returnUrl') || `${window.location.origin}/main`;
            window.location.href = `${Constants.API_BASE_URL}${Constants.API_AUTH_GOOGLE_LOGIN}?originUrl=${encodeURIComponent(originUrl)}&returnUrl=${encodeURIComponent(returnUrl)}`;
        }
        catch (err) {
            setError('An error occurred during Google login');
            console.error(err);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleLogout = () => {
        dispatch(logoutThunk()); // Dispatch the logoutThunk action
        router.push('/login');
    };

    if (!isClient) {
        return null;
    }

    if (loading) {
        return (
            <div className="flex items-center justify-center min-h-screen bg-gray-100">
                <div className="w-full max-w-md p-8 space-y-8 bg-white rounded shadow-md">
                    <h2 className="text-2xl font-bold text-center">Loading...</h2>
                </div>
            </div>
        );
    }

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100" suppressHydrationWarning={true}>
            <div className="w-full max-w-md p-8 space-y-8 bg-white rounded shadow-md">
                <h2 className="text-2xl font-bold text-center">Login</h2>
                {
                    isLoggedIn ? (
                        <div className="text-center">
                            <p className="text-lg">Welcome, {userName}!</p>
                            <button
                                onClick={handleLogout}
                                className="w-full px-4 py-2 mt-4 font-medium text-white bg-red-600 rounded-md hover:bg-red-700 focus:outline-none focus:ring focus:ring-red-200 flex items-center justify-center"
                            >
                                <FontAwesomeIcon icon={faSignOutAlt} className="mr-2" />
                                Logout
                            </button>
                        </div>
                    ) : (
                        <form className="space-y-6" onSubmit={handleStandardLogin}>
                            <div>
                                <label htmlFor="loginName" className="block text-sm font-medium text-gray-700">
                                    Username
                                </label>
                                <input
                                    id="loginName"
                                    name="loginName"
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
                                <div className="relative">
                                    <input
                                        id="password"
                                        name="password"
                                        type={showPassword ? 'text' : 'password'}
                                        required
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        className="w-full px-3 py-2 mt-1 border rounded-md shadow-sm focus:outline-none focus:ring focus:ring-indigo-200"
                                    />
                                    <button
                                        type="button"
                                        onClick={togglePasswordVisibility}
                                        className="absolute inset-y-0 right-0 px-3 py-2 mt-1 text-sm font-medium text-gray-700 bg-gray-200 rounded-r-md hover:bg-gray-300 focus:outline-none focus:ring focus:ring-indigo-200"
                                    >
                                        <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} />
                                    </button>
                                </div>
                            </div>
                            <div className="flex items-center">
                                <input
                                    id="rememberMe"
                                    name="rememberMe"
                                    type="checkbox"
                                    checked={rememberMe}
                                    onChange={(e) => setRememberMe(e.target.checked)}
                                    className="w-4 h-4 text-indigo-600 border-gray-300 rounded focus:ring-indigo-500"
                                />
                                <label htmlFor="rememberMe" className="ml-2 block text-sm text-gray-900">
                                    Remember Me
                                </label>
                            </div>
                            <button
                                type="submit"
                                className="w-full px-4 py-2 font-medium text-white bg-indigo-600 rounded-md hover:bg-indigo-700 focus:outline-none focus:ring focus:ring-indigo-200 flex items-center justify-center"
                                disabled={isSubmitting}
                            >
                                <FontAwesomeIcon icon={faSignInAlt} className="mr-2" />
                                Login
                            </button>
                        </form>
                    )
                }
                {
                    !isLoggedIn && (
                        <>
                            <div className="flex items-center justify-center mt-4">
                                <span className="text-sm text-gray-600">or</span>
                            </div>
                            <button
                                onClick={handleGoogleLogin}
                                className="w-full px-4 py-2 font-medium text-white bg-red-600 rounded-md hover:bg-red-700 focus:outline-none focus:ring focus:ring-red-200 flex items-center justify-center"
                                disabled={isSubmitting}
                            >
                                <FontAwesomeIcon icon={faGoogle} className="mr-2" />
                                Login with Google
                            </button>
                        </>
                    )
                }
                {message && <p className="mt-4 text-green-600">{message}</p>}
                {error && <p className="mt-4 text-red-600">{error}</p>}
            </div>
        </div>
    );
};

export default Login;