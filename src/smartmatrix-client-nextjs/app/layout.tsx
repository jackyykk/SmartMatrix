'use client';

import localFont from "next/font/local";
import { config } from '@fortawesome/fontawesome-svg-core';
import "./globals.css";
import { Provider } from 'react-redux';
import store from './store'; // Adjust the import path as necessary
import MyAppBar from './components/MyAppBar'; // Import the App Bar component

config.autoAddCss = false; // Tell Font Awesome to skip adding the CSS automatically since it's already imported

const geistSans = localFont({
  src: "./fonts/GeistVF.woff",
  variable: "--font-geist-sans",
  weight: "100 900",
});
const geistMono = localFont({
  src: "./fonts/GeistMonoVF.woff",
  variable: "--font-geist-mono",
  weight: "100 900",
});

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html>
      <head>
        <meta charSet="UTF-8" />
        <meta name="viewport" content="initial-scale=1, width=device-width" />
        <title>Smart Matrix</title>
        <meta name="description" content='Your intelligent data management solution' />
      </head>
      <body className={`${geistSans.variable} ${geistMono.variable} antialiased`}>
        <Provider store={store}>
          <MyAppBar /> {/* Add the App Bar component */}
          {children}
        </Provider>
      </body>
    </html>
  );
}
