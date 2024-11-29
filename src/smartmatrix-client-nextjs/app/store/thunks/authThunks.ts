import { AppDispatch } from '../index';
import { clearSecrets } from '../../utils/authSecretUtils';
import { logout } from '../slices/authSlice';

export const logoutThunk = () => (dispatch: AppDispatch) => {
  clearSecrets();
  dispatch(logout());
};