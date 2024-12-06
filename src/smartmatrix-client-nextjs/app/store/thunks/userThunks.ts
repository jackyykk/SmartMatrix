import apiProxy from '@/app/utils/apiProxy';
import { AppDispatch } from '../index';
import { setUser } from '../slices/userSlice';
import * as Constants from '../../constants/constants'; // Ensure the import path is correct

export const fetchUserThunk = (loginName: string) => async (dispatch: AppDispatch) => {
    try {
        const params = new URLSearchParams();
        params.append('partitionKey', `${Constants.PARTITION_KEY}`);
        params.append('loginName', `${loginName}`);                
        const queryString = params.toString();
        const url = `${Constants.API_BASE_URL}${Constants.API_CORE_SYSUSER_GETFIRST_BY_LOGIN_NAME}?${queryString}`;
        const response = await apiProxy({
            method: 'GET',
            url: url,
        });
      const result = response.data;
      if (result.succeeded && result.data)
      {
        dispatch(setUser({ user: result.data }));
      }
      else
      {
        console.error('Failed to fetch user:', `${result.messages.join(', ')}`);
      }      
    } catch (error) {
      console.error('Failed to fetch user:', error);
    }
  };  