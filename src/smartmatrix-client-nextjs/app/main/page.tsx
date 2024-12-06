'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import packageJson from '../../package.json';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select, { SelectChangeEvent } from '@mui/material/Select';

const MainPage = () => {
    const router = useRouter();
    const [isClient, setIsClient] = useState(false);
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    const [app, setApp] = useState('');

    const handleAppChange = (event: SelectChangeEvent) => {
        setApp(event.target.value as string);
    };

    const gotoPage = () => {
        router.push(`/apps/${app}`);
    };

    useEffect(() => {
        setIsClient(true);
        setMessage('');
        setError('');
        setApp('weather-forecast');
    }, []);

    const backToHome = () => {
        router.push('/'); // Navigate back to home using the router
    };

    if (!isClient) {
        return null;
    }

    return (
        <main className="flex flex-col items-center justify-center min-h-screen p-4 bg-gray-100" suppressHydrationWarning={true}>
            <h1 className="text-4xl text-gray-600 mb-4 text-center">Smart Matrix</h1>
            <p className="text-lg text-gray-600 mb-8 text-center">Version: {packageJson.version}</p>
            <p className="text-sm text-gray-500 mb-8 text-center">Main Page</p>
            <div className="flex gap-4 items-center flex-col sm:flex-row mb-6">
                <Button variant="contained" onClick={backToHome}>
                    Back To Home
                </Button>
            </div>
            <div className="flex flex-col sm:flex-row items-center gap-4 mb-8">
                <Box sx={{ minWidth: 200 }}>
                    <FormControl fullWidth>
                        <InputLabel id="app-simple-select-label">App</InputLabel>
                        <Select
                            labelId="app-simple-select-label"
                            id="app-simple-select"
                            value={app}
                            label="App"
                            onChange={handleAppChange}
                        >
                            <MenuItem value={'weather-forecast'}>Weather Forecast</MenuItem>
                            <MenuItem value={'simple-notes'}>Simple Notes</MenuItem>
                        </Select>
                    </FormControl>
                </Box>
                <Button variant="contained" color="primary" onClick={gotoPage}>
                    Go
                </Button>
            </div>
            <div>
                {message && <p className="mt-4 text-green-600">{message}</p>}
                {error && <p className="mt-4 text-red-600">{error}</p>}
            </div>
            <footer className="mt-6 flex gap-6 flex-wrap text-gray-600 items-center justify-center">
                &copy; 2024 SmartMatrix. All rights reserved.
            </footer>
        </main>
    );
}

export default MainPage;