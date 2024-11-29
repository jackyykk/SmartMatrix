import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface AuthState {
  isLoggedIn: boolean;
  loginName: string | null;
  userName: string | null;
}

const initialState: AuthState = {
  isLoggedIn: false,
  loginName: null,
  userName: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    login: (state, action: PayloadAction<{ loginName: string; userName: string }>) => {
      state.isLoggedIn = true;
      state.loginName = action.payload.loginName;
      state.userName = action.payload.userName;
    },
    logout: (state) => {
      state.isLoggedIn = false;
      state.loginName = null;
      state.userName = null;
    },
  },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;