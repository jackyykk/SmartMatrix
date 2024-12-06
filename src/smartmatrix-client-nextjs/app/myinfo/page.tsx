'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { AppDispatch, RootState } from '../store';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Container, Typography, CircularProgress, Box } from '@mui/material';
import { fetchUserThunk } from '../store/thunks/userThunks';

const MyInfoPage = () => {
    const router = useRouter();
    const dispatch: AppDispatch = useDispatch();
    const { isAuthenticated, secret } = useSelector((state: RootState) => state.auth);
    const { user } = useSelector((state: RootState) => state.user);

    const [isClient, setIsClient] = useState(false);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        setIsClient(true);
        setMessage('');
        setError('');

        if (!user) {
            fetchUser();
        }
    }, [dispatch, user]);

    const fetchUser = async () => {
        setLoading(true);
        try {
            await dispatch(fetchUserThunk(secret!.loginName));
            setMessage('User info refreshed successfully');
        } catch (err) {
            setError('Failed to fetch user info');
        } finally {
            setLoading(false);
        }
    };

    if (!isClient) {
        return null;
    }

    return (
        <Container maxWidth="sm">
            <Box mt={4}>
                <Typography variant="h4" gutterBottom>
                    My Info
                </Typography>
                {loading ? (
                    <CircularProgress />
                ) : (
                    <>
                        <Typography variant="body1">Username: {user?.userName}</Typography>
                        <Typography variant="body1">Display Name: {user?.displayName}</Typography>
                        <Typography variant="body1">Email: {user?.email}</Typography>
                        {message && <Typography color="success.main">{message}</Typography>}
                        {error && <Typography color="error.main">{error}</Typography>}
                        <Box mt={2}>
                            <Button variant="contained" color="primary" onClick={fetchUser}>
                                Refresh
                            </Button>
                        </Box>
                    </>
                )}
            </Box>
        </Container>
    );
}

export default MyInfoPage;