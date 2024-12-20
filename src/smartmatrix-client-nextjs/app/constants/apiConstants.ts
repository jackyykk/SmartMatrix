// API Endpoints, no slash at the end
export const API_BASE_URL = process.env.NEXT_PUBLIC_APISERVER_BASE_URL + '/api';
export const API_AUTH_STANDARD_LOGIN = '/v1.0/auth/standard/login';
export const API_AUTH_GOOGLE_LOGIN = '/v1.0/auth/google/login';

export const API_AUTH_STANDARD_RENEW_TOKEN_BY_OTT = '/v1.0/auth/standard/renew-token-by-ott';
export const API_CORE_SYSTOKEN_RENEW_TOKEN = '/v1.0/core/systoken/renew-token';
export const API_CORE_SYSUSER_GETFIRST_BY_LOGIN_NAME = '/v1.0/core/sysuser/getfirst-by_login_name';

export const API_APP_WEATHER_FORECAST_GETLIST = '/v1.0/tools/weather_forecast_tool/getlist';