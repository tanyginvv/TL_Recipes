import styles from "./header.module.css"
import login from "../../assets/images/login.svg"
import { Link } from "react-router-dom"
export const Header = () => {
    return (
        <>
            <div className={styles.headerBody}>
                <div className={styles.headerMenu}>
                    <p className={styles.menuIcon}>Recipes</p>
                    <div className={styles.menuButtons}>
                        <Link to='/homePage'><button className={styles.menuButton}>Главная</button></Link>
                        <Link to='/allRecipesPage'><button className={styles.menuButton}>Рецепты</button></Link>
                        <button className={styles.menuButton}>Избранное</button>
                    </div>
                </div>
                <div className={styles.headerLoginInfo}>
                    <img className={styles.loginInfoIcon} src={login}/>
                    <button className={styles.loginText}>Войти</button>
                </div>
            </div>
        </>
    )
}