import styles from "./authOrRegistrWindow.module.css"
import exit from "../../assets/images/exit.svg"
import useStore from "../../store/store"

export const AuthOrRegistrWindow = () => {

    const {
        setAuthOrRegistrWindowOpen,
        setRegistrationWindowOpen,
        setAuthorizationWindowOpen
    } = useStore(); 


    const setAuth = () => {
        setAuthOrRegistrWindowOpen(false) ;
        setAuthorizationWindowOpen(true)
    }

    const setRegister = () => {
        setAuthOrRegistrWindowOpen(false) ;
        setRegistrationWindowOpen(true)
    }
    return (
        <div className={styles.authOverlay}>
            <div className={styles.authContainer}>
                <span className={styles.exitButtonContainer}>
                    <button onClick={() => setAuthOrRegistrWindowOpen(false) }className={styles.exitButton}><img src={exit}/></button>
                </span>
                <span className={styles.authFormContainer}>
                <h3 className={styles.authHeader}>Войдите в профиль</h3>
                <p className={styles.authDescription}>Добавлять рецепты могут только зарегистрированные пользователи. </p>
                <div className={styles.authForm}>
                    <span className={styles.buttons}>
                        <button onClick={setAuth} className={styles.loginButton}>Войти</button>
                        <button onClick={setRegister} className={styles.registrButton}>Регистрация</button>
                    </span>
                </div>
                </span>
            </div>
    </div>
    )
}