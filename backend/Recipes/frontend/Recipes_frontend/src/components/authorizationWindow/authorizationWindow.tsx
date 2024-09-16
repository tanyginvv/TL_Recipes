import styles from "./authorizationWindow.module.css";
import exit from "../../assets/images/exit.svg";
import useStore from "../../store/store";
import { useState } from "react";
import { AuthenticationService } from "../../services/authService";

export const AuthorizationWindow = () => {
    const { setRegistrationWindowOpen, setAuthorizationWindowOpen, setAccessToken, setNotification } = useStore();
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const authService = new AuthenticationService();

    const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (!login || !password) {
            setErrorMessage("Пожалуйста, заполните все поля.");
            return;
        }

        try {
            const credentials = { Login: login, Password: password };
            const tokenData = await authService.authentication(credentials);

            if (tokenData.accessToken) {
                setAccessToken(tokenData.accessToken);
                setAuthorizationWindowOpen(false);
                setNotification("Вы успешно вошли в систему", "success")
            } else {
                setErrorMessage(String(tokenData?.errorMessage));
            }
        } catch (error) {
            setErrorMessage("Ошибка при авторизации. Попробуйте еще раз.");
        }
    };

    const setRegister = () => {
        setRegistrationWindowOpen(true);
        setAuthorizationWindowOpen(false);
    };

    const displayErrorMessage = errorMessage.startsWith("Error:") ? errorMessage.replace("Error:", "").trim() : errorMessage;

    const loginInputClass = !login && errorMessage ? `${styles.inputText} ${styles.errorBorder}` : styles.inputText;
    const passwordInputClass = !password && errorMessage ? `${styles.inputText} ${styles.errorBorder}` : styles.inputText;

    return (
        <div className={styles.authOverlay}>
            <div className={styles.authContainer}>
                <span className={styles.exitButtonContainer}>
                    <button onClick={() => setAuthorizationWindowOpen(false)} className={styles.exitButton}>
                        <img src={exit} alt="Close" />
                    </button>
                </span>
                <span className={styles.authFormContainer}>
                    <h3 className={styles.authHeader}>Войти</h3>
                    <form className={styles.authForm} onSubmit={handleLogin}>
                        <input 
                            type="text" 
                            placeholder="Логин" 
                            className={loginInputClass} 
                            value={login} 
                            onChange={(e) => setLogin(e.target.value)}
                        />
                        <input 
                            type="password" 
                            placeholder="Пароль" 
                            className={passwordInputClass} 
                            value={password} 
                            onChange={(e) => setPassword(e.target.value)}
                        />
                        {errorMessage && <p className={styles.authError}>{displayErrorMessage}</p>}
                        <span className={styles.authButtons}>
                            <button type="submit" className={styles.submitButton}>Войти</button>
                            <button type="reset" className={styles.resetButton} onClick={() => {
                                setLogin("");
                                setPassword("");
                                setErrorMessage("");
                            }}>Отмена</button>
                        </span>
                    </form>
                </span>
                <button onClick={setRegister} className={styles.registrationLink}>У меня еще нет аккаунта</button>
            </div>
        </div>
    );
};
