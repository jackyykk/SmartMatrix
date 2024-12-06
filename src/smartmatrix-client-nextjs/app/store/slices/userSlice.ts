import { SysUser_OutputPayload } from '@/app/types/core/identitiesTypes';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UserState {
    user: SysUser_OutputPayload | null;
}

const initialState: UserState = {
    user: null,
};

const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        setUser: (state, action: PayloadAction<{ user: SysUser_OutputPayload }>) => {
            state.user = action.payload.user;
        },
        clearUser: (state) => {            
            state.user = null;
        },
    },
});

export const { setUser, clearUser } = userSlice.actions;
export default userSlice.reducer;