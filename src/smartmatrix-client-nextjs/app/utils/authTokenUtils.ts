import * as Constants from '../constants/constants';
import { AuthTokenData } from '../types/utils/authTokenTypes';

export const saveTokens = (tokenData: AuthTokenData) => {
    localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN, tokenData.accessToken);
    localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN_LifeInMinutes, tokenData.accessToken_LifeInMinutes.toString());
    localStorage.setItem(Constants.LSK_AUTH_ACCESS_TOKEN_Expires, tokenData.accessToken_Expires);
    localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN, tokenData.refreshToken);
    localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN_LifeInMinutes, tokenData.refreshToken_LifeInMinutes.toString());
    localStorage.setItem(Constants.LSK_AUTH_REFRESH_TOKEN_Expires, tokenData.refreshToken_Expires);
};