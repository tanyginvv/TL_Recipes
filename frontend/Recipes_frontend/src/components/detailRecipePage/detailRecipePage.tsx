import { useNavigate, useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import { IRecipe } from "../../models/types";
import styles from "./detailRecipePage.module.css";
import { RecipeHeader } from "./recipeHeader/recipeHeader";
import { IngredientsList } from "./ingredientsList/ingredientsList";
import { StepsList } from "./stepsList/stepsList";
import { RecipeCard } from "../recipeCard/recipeCard";
import { RecipeService } from "../../services/recipeServices";

const recipeService = new RecipeService();

export const DetailRecipePage = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();

    const allRecipesPageHandler = () => {
        navigate("/allRecipesPage");
    }

    const [recipe, setRecipe] = useState<IRecipe | null>(null);

    useEffect(() => {
        const fetchRecipe = async () => {
            try {
                const data = await recipeService.fetchRecipeById(id!);
                setRecipe(data);
            } catch (error) {
                console.error('Ошибка:', error);
            }
        };
        fetchRecipe();
    }, [id]);

    if (!recipe) {
        return <div>Loading...</div>;
    }

    return (
        <div className={styles.recipeContainer}>
            <RecipeHeader name={recipe.name} id={recipe.id} onBack={allRecipesPageHandler} />
            <RecipeCard recipe={recipe} />
            <div className={styles.recipeInfo}>
                <IngredientsList ingredients={recipe.ingredients} />
                <StepsList steps={recipe.steps} />
            </div>
        </div>
    );
};
