import axios, { AxiosRequestConfig, AxiosResponse } from 'axios';
import * as coreConstants from '../constants/coreConstants';
import * as apiConstants from '../constants/apiConstants';
import { saveSecret, getSecret } from './authSecretUtils';

const apiProxy = async (config: AxiosRequestConfig): Promise<AxiosResponse> => {
  let secret = getSecret();

  if (!secret) {
    throw new Error('No authentication secret found');
  }

  // Check if the access token has expired
  const accessTokenExpires = new Date(secret.token.accessToken_Expires);
  const accessTokenThreshold = new Date(accessTokenExpires.getTime() - coreConstants.ACCESS_TOKEN_REFRESH_THRESHOLD_MINUTES * 60 * 1000);
  if (new Date() >= accessTokenThreshold) {
    // Refresh the access token
    const refreshTokenResponse = await axios.post(`${apiConstants.API_BASE_URL}${apiConstants.API_CORE_SYSTOKEN_RENEW_TOKEN}`, {
      refreshToken: secret.token.refreshToken,
    });
    
    const refreshTokenResult = refreshTokenResponse.data;
    if (refreshTokenResult.succeeded) {
      const newToken = refreshTokenResult.data.token;
      secret = {
        ...secret,
        token: newToken,
      };
      saveSecret(secret, true); // Save the new token
    } else {
      throw new Error('Failed to refresh token');
    }
  }

  // Add the access token to the request headers
  config.headers = {
    ...config.headers,
    Authorization: `Bearer ${secret.token.accessToken}`,
  };

  // Make the API call
  return axios(config);
};

export default apiProxy;