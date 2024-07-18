import { useNavigate } from "react-router-dom"
import Sorting from "../../homePage/sorting/sorting"
import styles from "./recipesTitle.module.css"
export const RecipesTitle = () => {
    const navigate = useNavigate();

    const addRecipeHandler = () => {
        navigate("/addAndEditRecipePage")
    }
    return (
        <>
        <div className={styles.titleContainer}>
            <span className={styles.titleHeader}>
                <h1 className={styles.headerText}>Рецепты</h1>
                <button className={styles.addRecipeButton} onClick={addRecipeHandler}>+  Добавить рецепт</button>
            </span>
            <Sorting showCardText={false}/>
        </div>
        </>
    )
}