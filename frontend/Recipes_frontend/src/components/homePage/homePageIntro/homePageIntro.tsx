import styles from "./homePageIntro.module.css"
import introImg from "../../../assets/images/introImage.png"
import plus from "../../../assets/images/plus.svg"

export const HomePageIntro = () => {
    return (
        <>
            <div className={styles.introBody}>
                <div className={styles.introInfo}>
                    <span className={styles.infoContainer}>
                        <h1 className={styles.infoTitle}>Готовь и делись рецептами</h1>
                        <p className={styles.infoDescription}>Никаких кулинарных книг и блокнотов! 
                            Храни все любимые рецепты в одном месте.</p>
                    </span>
                    <div className={styles.introButtons}>
                        <button className={styles.addRecipeButton}><img className={styles.plus} src={plus}/>  Добавить рецепт</button>
                        <button className={styles.loginButton}>Войти</button>
                    </div>
                </div>
                <img src={introImg} className={styles.introImg}/>
            </div>
        </>
    )
}