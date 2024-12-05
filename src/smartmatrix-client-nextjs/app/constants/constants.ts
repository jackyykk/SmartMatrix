// Note: This file is used to store all the constants used in the application
export const APP_NAME = 'Smart Matrix';
export const APP_VERSION = '1.0.0';

// API Endpoints, no slash at the end
export const API_BASE_URL = process.env.NEXT_PUBLIC_APISERVER_BASE_URL + '/api';
export const API_AUTH_STANDARD_LOGIN = '/v1.0/auth/standard/login';
export const API_AUTH_GOOGLE_LOGIN = '/v1.0/auth/google/login';

export const API_AUTH_STANDARD_RENEW_TOKEN_BY_OTT = '/v1.0/auth/standard/renew-token-by-ott';

export const API_APP_WEATHER_FORECAST_GETLIST = '/v1.0/apps/weather_forecast_app/getlist';


// Local Storage Keys
export const LSK_AUTH_LOGIN_NAME = 'auth-login_name';
export const LSK_AUTH_USER_NAME = 'auth-user_name';
export const LSK_AUTH_ACCESS_TOKEN = 'auth-access_token';
export const LSK_AUTH_ACCESS_TOKEN_LifeInMinutes = 'auth-access_token_lifeinminutes';
export const LSK_AUTH_ACCESS_TOKEN_Expires = 'auth-access_token_expires';
export const LSK_AUTH_REFRESH_TOKEN = 'auth-refresh_token';
export const LSK_AUTH_REFRESH_TOKEN_LifeInMinutes = 'auth-refresh_token_lifeinminutes';
export const LSK_AUTH_REFRESH_TOKEN_Expires = 'auth-refresh_token_expires';