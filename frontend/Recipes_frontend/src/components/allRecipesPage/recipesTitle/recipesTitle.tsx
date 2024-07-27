import { useNavigate } from "react-router-dom"
import styles from "./recipesTitle.module.css"
import plus from "../../../assets/images/plus.svg"
import { TagsPanel } from "../../homePage/tagsPanel/tagsPanel";
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
                <button className={styles.addRecipeButton} onClick={addRecipeHandler}><img className={styles.plus} src={plus}/>  Добавить рецепт</button>
            </span>
            <TagsPanel showCardText={false}/>
        </div>
        </>
    )
}