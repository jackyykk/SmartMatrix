import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AuthSecret } from '../../types/utils/authSecretTypes';

interface AuthState {
  isAuthenticated: boolean;
  secret: AuthSecret | null;  
}

const initialState: AuthState = {
  isAuthenticated: false,
  secret: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    login: (state, action: PayloadAction<{ secret: AuthSecret }>) => {
      state.isAuthenticated = true;
      state.secret = action.payload.secret;      
    },
    logout: (state) => {
      state.isAuthenticated = false;
      state.secret = null;
    },
  },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;