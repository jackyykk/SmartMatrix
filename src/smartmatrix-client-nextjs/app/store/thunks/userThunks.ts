import apiProxy from '@/app/utils/apiProxy';
import { AppDispatch } from '../index';
import { setUser, setError } from '../slices/userSlice';
import * as coreConstants from '../../constants/coreConstants'; // Ensure the import path is correct
import * as apiConstants from '../../constants/apiConstants'; // Ensure the import path is correct

export const fetchUserThunk = (loginName: string) => async (dispatch: AppDispatch) => {
    try {
        const params = new URLSearchParams();
        params.append('partitionKey', `${coreConstants.PARTITION_KEY}`);
        params.append('loginName', `${loginName}`);
        const queryString = params.toString();
        const url = `${apiConstants.API_BASE_URL}${apiConstants.API_CORE_SYSUSER_GETFIRST_BY_LOGIN_NAME}?${queryString}`;
        const response = await apiProxy({
            method: 'GET',
            url: url,
        });
        const result = response.data;
        if (result.succeeded && result.data) {
            dispatch(setUser({ user: result.data.user, error: '' }));
        }
        else {
            const error = `Failed to fetch user: ${result.messages.join(', ')}`;
            dispatch(setError(error));
        }
    } catch (error) {
        console.error('Failed to fetch user:', error);
    }
};  