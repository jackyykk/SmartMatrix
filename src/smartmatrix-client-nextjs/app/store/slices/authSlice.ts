import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { SysUser_OutputPayload } from '../../types/core/identitiesTypes';

interface AuthState {
  isAuthenticated: boolean;
  loginName: string | null;
  userName: string | null;  
}

const initialState: AuthState = {
  isAuthenticated: false,
  loginName: null,
  userName: null,  
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    login: (state, action: PayloadAction<{ loginName: string; userName: string }>) => {
      state.isAuthenticated = true;
      state.loginName = action.payload.loginName;
      state.userName = action.payload.userName;
    },
    logout: (state) => {
      state.isAuthenticated = false;
      state.loginName = null;
      state.userName = null;      
    },
  },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;