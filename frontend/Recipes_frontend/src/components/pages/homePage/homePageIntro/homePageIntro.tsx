import styles from "./homePageIntro.module.css";
import introImg from "../../../../assets/images/introImage.png";
import plus from "../../../../assets/images/plus.svg";
import useStore from "../../../../store/store";
import { useNavigate } from "react-router-dom";

export const HomePageIntro = () => {
    const { setAuthOrRegistrWindowOpen, userId } = useStore();
    const navigate = useNavigate();

    const handleAddRecipeClick = () => {
        if (userId === null) {
            setAuthOrRegistrWindowOpen(true);
        } else {
            navigate("/addAndEditRecipePage");
        }
    };

    const handleLoginClick = () => {
        setAuthOrRegistrWindowOpen(true);
    };

    return (
        <div className={styles.introBody}>
            <div className={styles.introInfo}>
                <span className={styles.infoContainer}>
                    <h1 className={styles.infoTitle}>Готовь и делись рецептами</h1>
                    <p className={styles.infoDescription}>
                        Никаких кулинарных книг и блокнотов! 
                        Храни все любимые рецепты в одном месте.
                    </p>
                </span>
                <div className={styles.introButtons}>
                    <button 
                        className={styles.addRecipeButton} 
                        onClick={handleAddRecipeClick}
                    >
                        <img className={styles.plus} src={plus} alt="Plus icon" /> 
                        Добавить рецепт
                    </button>
                    {userId === null && (
                        <button 
                            onClick={handleLoginClick} 
                            className={styles.loginButton}
                        >
                            Войти
                        </button>
                    )}
                </div>
            </div>
            <img src={introImg} className={styles.introImg} alt="Intro" />
        </div>
    );
};
