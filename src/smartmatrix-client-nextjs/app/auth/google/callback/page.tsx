'use client';

import { useEffect } from 'react';
import axios from 'axios';
import { useRouter } from 'next/navigation';

const GoogleCallback = () => {
    const router = useRouter();

    useEffect(() => {
        const fetchToken = async () => {
            try {
                const response = await axios.get('https://localhost:5001/auth/google/callback', { withCredentials: true });
                localStorage.setItem('token', response.data.token);
                router.push('/'); // Redirect to home or dashboard
            } catch (error) {
                console.error(error);
                // Handle error (e.g. redirect to an error page or show a message)
            }
        };

        fetchToken();
    }, [router]);

    return <div>Loading...</div>;
};

export default GoogleCallback;