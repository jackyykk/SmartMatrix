"use client";

import { useRouter } from 'next/navigation';
import Button from '@mui/material/Button';

export default function NotesPage() {
    const router = useRouter();

    const backToHome = () => {
        router.push('/'); // Navigate back to home using the router
    };

    return (
        <main className="p-4">
            <p className="text-sm text-gray-500 mb-4">Demo: Notes</p>
            <div className="flex gap-4 items-center flex-col sm:flex-row mb-6">
                <Button
                    variant="contained"
                    onClick={backToHome}
                >
                    Back To Home
                </Button>
            </div>

            <footer className="mt-6 flex gap-6 flex-wrap items-center justify-center">
                &copy; 2024 SmartMatrix. All rights reserved.
            </footer>
        </main>
    );
}