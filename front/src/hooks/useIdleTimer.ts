import { useEffect, useRef } from 'react';

type IdleCallback = () => void;

// 7200000 = 2h
const useIdleTimer = (onIdle: IdleCallback, timeout: number = 7200000): void => {
    const timerRef = useRef<number | null>(null);
    const debounceRef = useRef<number | null>(null);

    const resetTimer = () => {
        if (timerRef.current) {
            clearTimeout(timerRef.current);
        }
        timerRef.current = setTimeout(onIdle, timeout);
    };

    const handleActivity = () => {
        if (debounceRef.current) {
            clearTimeout(debounceRef.current);
        }
        debounceRef.current = setTimeout(resetTimer, 200);
    };

    useEffect(() => {
        window.addEventListener('mousemove', handleActivity);
        window.addEventListener('mousedown', handleActivity);
        window.addEventListener('keypress', handleActivity);
        window.addEventListener('scroll', handleActivity);
        window.addEventListener('touchstart', handleActivity);

        resetTimer();

        return () => {
            if (timerRef.current) {
                clearTimeout(timerRef.current);
            }
            if (debounceRef.current) {
                clearTimeout(debounceRef.current);
            }
            window.removeEventListener('mousemove', handleActivity);
            window.removeEventListener('mousedown', handleActivity);
            window.removeEventListener('keypress', handleActivity);
            window.removeEventListener('scroll', handleActivity);
            window.removeEventListener('touchstart', handleActivity);
        };
    }, [onIdle, timeout]);

};

export default useIdleTimer;