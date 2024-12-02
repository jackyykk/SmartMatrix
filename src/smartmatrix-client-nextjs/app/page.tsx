'use client';

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
  const router = useRouter();
  const [isClient, setIsClient] = useState(false);
  
  useEffect(() => {
    setIsClient(true);    
  }, []);

  if (isClient) {
    return (
      <main className="flex flex-col items-center justify-center min-h-screen p-4 bg-gray-100" suppressHydrationWarning={true}>        
        <div className="flex flex-col sm:flex-row items-center gap-4 mb-8">
          
        </div>
        <footer className="mt-6 flex gap-6 flex-wrap text-gray-600 items-center justify-center">
          &copy; 2024 SmartMatrix. All rights reserved.
        </footer>
      </main>
    );
  }
  else {
    return <></>;
  }
}



