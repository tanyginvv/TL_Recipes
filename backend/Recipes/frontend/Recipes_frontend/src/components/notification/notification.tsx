import { useEffect } from 'react';
import styles from './notification.module.css';

interface NotificationProps {
    message: string;
    status: string;
    onClose: () => void;
}

export const Notification = ({ message, status, onClose }: NotificationProps) => {
    useEffect(() => {
        const timer = setTimeout(onClose, 3000); 
        return () => clearTimeout(timer);
    }, [onClose]);

    return (
        <div className={`${styles.notification} ${styles[status]}`} onClick={onClose}>
            {message}
            <div className={styles.progressBar}></div>
        </div>
    );
};
