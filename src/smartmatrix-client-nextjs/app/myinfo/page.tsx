'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { AppDispatch, RootState } from '../store';
import { useDispatch, useSelector } from 'react-redux';
import { fetchUserThunk } from '../store/thunks/userThunks';
import { styled } from '@mui/material/styles';
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
    tableCellClasses,
    Paper
} from '@mui/material';

const MyInfoPage = () => {
    const router = useRouter();
    const dispatch: AppDispatch = useDispatch();
    const { isAuthenticated, secret } = useSelector((state: RootState) => state.auth);
    const { user, error: userError } = useSelector((state: RootState) => state.user);
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
    }, [user]);

    useEffect(() => {
        if (userError) {
            setError(userError);
        }
    }, [userError]);

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

    const StyledTableRowHead = styled(TableCell)(({ theme }) => ({
        [`&.${tableCellClasses.body}`]: {
            backgroundColor: theme.palette.common.black,
            color: theme.palette.common.white,
            fontSize: 14,
            width: '25%',
            minWidth: '180px',
        },
    }));

    const StyledTableCell = styled(TableCell)(({ theme }) => ({
        [`&.${tableCellClasses.head}`]: {
            backgroundColor: theme.palette.common.black,
            color: theme.palette.common.white,
        },
        [`&.${tableCellClasses.body}`]: {
            fontSize: 14,
        },
    }));

    const StyledTableRow = styled(TableRow)(({ theme }) => ({
        '&:nth-of-type(odd)': {
            backgroundColor: theme.palette.action.hover,
        },
        // hide last border
        '&:last-child td, &:last-child th': {
            border: 0,
        },
    }));

    if (!isClient) {
        return null;
    }

    return (
        <Container maxWidth="lg">
            <Box mt={4}>                
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <Typography variant="h5" gutterBottom>
                        My Info
                    </Typography>
                    <Button variant="contained" color="primary" onClick={fetchUser}>
                        Refresh
                    </Button>
                </Box>
                <Tabs value={tabIndex} onChange={handleTabChange} variant="scrollable" scrollButtons="auto">
                    <Tab label="General Info" />
                    <Tab label="Configuration" />
                </Tabs>
                {tabIndex === 0 && (
                    <Box
                        sx={{
                            padding: '16px',
                            backgroundColor: 'white',
                        }}>
                        {loading ? (
                            <CircularProgress />
                        ) : (
                            <>
                                <Box mt={2}>
                                    <TableContainer component={Paper}>
                                        <Table size="small">
                                            <TableBody>
                                                <TableRow>
                                                    <StyledTableRowHead>Username</StyledTableRowHead>
                                                    <StyledTableCell>{user?.userName}</StyledTableCell>
                                                </TableRow>
                                                <TableRow>
                                                    <StyledTableRowHead>Display Name</StyledTableRowHead>
                                                    <StyledTableCell>{user?.displayName}</StyledTableCell>
                                                </TableRow>
                                                <TableRow>
                                                    <StyledTableRowHead>Email</StyledTableRowHead>
                                                    <StyledTableCell>{user?.email}</StyledTableCell>
                                                </TableRow>
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                </Box>
                                <Box mt={2}>
                                    <Typography variant="h6">Logins</Typography>
                                    <TableContainer component={Paper}>
                                        <Table size="small">
                                            <TableHead>
                                                <TableRow>
                                                    <StyledTableCell>Provider</StyledTableCell>
                                                    <StyledTableCell>Type</StyledTableCell>
                                                    <StyledTableCell>Login Name</StyledTableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {user?.logins.map((login) => (
                                                    <StyledTableRow key={login.id}>
                                                        <StyledTableCell>{login.loginProvider}</StyledTableCell>
                                                        <StyledTableCell>{login.loginType}</StyledTableCell>
                                                        <StyledTableCell>{login.loginName}</StyledTableCell>
                                                    </StyledTableRow >
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                </Box>
                                <Box mt={2}>
                                    <Typography variant="h6">Roles</Typography>
                                    <TableContainer component={Paper}>
                                        <Table size="small">
                                            <TableHead>
                                                <TableRow>
                                                    <StyledTableCell>Role Name</StyledTableCell>
                                                    <StyledTableCell>Role Description</StyledTableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {user?.roles.map((role) => (
                                                    <StyledTableRow key={role.id}>
                                                        <StyledTableCell>{role.roleName}</StyledTableCell>
                                                        <StyledTableCell>{role.description}</StyledTableCell>
                                                    </StyledTableRow >
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                </Box>
                                <Box mt={2}>
                                    {message && <Typography color="success.main">{message}</Typography>}
                                    {error && <Typography color="error.main">{error}</Typography>}
                                </Box>
                            </>
                        )}
                    </Box>
                )}
                {tabIndex === 1 && (
                    <Box
                        sx={{
                            padding: '16px',
                            backgroundColor: 'white',
                        }}>
                        <Typography variant="body1">Configuration content goes here...</Typography>
                    </Box>
                )}
            </Box>
        </Container>
    );
}

export default MyInfoPage;