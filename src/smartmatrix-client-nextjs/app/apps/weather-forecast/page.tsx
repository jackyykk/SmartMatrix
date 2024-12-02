'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Button from '@mui/material/Button';
import { DataGrid, GridColDef, GridToolbarContainer, GridToolbarColumnsButton, GridToolbarFilterButton, GridToolbarDensitySelector, GridToolbarExport } from '@mui/x-data-grid';
import Paper from '@mui/material/Paper';
import { WeatherForecast_OutputPayload } from './types/weatherForecastTypes';

export default function WeatherForecastPage() {
    const [isClient, setIsClient] = useState(false);
    const [weatherData, setWeatherData] = useState<WeatherForecast_OutputPayload[]>([]);
    const [loading, setLoading] = useState(false);
    const router = useRouter();

    useEffect(() => {
        setIsClient(true);
        fetchWeatherData();
    }, []); // Empty dependency array ensures this runs only once

    const fetchWeatherData = async () => {
        try {
            if (loading) return;
            setLoading(true);
            const url = process.env.NEXT_PUBLIC_APISERVER_BASE_URL + '/api/apps/weather_forecast_app/getlist';
            const response = await fetch(url);
            const result = await response.json();
            if (result.succeeded && result.data) {
                const data = result.data.weatherForecasts
                // fill in the id field
                data.forEach((item: WeatherForecast_OutputPayload, index: number) => {
                    item.id = index + 1;
                });
                setWeatherData(data);
            }
            else {
                console.error('Error fetching weather data:', result.statusCode);
                return;
            }
        } catch (error) {
            console.error('Error fetching weather data:', error);
        } finally {
            setLoading(false);
        }
    };

    const backToMain = () => {
        router.push('/main/'); // Navigate back to main using the router
    };

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

    if (isClient) {
        return (
            <main className="min-h-screen p-4 bg-gray-100" suppressHydrationWarning={true}>
                <p className="text-sm text-gray-500 mb-4">App: Weather Forecast</p>
                <div className="flex gap-4 items-center flex-col sm:flex-row mb-6">
                    <Button
                        variant="contained"
                        onClick={backToMain}
                    >
                        Back To Main
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
                <footer className="mt-6 flex gap-6 flex-wrap text-gray-600 items-center justify-center">
                    &copy; 2024 SmartMatrix. All rights reserved.
                </footer>
            </main>
        )
    }
    else {
        return <></>;
    }
}