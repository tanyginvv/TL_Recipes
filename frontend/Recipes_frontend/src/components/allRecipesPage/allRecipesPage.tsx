import styles from "./allRecipesPage.module.css"
import { RecipesList } from "./recipesList/recipesList"
import { RecipesTitle } from "./recipesTitle/recipesTitle"
export const AllRecipesPage = () => {
    return(
        <div className={styles.recipesContainer}>
            <RecipesTitle/>
            <RecipesList/>
        </div>
    )
}