import { useNavigate, useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import { IRecipe, IIngredient, IStep} from "../../models/types";
import styles from "./detailRecipePage.module.css";
import backspace from "../../assets/images/backspace.svg";
import trash from "../../assets/images/trash.svg";
import edit from "../../assets/images/edit.svg";
import { RecipeListItem } from "../allRecipesPage/recipeListItem/recipeListItem";

export const DetailRecipePage = () => {
    const navigate = useNavigate();

    const allRecipesPageHandler = () => {
        navigate("/allRecipesPage")
    }
    const { id } = useParams<{ id: string }>();

    const [recipe, setRecipe] = useState<IRecipe | null>(null);

    useEffect(() => {
        fetch(`http://localhost:5218/api/recipes/${id}`)
            .then(response => response.json())
            .then(data => setRecipe(data))
            .catch(error => console.error('Ошибка:', error));
    }, [id]);

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

    const renderSteps = (steps: IStep[]) => {
        const sortedSteps = [...steps].sort((a, b) => a.stepNumber - b.stepNumber);
        return sortedSteps.map((step, index) => (
            <li className={styles.stepListItem} key={index}>
                <h5 className={styles.stepTitle}>{"Шаг " + step.stepNumber}</h5>
                <p className={styles.stepDescription}>{step.stepDescription}</p>
            </li>
        ));
    };

    if (!recipe) {
        return <div>Loading...</div>;
    }

    return (
        <div className={styles.recipeContainer}>
            <div className={styles.recipeHeader}>
                <button className={styles.buttonBack} onClick={allRecipesPageHandler}><img src={backspace} alt="back" /><p>Назад</p></button>
                <div className={styles.recipeTitle}>
                    <h1 className={styles.title}>{recipe.name}</h1>
                    <div className={styles.titleButtons}>
                        <button className={styles.buttonDelete}><img src={trash} alt="delete" /></button>
                        <button className={styles.buttonEdit}><img src={edit} alt="edit" /><p>Редактировать</p></button>
                    </div>
                </div>
            </div>
            <RecipeListItem recipe={recipe} />
            <div className={styles.recipeInfo}>
                <div className={styles.infoIngredients}>
                    <h4 className={styles.ingredientsTitle}>Ингредиенты</h4>
                    <ul className={styles.ingredientsList}>
                        {renderIngredients(recipe.ingredients)}
                    </ul>
                </div>
                <div className={styles.infoSteps}>
                    <ul className={styles.stepsList}>
                        {renderSteps(recipe.steps)}
                    </ul>
                    <p className={styles.stepEnd}>Приятного аппетита!</p>
                </div>
            </div>
        </div>
    );
}