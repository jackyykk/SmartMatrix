import * as Constants from '../constants/constants';
import { AuthSecret } from '../types/utils/authSecretTypes';

export const saveSecrets = (secret: AuthSecret, rememberMe: boolean) => {
    const storage = rememberMe ? localStorage : sessionStorage;

    const token = secret.token;

    storage.setItem(Constants.LSK_AUTH_LOGIN_NAME, secret.loginName);
    storage.setItem(Constants.LSK_AUTH_USER_NAME, secret.userName);
    storage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN, token.accessToken);
    storage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN_LifeInMinutes, token.accessToken_LifeInMinutes.toString());
    storage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN_Expires, token.accessToken_Expires);
    storage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN, token.refreshToken);
    storage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN_LifeInMinutes, token.refreshToken_LifeInMinutes.toString());
    storage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN_Expires, token.refreshToken_Expires);
};


export const clearSecrets = () => {
    localStorage.removeItem(Constants.LSK_AUTH_LOGIN_NAME);
    localStorage.removeItem(Constants.LSK_AUTH_USER_NAME);
    localStorage.removeItem(Constants.LSK_AUTH_ACCESS_TOKEN);
    localStorage.removeItem(Constants.LSK_AUTH_ACCESS_TOKEN_LifeInMinutes);
    localStorage.removeItem(Constants.LSK_AUTH_ACCESS_TOKEN_Expires);
    localStorage.removeItem(Constants.LSK_AUTH_REFRESH_TOKEN);
    localStorage.removeItem(Constants.LSK_AUTH_REFRESH_TOKEN_LifeInMinutes);
    localStorage.removeItem(Constants.LSK_AUTH_REFRESH_TOKEN_Expires);

    sessionStorage.removeItem(Constants.LSK_AUTH_LOGIN_NAME);
    sessionStorage.removeItem(Constants.LSK_AUTH_USER_NAME);
    sessionStorage.removeItem(Constants.LSK_AUTH_ACCESS_TOKEN);
    sessionStorage.removeItem(Constants.LSK_AUTH_ACCESS_TOKEN_LifeInMinutes);
    sessionStorage.removeItem(Constants.LSK_AUTH_ACCESS_TOKEN_Expires);
    sessionStorage.removeItem(Constants.LSK_AUTH_REFRESH_TOKEN);
    sessionStorage.removeItem(Constants.LSK_AUTH_REFRESH_TOKEN_LifeInMinutes);
    sessionStorage.removeItem(Constants.LSK_AUTH_REFRESH_TOKEN_Expires);
};

export const checkSecrets = () => {
    const loginName = localStorage.getItem(Constants.LSK_AUTH_LOGIN_NAME) || sessionStorage.getItem(Constants.LSK_AUTH_LOGIN_NAME);
    const userName = localStorage.getItem(Constants.LSK_AUTH_USER_NAME) || sessionStorage.getItem(Constants.LSK_AUTH_USER_NAME);
    const accessToken = localStorage.getItem(Constants.LSK_AUTH_ACCESS_TOKEN) || sessionStorage.getItem(Constants.LSK_AUTH_ACCESS_TOKEN);
    const accessToken_LifeInMinutes = localStorage.getItem(Constants.LSK_AUTH_ACCESS_TOKEN_LifeInMinutes) || sessionStorage.getItem(Constants.LSK_AUTH_ACCESS_TOKEN_LifeInMinutes);
    const accessToken_Expires = localStorage.getItem(Constants.LSK_AUTH_ACCESS_TOKEN_Expires) || sessionStorage.getItem(Constants.LSK_AUTH_ACCESS_TOKEN_Expires);
    const refreshToken = localStorage.getItem(Constants.LSK_AUTH_REFRESH_TOKEN) || sessionStorage.getItem(Constants.LSK_AUTH_REFRESH_TOKEN);
    const refreshToken_LifeInMinutes = localStorage.getItem(Constants.LSK_AUTH_REFRESH_TOKEN_LifeInMinutes) || sessionStorage.getItem(Constants.LSK_AUTH_REFRESH_TOKEN_LifeInMinutes);
    const refreshToken_Expires = localStorage.getItem(Constants.LSK_AUTH_REFRESH_TOKEN_Expires) || sessionStorage.getItem(Constants.LSK_AUTH_REFRESH_TOKEN_Expires);

    if (!loginName || !userName || !accessToken || !accessToken_LifeInMinutes || !accessToken_Expires || !refreshToken || !refreshToken_LifeInMinutes || !refreshToken_Expires)
        return null;
        
    return {
        loginName,
        userName,
        token: {
            accessToken,
            accessToken_LifeInMinutes: parseInt(accessToken_LifeInMinutes),
            accessToken_Expires,
            refreshToken,
            refreshToken_LifeInMinutes: parseInt(refreshToken_LifeInMinutes),
            refreshToken_Expires
    }};
};