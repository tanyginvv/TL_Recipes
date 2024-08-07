import styles from "./userRecipesHeader.module.css"
import book from "./../../../assets/images/bookTag.png"
interface HeaderProps {
    recipesCount : number,
    likesCount : number,
    favouritesCount : number
}

export const UserRecipesHeader = ( props: HeaderProps) => {
    return(
        <div className={styles.headerContainer}>
            <span className={styles.headerItem}>
                <span className={styles.itemInfo}>
                    <span className={styles.itemImg}><img className={styles.image}src={book}/></span>
                    <p className={styles.itemText}>Всего рецептов</p>
                </span>
                <p className={styles.itemCount}>{props.recipesCount}</p>
            </span>
            <span className={styles.headerItem}>
                <span className={styles.itemInfo}>
                    <span className={styles.itemImg}><img className={styles.image}src={book}/></span>
                    <p className={styles.itemText}>Всего лайков</p>
                </span>
                <p className={styles.itemCount}>{props.likesCount}</p>
            </span>
            <span className={styles.headerItem}>
                <span className={styles.itemInfo}>
                    <span className={styles.itemImg}><img className={styles.image}src={book}/></span>
                    <p className={styles.itemText}>В избранных</p>
                </span>
                <p className={styles.itemCount}>{props.favouritesCount}</p>
            </span>
        </div>
    )
}