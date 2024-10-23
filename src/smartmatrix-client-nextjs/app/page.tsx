"use client";

import { useEffect, useState } from 'react';
import packageJson from '../package.json';
import Button from '@mui/material/Button';
import { DataGrid, GridColDef, GridToolbarContainer, GridToolbarColumnsButton, GridToolbarFilterButton, GridToolbarDensitySelector, GridToolbarExport } from '@mui/x-data-grid';
import Paper from '@mui/material/Paper';

interface WeatherForecast {
  id: number;
  date: string;
  temperatureC: number;
  summary: string;
}

export default function Home() {  
  const [weatherData, setWeatherData] = useState<WeatherForecast[]>([]);
  const [loading, setLoading] = useState(false);

  const fetchWeatherData = async () => {
    try {
      if (loading) return;      
      setLoading(true);
      const url = process.env.NEXT_PUBLIC_APISERVER_BASE_URL + '/api/demos/WeatherForecast';
      const response = await fetch(url);
      const data = await response.json();
      // fill in the id field
      data.forEach((item: WeatherForecast, index: number) => {
        item.id = index + 1;
      });
      setWeatherData(data);
    } catch (error) {
      console.error('Error fetching weather data:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchWeatherData();
  }, []); // Empty dependency array ensures this runs only once
  
  // table definition
  const columns: GridColDef[] = [
    { field: 'id', headerName: 'Id', width: 100 },
    { field: 'temperatureC', headerName: 'Temperature (C)', width: 300 },
    { field: 'summary', headerName: 'Summary', width: 300 },    
  ];

  const paginationModel = { page: 0, pageSize: 5 };

  function CustomToolbar() {
    return (
      <GridToolbarContainer>
        <GridToolbarColumnsButton />
        <GridToolbarFilterButton />
        <GridToolbarDensitySelector />
        <GridToolbarExport />
      </GridToolbarContainer>
    );
  }

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">{packageJson.name}</h1>
      <p className="text-sm text-gray-500 mb-4">Version: {packageJson.version}</p>
      <p className="text-sm text-gray-500 mb-4">Module: Weather Forecast</p>
      <div className="flex gap-4 items-center flex-col sm:flex-row mb-6">
        <Button
          variant="contained"
          onClick={fetchWeatherData}
        >
          Refresh Weather Data
        </Button>        
      </div>
      
      <div className="overflow-x-auto">
        {loading ? (
          <p>Loading...</p>
        ) : (
          <Paper sx={{ width: '100%' }}>
            <DataGrid
              rows={weatherData}
              columns={columns}
              initialState={{ pagination: { paginationModel } }}
              pageSizeOptions={[5, 10]}
              checkboxSelection
              sx={{ border: 0 }}
              slots={{ toolbar: CustomToolbar }}
            />
          </Paper>          
        )}
      </div>
      <footer className="mt-6 flex gap-6 flex-wrap items-center justify-center">
        <a
          className="rounded-full border border-solid border-black/[.08] dark:border-white/[.145] transition-colors flex items-center justify-center hover:bg-[#f2f2f2] dark:hover:bg-[#1a1a1a] hover:border-transparent text-sm sm:text-base h-10 sm:h-12 px-4 sm:px-5 sm:min-w-44"
          href="https://nextjs.org/docs?utm_source=create-next-app&utm_medium=appdir-template-tw&utm_campaign=create-next-app"
          target="_blank"
          rel="noopener noreferrer"
        >
          Read our docs
        </a>
        <a
          className="rounded-full border border-solid border-black/[.08] dark:border-white/[.145] transition-colors flex items-center justify-center hover:bg-[#f2f2f2] dark:hover:bg-[#1a1a1a] hover:border-transparent text-sm sm:text-base h-10 sm:h-12 px-4 sm:px-5 sm:min-w-44"
          href="https://nextjs.org/learn?utm_source=create-next-app&utm_medium=appdir-template-tw&utm_campaign=create-next-app"
          target="_blank"
          rel="noopener noreferrer"
        >          
          Learn Next.js
        </a>
      </footer>
    </main>
  );
}



