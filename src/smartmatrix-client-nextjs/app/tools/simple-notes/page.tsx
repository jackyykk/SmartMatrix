'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Button from '@mui/material/Button';

const SimpleNotesPage = () => {
    const [isClient, setIsClient] = useState(false);
    const router = useRouter();

    useEffect(() => {
        setIsClient(true);
    }, []);

    const backToMain = () => {
        router.push('/main/'); // Navigate back to main using the router
    };

    if (!isClient) {
        return null;
    }

    return (
        <main className="min-h-screen p-4 bg-gray-100" suppressHydrationWarning={true}>
            <p className="text-sm text-gray-500 mb-4">Tool: Simple Notes</p>
            <div className="flex gap-4 items-center flex-col sm:flex-row mb-6">
                <Button
                    variant="contained"
                    onClick={backToMain}
                >
                    Back To Main
                </Button>
            </div>

            <footer className="mt-6 flex gap-6 flex-wrap text-gray-600 items-center justify-center">
                &copy; 2024 SmartMatrix. All rights reserved.
            </footer>
        </main>
    );
}

export default SimpleNotesPage;