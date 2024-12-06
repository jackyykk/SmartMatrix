import { SysUser_OutputPayload } from '@/app/types/core/identitiesTypes';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UserState {
    user: SysUser_OutputPayload | null;
    error: string | null;
}

const initialState: UserState = {
    user: null,
    error: null,
};

const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        setUser: (state, action: PayloadAction<{ user: SysUser_OutputPayload, error: string }>) => {
            state.user = action.payload.user;
            state.error = action.payload.error;
        },        
        setError: (state, action: PayloadAction<string>) => {
            state.error = action.payload;
        },
        clearUser: (state) => {            
            state.user = null;
            state.error = null;
        },
        clearError: (state) => {
            state.error = null;
        },
    },
});

export const { setUser, setError, clearUser, clearError } = userSlice.actions;
export default userSlice.reducer;