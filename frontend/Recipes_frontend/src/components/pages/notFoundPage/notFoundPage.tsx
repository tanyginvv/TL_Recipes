import { Link } from 'react-router-dom';
import styles from './notFoundPage.module.css';

export const NotFoundPage = () => {
    return (
        <div className={styles.notFoundContainer}>
            <h1 className={styles.heading}>404</h1>
            <p className={styles.message}>Страница не найдена</p>
            <p className={styles.description}>
                К сожалению, страница, которую вы ищете, не существует.
            </p>
            <Link to="/homePage" className={styles.homeLink}>Вернуться на главную</Link>
        </div>
    );
};
