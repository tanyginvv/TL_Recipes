import { useState } from "react";
import styles from "./registrationWindow.module.css";
import exit from "../../assets/images/exit.svg";
import useStore from "../../store/store";
import { AuthenticationService } from "../../services/authService";
import { IRegister } from "../../models/types"; 

export const RegistrationWindow = () => {
    const {
        setRegistrationWindowOpen,
        setAuthorizationWindowOpen,
        setAccessToken,
        setNotification
    } = useStore(); 

    const [name, setName] = useState('');
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [generalError, setGeneralError] = useState<string | null>(null);
    const [passwordError, setPasswordError] = useState<string | null>(null);
    const [fieldError, setFieldError] = useState<string | null>(null); 
    const authService = new AuthenticationService();

    const handleRegistration = async (event: React.FormEvent) => {
        event.preventDefault();

        setGeneralError(null);
        setPasswordError(null);
        setFieldError(null);

        if (!name || !login || !password || !confirmPassword) {
            setFieldError('Пожалуйста, заполните все поля.');
            return;
        }

        if (password !== confirmPassword) {
            setPasswordError('Пароли не совпадают');
            return;
        }

        const registrationData: IRegister = {
            Name: name,
            Login: login,
            Password: password,
            Description : ""
        };

        try {
            const tokenData = await authService.register(registrationData);

            if (tokenData.accessToken) {
                setAccessToken(tokenData.accessToken);
                setRegistrationWindowOpen(false);
                setAuthorizationWindowOpen(false);
                setNotification("Вы успешно зарегистрировались", "success")
            } else {
                setGeneralError(String(tokenData.errorMessage));
            }
        } catch (e) {
            setGeneralError('Ошибка регистрации. Попробуйте снова.');
        }
    };

    const handleCancel = () => {
        setName("");
        setLogin("");
        setPassword("");
        setConfirmPassword("");
        setFieldError(null);
        setPasswordError(null);
    };

    const setAuth = () => {
        setAuthorizationWindowOpen(true);
        setRegistrationWindowOpen(false);
    };

    const nameInputClass = !name && fieldError ? `${styles.inputText} ${styles.errorBorder}` : styles.inputText;
    const loginInputClass = !login && fieldError ? `${styles.inputText} ${styles.errorBorder}` : styles.inputText;
    const passwordInputClass = !password && fieldError ? `${styles.inputPassword} ${styles.errorBorder}` : styles.inputPassword;
    const confirmPasswordInputClass = !confirmPassword && fieldError ? `${styles.inputPassword} ${styles.errorBorder}` : styles.inputPassword;

    return (
        <div className={styles.registrationOverlay}>
            <div className={styles.registrationContainer}>
                <span className={styles.exitButtonContainer}>
                    <button 
                        onClick={() => setRegistrationWindowOpen(false)} 
                        className={styles.exitButton}
                    >
                        <img src={exit} alt="Exit" />
                    </button>
                </span>
                <span className={styles.registrationFormContainer}>
                    <h3 className={styles.registrationHeader}>Регистрация</h3>
                    <form className={styles.registrationForm} onSubmit={handleRegistration}>
                        <input 
                            type="text" 
                            placeholder="Имя" 
                            className={nameInputClass}
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />
                        <input 
                            type="text" 
                            placeholder="Логин" 
                            className={loginInputClass}
                            value={login}
                            onChange={(e) => setLogin(e.target.value)}
                        />
                        {fieldError && <p className={styles.fieldError}>{fieldError}</p>}
                        {generalError && <p className={styles.generalError}>{generalError.slice(7)}</p>}
                        <span className={styles.passwordsContainer}>
                            <span className={styles.passwordBlock}>
                                <input 
                                    type="password" 
                                    placeholder="Пароль" 
                                    className={passwordInputClass}
                                    value={password}
                                    minLength={8}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                                <p className={styles.passwordSubtext}>Минимум 8 символов</p>
                            </span>
                            <span className={styles.passwordBlock}>
                                <input 
                                    type="password" 
                                    placeholder="Повторите пароль" 
                                    className={confirmPasswordInputClass}
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                />
                                {passwordError && <p className={styles.passwordError}>{passwordError}</p>}
                            </span>
                        </span>
                        <span className={styles.registrationButtons}>
                            <button 
                                type="submit" 
                                className={styles.submitButton}
                            >
                                Зарегистрироваться
                            </button>
                            <button 
                                type="reset" 
                                onClick={handleCancel} 
                                className={styles.resetButton}
                            >
                                Отмена
                            </button>
                        </span>
                    </form>
                </span>
                <button onClick={setAuth} className={styles.authLink}>
                    У меня уже есть аккаунт
                </button>
            </div>
        </div>
    );
};
