import styles from "./homePageIntro.module.css";
import introImg from "../../../assets/images/introImage.png";
import plus from "../../../assets/images/plus.svg";
import useStore from "../../../store/store";

export const HomePageIntro = () => {
    const {
        setAuthOrRegistrWindowOpen,
        userId // Get userId from the store to check if the user is authenticated
    } = useStore();

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
                    <button className={styles.addRecipeButton}>
                        <img className={styles.plus} src={plus} alt="Plus icon" /> 
                        Добавить рецепт
                    </button>
                    {userId === null ? (
                        <button 
                            onClick={() => setAuthOrRegistrWindowOpen(true)} 
                            className={styles.loginButton}
                        >
                            Войти
                        </button>
                    ) : null}
                </div>
            </div>
            <img src={introImg} className={styles.introImg} alt="Intro" />
        </div>
    );
};
