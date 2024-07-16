import { AddRecipeForm } from "./addRecipeForm/addRecipeForm"
import { AddRecipeHeader } from "./addRecipeHeader/addRecipeHeader"
import styles from "./addRecipePage.module.css"
export const AddRecipePage = () => {
    return (
        <div className={styles.addRecipeContainer}>
            <AddRecipeHeader />
            <AddRecipeForm/>
        </div>
    )
}