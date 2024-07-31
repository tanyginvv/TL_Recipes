import { useEffect, useState } from "react";
import styles from "./header.module.css";
import login from "../../assets/images/login.svg";
import logout from "../../assets/images/logout.svg";
import { Link, useLocation, useNavigate } from "react-router-dom";
import useStore from "../../store/store";
import { UserService } from "../../services/userService";
import { AuthenticationService } from "../../services/authService";
import { IUser } from "../../models/types";

export const Header = () => {
    const location = useLocation();
    const navigate = useNavigate(); 
    const [user, setUser] = useState<IUser | undefined>(undefined);

    const {
        setAuthOrRegistrWindowOpen,
        userId,
        setUserId
    } = useStore();
    
    const userService = new UserService();
    const authService = new AuthenticationService();

    useEffect(() => {
        const fetchUser = async () => {
            if (userId) {
                const fetchedUser = await userService.fetchUser(userId);
                setUser(fetchedUser);
            }
        };

        fetchUser();
    }, [userId]);

    const logOut = async () => {
        await authService.logout(); 
        setUserId(null); 
        setUser(undefined); 
        navigate('/'); 
    }

    return (
        <div className={styles.headerBody}>
            <div className={styles.headerMenu}>
                <Link to='/homePage'>
                    <button className={styles.menuIcon}>
                        Recipes
                    </button>
                </Link>
                <div className={styles.menuButtons}>
                    <Link to='/homePage'>
                        <button 
                            className={location.pathname === '/homePage' ? styles.menuButtonBold : styles.menuButton}
                        >
                            Главная
                        </button>
                    </Link>
                    <Link to='/allRecipesPage'>
                        <button 
                            className={location.pathname !== '/homePage' ? styles.menuButtonBold : styles.menuButton}
                        >
                            Рецепты
                        </button>
                    </Link>
                    <button className={styles.menuButton}>Избранное</button>
                </div>
            </div>
            <div className={styles.headerLoginInfo}>
                <img className={styles.loginInfoIcon} src={login} alt="Login icon" />
                {userId ? 
                    <span className={styles.loginInfo}>
                        <p className={styles.loginGreeting}>{"Привет, " + user?.name}</p>
                        <img 
                            className={styles.logoutButton} 
                            onClick={logOut} 
                            src={logout} 
                            alt="Logout icon" 
                        />
                    </span>
                    :
                    <button 
                        onClick={() => setAuthOrRegistrWindowOpen(true)} 
                        className={styles.loginText}
                    >
                        Войти
                    </button>
                }
            </div>
        </div>
    );
};
