'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { useInView } from 'react-intersection-observer';
import Button from '@mui/material/Button';

export default function Home() {
  const router = useRouter();
  const [isClient, setIsClient] = useState(false);

  useEffect(() => {
    setIsClient(true);
  }, []);

  const { ref: section1Ref, inView: section1InView } = useInView({ triggerOnce: true });
  const { ref: section2Ref, inView: section2InView } = useInView({ triggerOnce: true });
  const { ref: section3Ref, inView: section3InView } = useInView({ triggerOnce: true });
  const { ref: section4Ref, inView: section4InView } = useInView({ triggerOnce: true });
  const { ref: section5Ref, inView: section5InView } = useInView({ triggerOnce: true });

  if (!isClient) {
    return null;
  }

  return (
    <main className="flex flex-col items-center justify-center min-h-screen p-4 bg-gray-100" suppressHydrationWarning={true}>
      <div className="background-lines">
        <div className="line" style={{ left: '10%' }}></div>
        <div className="line" style={{ left: '30%' }}></div>
        <div className="line" style={{ left: '50%' }}></div>
        <div className="line" style={{ left: '70%' }}></div>
        <div className="line" style={{ left: '90%' }}></div>
      </div>

      <section className="flex flex-col items-center justify-center min-h-screen p-4 bg-white">
        <motion.div
          ref={section1Ref}
          initial={{ opacity: 0, y: 50 }}
          animate={{ opacity: section1InView ? 1 : 0, y: section1InView ? 0 : 50 }}
          transition={{ duration: 0.5 }}
          className="text-center"
        >
          <h1 className="text-5xl font-bold text-gray-800 mb-4">Welcome to Smart Matrix</h1>
          <p className="text-lg text-gray-600 mb-8">Your platform for smart apps and solutions.</p>
          <Button variant="contained" color="primary" onClick={() => router.push('/main')}>
            Explore Apps
          </Button>
        </motion.div>
      </section>

      <section className="flex flex-col items-center justify-center min-h-screen p-4 bg-gray-100">
        <motion.div
          ref={section2Ref}
          initial={{ opacity: 0, y: 50 }}
          animate={{ opacity: section2InView ? 1 : 0, y: section2InView ? 0 : 50 }}
          transition={{ duration: 0.5 }}
          className="text-center"
        >
          <h2 className="text-4xl font-bold text-gray-800 mb-4">Why Choose Us?</h2>
          <p className="text-lg text-gray-600 mb-8">We provide innovative solutions to make your life easier.</p>
        </motion.div>
      </section>

      <section className="flex flex-col items-center justify-center min-h-screen p-4 bg-white">
        <motion.div
          ref={section3Ref}
          initial={{ opacity: 0, y: 50 }}
          animate={{ opacity: section3InView ? 1 : 0, y: section3InView ? 0 : 50 }}
          transition={{ duration: 0.5 }}
          className="text-center"
        >
          <h2 className="text-4xl font-bold text-gray-800 mb-4">Our Services</h2>
          <p className="text-lg text-gray-600 mb-8">We offer a wide range of smart solutions tailored to your needs.</p>
          <ul className="list-disc list-inside text-left">
            <li className="text-lg text-gray-600 mb-2">Custom App Development</li>
            <li className="text-lg text-gray-600 mb-2">AI and Machine Learning Solutions</li>
            <li className="text-lg text-gray-600 mb-2">Cloud Integration</li>
            <li className="text-lg text-gray-600 mb-2">IoT Solutions</li>
          </ul>
        </motion.div>
      </section>

      <section className="flex flex-col items-center justify-center min-h-screen p-4 bg-gray-100">
        <motion.div
          ref={section4Ref}
          initial={{ opacity: 0, y: 50 }}
          animate={{ opacity: section4InView ? 1 : 0, y: section4InView ? 0 : 50 }}
          transition={{ duration: 0.5 }}
          className="text-center"
        >
          <h2 className="text-4xl font-bold text-gray-800 mb-4">Testimonials</h2>
          <p className="text-lg text-gray-600 mb-8">Hear what our clients have to say about us.</p>
          <div className="flex flex-col sm:flex-row gap-4">
            <div className="bg-white p-4 rounded shadow-md">
              <p className="text-lg text-gray-600 mb-2">"Smart Matrix transformed our business with their innovative solutions."</p>
              <p className="text-sm text-gray-500">- Client A</p>
            </div>
            <div className="bg-white p-4 rounded shadow-md">
              <p className="text-lg text-gray-600 mb-2">"Their team is highly skilled and professional."</p>
              <p className="text-sm text-gray-500">- Client B</p>
            </div>
            <div className="bg-white p-4 rounded shadow-md">
              <p className="text-lg text-gray-600 mb-2">"We highly recommend Smart Matrix for any smart solution needs."</p>
              <p className="text-sm text-gray-500">- Client C</p>
            </div>
          </div>
        </motion.div>
      </section>

      <section className="flex flex-col items-center justify-center min-h-screen p-4 bg-white">
        <motion.div
          ref={section5Ref}
          initial={{ opacity: 0, y: 50 }}
          animate={{ opacity: section5InView ? 1 : 0, y: section5InView ? 0 : 50 }}
          transition={{ duration: 0.5 }}
          className="text-center"
        >
          <h2 className="text-4xl font-bold text-gray-800 mb-4">Get Started Today</h2>
          <p className="text-lg text-gray-600 mb-8">Join us and start using our smart apps now.</p>
          <Button variant="contained" color="primary" onClick={() => router.push('/login')}>
            Sign Up
          </Button>
        </motion.div>
      </section>

      <footer className="mt-6 flex gap-6 flex-wrap text-gray-600 items-center justify-center">
        &copy; 2024 SmartMatrix. All rights reserved.
      </footer>
    </main>
  );
}