import styles from "./ingredientsList.module.css";
import { IIngredient } from "../../../../models/types";

interface IngredientsListProps {
    ingredients: IIngredient[];
}

export const IngredientsList: React.FC<IngredientsListProps> = ({ ingredients }) => {
    const renderIngredients = (ingredients: IIngredient[]) => {
        return ingredients.map((ingredient, index) => {
            const formattedDescription = ingredient.description.split('. ').join('.\n');
            return (
                <li key={index}>
                    <h5 className={styles.ingredientTitle}>{ingredient.title}</h5>
                    <p className={styles.ingredientDescription}>
                        {formattedDescription.split('\n').map((line, i) => (
                            <span key={i}>{line}<br /></span>
                        ))}
                    </p>
                </li>
            );
        });
    };

    return (
        <div className={styles.infoIngredients}>
            <h4 className={styles.ingredientsTitle}>Ингредиенты</h4>
            <ul className={styles.ingredientsList}>
                {renderIngredients(ingredients)}
            </ul>
        </div>
    );
}