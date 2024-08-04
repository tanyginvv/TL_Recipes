import styles from "./userRecipesHeader.module.css"
import book from "./../../../assets/images/bookTag.png"
interface HeaderProps {
    recipesCount : number
}

export const UserRecipesHeader = (recipesCount: HeaderProps) => {
    return(
        <div className={styles.headerContainer}>
            <span className={styles.headerItem}>
                <span className={styles.itemInfo}>
                    <span className={styles.itemImg}><img className={styles.image}src={book}/></span>
                    <p className={styles.itemText}>Всего рецептов</p>
                </span>
                <p className={styles.itemCount}>{recipesCount.recipesCount}</p>
            </span>
            <span className={styles.headerItem}>
                <span className={styles.itemInfo}>
                    <span className={styles.itemImg}><img className={styles.image}src={book}/></span>
                    <p className={styles.itemText}>Всего лайков</p>
                </span>
                <p className={styles.itemCount}>0</p>
            </span>
            <span className={styles.headerItem}>
                <span className={styles.itemInfo}>
                    <span className={styles.itemImg}><img className={styles.image}src={book}/></span>
                    <p className={styles.itemText}>В избранных</p>
                </span>
                <p className={styles.itemCount}>0</p>
            </span>
        </div>
    )
}