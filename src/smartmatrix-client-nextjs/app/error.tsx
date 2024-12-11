'use client';

import React, { useEffect } from 'react';

interface ErrorProps {
  error: Error;
  reset: () => void;
}

const ErrorPage: React.FC<ErrorProps> = ({ error, reset }) => {
  useEffect(() => {
    console.error(error);
  }, [error]);

  return (
    <div>
      <h2>Something went wrong!</h2>
      <button onClick={() => reset()}>Try again</button>
    </div>
  );
};

export default ErrorPage;