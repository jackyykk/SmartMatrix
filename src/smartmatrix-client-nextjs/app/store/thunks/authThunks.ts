import { AppDispatch } from '../index';
import { clearSecret } from '../../utils/authSecretUtils';
import { logout } from '../slices/authSlice';

export const logoutThunk = () => (dispatch: AppDispatch) => {
  clearSecret();
  dispatch(logout());
};