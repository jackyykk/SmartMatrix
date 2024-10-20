"use client";

import { useEffect, useState } from 'react';
import Image from 'next/image';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  summary: string;
}

export default function Home() {
  const [weatherData, setWeatherData] = useState<WeatherForecast[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchWeatherData = async () => {
    try {
      const response = await fetch('https://smartmatrix.azurewebsites.net/api/tests/WeatherForecast');
      const data = await response.json();
      setWeatherData(data);
    } catch (error) {
      console.error('Error fetching weather data:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchWeatherData();
  }, []);

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Weather Forecast</h1>
      <div className="flex gap-4 items-center flex-col sm:flex-row mb-6">
        <button
          onClick={fetchWeatherData}
          className="mb-6 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          Refresh Weather Data
        </button>        
      </div>
      
      <div className="overflow-x-auto">
        {loading ? (
          <p>Loading...</p>
        ) : (
          <table className="min-w-full bg-white dark:bg-gray-800 shadow-md rounded-lg overflow-hidden">
            <thead className="bg-gray-200 dark:bg-gray-700">
              <tr>
                <th className="px-4 py-2">#</th>
                <th className="px-4 py-2">Date</th>
                <th className="px-4 py-2">Temperature (C)</th>
                <th className="px-4 py-2">Summary</th>
              </tr>
            </thead>
            <tbody>
              {weatherData.map((forecast, index) => (
                <tr key={index} className="border-t dark:border-gray-700">
                  <td className="px-4 py-2">{index + 1}</td>
                  <td className="px-4 py-2">{forecast.date}</td>
                  <td className="px-4 py-2">{forecast.temperatureC}</td>
                  <td className="px-4 py-2">{forecast.summary}</td>
                </tr>
              ))}
            </tbody>
          </table>
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