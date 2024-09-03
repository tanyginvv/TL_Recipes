import { useEffect, useState } from "react";
import styles from "./header.module.css";
import login from "../../assets/images/login.svg";
import logout from "../../assets/images/logout.svg";
import { Link, useLocation, useNavigate } from "react-router-dom";
import useStore from "../../store/store";
import { AuthenticationService } from "../../services/authService";
import { UserService } from "../../services/userService";
import { IName } from "../../models/types";

export const Header = () => {
    const location = useLocation();
    const navigate = useNavigate(); 

    const {
        userId,
        setAuthorizationWindowOpen,
        setUserId,
        setNotification
    } = useStore();
    
    const [ userName, setUserName] = useState<IName>()
    const authService = new AuthenticationService();
    const userService = new UserService();

    useEffect(() => {
        const fetchUserData = async () => {
            if (userId) {
                try {
                    const name = await userService.fetchUserName();
                    setUserName(name);
                } catch (error) {
                    console.error("Error fetching user data:", error);
                }
            }
        };

        fetchUserData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [userId]);

    const logOut = async () => {
        await authService.logout(); 
        setUserId(null); 
        navigate('/homePage'); 
        setNotification("Вы успешно вышли из системы, до скорых встреч!", "success")
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
                            className={(location.pathname !== '/homePage' && !location.pathname.includes('/favourites'))
                                 ? styles.menuButtonBold : styles.menuButton}
                        >
                            Рецепты
                        </button>
                    </Link>
                    <button  className={location.pathname.includes('/favourites') ? styles.menuButtonBold : styles.menuButton}
                     onClick={()=> !userId ? setAuthorizationWindowOpen(true) : navigate(`/favourites`)}>Избранное</button>
                </div>
            </div>
            <div className={styles.headerLoginInfo}>
                <img className={styles.loginInfoIcon} src={login} alt="Login icon" />
                {userId ? 
                    <span className={styles.loginInfo}>
                        <p className={styles.loginGreeting}
                        onClick={() => navigate(`/userPage`)}
                        >
                            {"Привет, " + (userName?.name || 'пользователь')}
                        </p>
                        <img 
                            className={styles.logoutButton} 
                            onClick={logOut} 
                            src={logout} 
                            alt="Logout icon" 
                        />
                    </span>
                    :
                    <button 
                        onClick={() => setAuthorizationWindowOpen(true)} 
                        className={styles.loginText}
                    >
                        Войти
                    </button>
                }
            </div>
        </div>
    );
};
