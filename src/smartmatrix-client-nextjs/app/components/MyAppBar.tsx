'use client';

import React, { useEffect, useState } from 'react';
import { AppBar, Toolbar, Typography, Button, IconButton, Menu, MenuItem } from '@mui/material';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser } from '@fortawesome/free-solid-svg-icons';

import { useRouter } from 'next/navigation';
import { useSelector, useDispatch } from 'react-redux';
import { AppDispatch, RootState } from '../store'; // Ensure the import path is correct
import { login } from '../store/slices/authSlice'; // Import the login and logout actions
import { logoutThunk } from '../store/thunks/authThunks'; // Import the logoutThunk action
import { fetchUserThunk } from '../store/thunks/userThunks'; // Import the fetchUserThunk action
import { getSecret } from '../utils/authSecretUtils';

const MyAppBar = () => {
    const router = useRouter();
    const dispatch: AppDispatch = useDispatch();
    const { isAuthenticated, secret } = useSelector((state: RootState) => state.auth);
    const { user } = useSelector((state: RootState) => state.user);

    const [isClient, setIsClient] = useState(false);

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

    useEffect(() => {
        setIsClient(true);

        // Check if the user is already logged in
        const secret = getSecret();
        if (secret) {
            dispatch(login({ secret })); // Dispatch the login action
            if (!user) {
                dispatch(fetchUserThunk(secret.loginName)); // Dispatch the fetchUserThunk action
            }
        }
    }, [])

    const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const handleLogout = () => {
        dispatch(logoutThunk()); // Dispatch the logoutThunk action
        router.push('/login'); // Redirect to login page after logout
        handleMenuClose();
    };

    const handleShowMyInfo = () => {
        // Implement user profile logic here
        router.push('/myinfo'); // Redirect to my info page
        handleMenuClose();
    };

    const handleLogin = () => {
        router.push('/login'); // Redirect to login page
    };

    return (
        <AppBar position="static">
            <Toolbar>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Smart Matrix
                </Typography>
                {isAuthenticated ? (
                    <div className="flex items-center space-x-4">
                        <Typography variant="body1">Welcome, {secret?.userName}!</Typography>
                        <IconButton color="inherit" onClick={handleMenuOpen}>
                            <FontAwesomeIcon icon={faUser} />
                        </IconButton>
                        <Menu
                            anchorEl={anchorEl}
                            open={Boolean(anchorEl)}
                            onClose={handleMenuClose}
                        >
                            <MenuItem onClick={handleShowMyInfo}>My Info</MenuItem>
                            <MenuItem onClick={handleLogout}>Logout</MenuItem>
                        </Menu>
                    </div>
                ) : (
                    <Button color="inherit" onClick={handleLogin}>
                        Login
                    </Button>
                )}
            </Toolbar>
        </AppBar>
    );
};

export default MyAppBar;