import styles from "./header.module.css";
import login from "../../assets/images/login.svg";
import { Link, useLocation } from "react-router-dom";

export const Header = () => {
    const location = useLocation();

    return (
        <>
            <div className={styles.headerBody}>
                <div className={styles.headerMenu}>
                    <Link to='/homePage'>
                        <button className={styles.menuIcon}>
                            Recipes
                        </button>
                    </Link>
                    <div className={styles.menuButtons}>
                        <Link to='/homePage'>
                            <button className={location.pathname === '/homePage' ? styles.menuButtonBold : styles.menuButton}>
                                Главная
                            </button>
                        </Link>
                        <Link to='/allRecipesPage'>
                            <button className={location.pathname != '/homePage' ? styles.menuButtonBold : styles.menuButton}>
                                Рецепты
                            </button>
                        </Link>
                        <button className={styles.menuButton}>Избранное</button>
                    </div>
                </div>
                <div className={styles.headerLoginInfo}>
                    <img className={styles.loginInfoIcon} src={login} />
                    <button className={styles.loginText}>Войти</button>
                </div>
            </div>
        </>
    );
};