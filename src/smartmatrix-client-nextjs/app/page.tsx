"use client";

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import packageJson from '../package.json';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select, { SelectChangeEvent } from '@mui/material/Select';

export default function Home() {

  const [demo, setDemo] = useState('');
  const router = useRouter();

  const handleDemoChange = (event: SelectChangeEvent) => {
    setDemo(event.target.value as string);
  };

  const gotoPage = () => {
    router.push(`/demos/${demo}`);
  };

  useEffect(() => {
    setDemo('weather-forecast');
  }, []);

  return (
    <main className="flex flex-col items-center justify-center min-h-screen p-4 bg-gray-100">
      <h1 className="text-4xl text-gray-600 mb-4 text-center">Smart Matrix</h1>
      <p className="text-lg text-gray-600 mb-8 text-center">Version: {packageJson.version}</p>
      <div className="flex flex-col sm:flex-row items-center gap-4 mb-8">
        <Box sx={{ minWidth: 200 }}>
          <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">Demo</InputLabel>
            <Select
              labelId="demo-simple-select-label"
              id="demo-simple-select"
              value={demo}
              label="Demo"
              onChange={handleDemoChange}
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
      <footer className="mt-6 flex gap-6 flex-wrap items-center justify-center">
        &copy; 2024 SmartMatrix. All rights reserved.
      </footer>
    </main>
  );
}



