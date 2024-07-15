import Sorting from "../../homePage/sorting/sorting"
import styles from "./recipesTitle.module.css"
export const RecipesTitle = () => {
    return (
        <>
        <div className={styles.titleContainer}>
            <span className={styles.titleHeader}>
                <h1 className={styles.headerText}>Рецепты</h1>
                <button className={styles.addRecipeButton}>+  Добавить рецепт</button>
            </span>
            <Sorting showCardText={false}/>
        </div>
        </>
    )
}