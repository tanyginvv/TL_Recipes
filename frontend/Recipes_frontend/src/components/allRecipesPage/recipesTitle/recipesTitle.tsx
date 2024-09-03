import { useNavigate } from "react-router-dom";
import styles from "./recipesTitle.module.css";
import plus from "../../../assets/images/plus.svg";
import { TagsPanel } from "../../homePage/tagsPanel/tagsPanel";

interface RecipesTitleProps {
    onTagClick: (tag: string) => void;
}

export const RecipesTitle = ({ onTagClick }: RecipesTitleProps) => {
    const navigate = useNavigate();

    const addRecipeHandler = () => {
        navigate("/addAndEditRecipePage");
    };

    return (
        <>
            <div className={styles.titleContainer}>
                <span className={styles.titleHeader}>
                    <h1 className={styles.headerText}>Рецепты</h1>
                    <button className={styles.addRecipeButton} onClick={addRecipeHandler}>
                        <img className={styles.plus} src={plus} alt="Add" /> Добавить рецепт
                    </button>
                </span>
                <TagsPanel showCardText={false} onTagClick={onTagClick} />
            </div>
        </>
    );
};
