"use client";

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Button from '@mui/material/Button';
import { DataGrid, GridColDef, GridToolbarContainer, GridToolbarColumnsButton, GridToolbarFilterButton, GridToolbarDensitySelector, GridToolbarExport } from '@mui/x-data-grid';
import Paper from '@mui/material/Paper';
import { WeatherForecast } from '../../types/demos/weatherForecastTypes';

export default function WeatherForecastPage() {
    const [weatherData, setWeatherData] = useState<WeatherForecast[]>([]);
    const [loading, setLoading] = useState(false);
    const router = useRouter();

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

    const backToHome = () => {
        router.push('/'); // Navigate back to home using the router
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
            <p className="text-sm text-gray-500 mb-4">Demo: Weather Forecast</p>
            <div className="flex gap-4 items-center flex-col sm:flex-row mb-6">
                <Button
                    variant="contained"
                    onClick={backToHome}
                >
                    Back To Home
                </Button>

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
                &copy; 2024 SmartMatrix. All rights reserved.
            </footer>
        </main>
    );
}