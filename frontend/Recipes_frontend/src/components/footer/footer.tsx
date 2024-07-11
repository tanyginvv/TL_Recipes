import styles from './footer.module.css';

const Footer = () => {
    return (
        <div className={styles.footer}>
            <p className={styles.footerIcon}>Recipes</p>
            <p className={styles.footerText}>© Recipes 2024</p>
        </div>
    );
};

export default Footer;
