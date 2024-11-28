"use client";

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Button from '@mui/material/Button';

export default function MainPage() {
    const router = useRouter();

    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        setMessage('');
        setError('');

    }, [router]);

    const backToHome = () => {
        router.push('/'); // Navigate back to home using the router
    };

    return (
        <main className="min-h-screen p-4 bg-gray-100">
            <p className="text-sm text-gray-500 mb-4">Main Page</p>
            <div className="flex gap-4 items-center flex-col sm:flex-row mb-6">
                <Button variant="contained" onClick={backToHome}>
                    Back To Home
                </Button>
            </div>
            <div>
                
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