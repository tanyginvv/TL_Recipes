import { useNavigate, useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import { IRecipe } from "../../models/types";
import styles from "./detailRecipePage.module.css";
import { RecipeHeader } from "./recipeHeader/recipeHeader";
import { IngredientsList } from "./ingredientsList/ingredientsList";
import { StepsList } from "./stepsList/stepsList";
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

    if (!recipe) {
        return <div>Loading...</div>;
    }

    return (
        <div className={styles.recipeContainer}>
            <RecipeHeader name={recipe.name} id={recipe.id} onBack={allRecipesPageHandler} />
            <RecipeListItem recipe={recipe} />
            <div className={styles.recipeInfo}>
                <IngredientsList ingredients={recipe.ingredients} />
                <StepsList steps={recipe.steps} />
            </div>
        </div>
    );
}