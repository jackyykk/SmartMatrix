'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { AppDispatch, RootState } from '../store';
import { useDispatch, useSelector } from 'react-redux';
import { fetchUserThunk } from '../store/thunks/userThunks';
import {
    Button,
    Container,
    Typography,
    CircularProgress,
    Box,
    Tabs,
    Tab,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper
} from '@mui/material';

const MyInfoPage = () => {
    const router = useRouter();
    const dispatch: AppDispatch = useDispatch();
    const { isAuthenticated, secret } = useSelector((state: RootState) => state.auth);
    const { user } = useSelector((state: RootState) => state.user);

    const [isClient, setIsClient] = useState(false);
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');
    const [tabIndex, setTabIndex] = useState(0);

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

    const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
        setTabIndex(newValue);
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
                <Tabs value={tabIndex} onChange={handleTabChange}>
                    <Tab label="General Info" />
                    <Tab label="Configuration" />
                </Tabs>
                {tabIndex === 0 && (
                    <>
                        {loading ? (
                            <CircularProgress />
                        ) : (
                            <>
                                <Typography variant="body1">Username: {user?.userName}</Typography>
                                <Typography variant="body1">Display Name: {user?.displayName}</Typography>
                                <Typography variant="body1">Email: {user?.email}</Typography>
                                <Box mt={2}>
                                    <Typography variant="h6">Logins</Typography>
                                    <TableContainer component={Paper}>
                                        <Table>
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell>Provider</TableCell>
                                                    <TableCell>Type</TableCell>
                                                    <TableCell>Login Name</TableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {user?.logins.map((login) => (
                                                    <TableRow key={login.id}>
                                                        <TableCell>{login.loginProvider}</TableCell>
                                                        <TableCell>{login.loginType}</TableCell>
                                                        <TableCell>{login.loginName}</TableCell>
                                                    </TableRow>
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                </Box>
                                <Box mt={2}>
                                    <Typography variant="h6">Roles</Typography>
                                    <TableContainer component={Paper}>
                                        <Table>
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell>Role Name</TableCell>
                                                    <TableCell>Role Description</TableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {user?.roles.map((role) => (
                                                    <TableRow key={role.id}>
                                                        <TableCell>{role.roleName}</TableCell>
                                                        <TableCell>{role.description}</TableCell>
                                                    </TableRow>
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                </Box>
                                {message && <Typography color="success.main">{message}</Typography>}
                                {error && <Typography color="error.main">{error}</Typography>}
                                <Box mt={2}>
                                    <Button variant="contained" color="primary" onClick={fetchUser}>
                                        Refresh
                                    </Button>
                                </Box>
                            </>
                        )}
                    </>
                )}
                {tabIndex === 1 && (
                    <Typography variant="body1">Configuration content goes here...</Typography>
                )}
            </Box>
        </Container>
    );
}

export default MyInfoPage;