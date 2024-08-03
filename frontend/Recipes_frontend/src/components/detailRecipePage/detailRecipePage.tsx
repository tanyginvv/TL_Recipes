import { useNavigate, useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import { IRecipe } from "../../models/types";
import styles from "./detailRecipePage.module.css";
import { RecipeHeader } from "./recipeHeader/recipeHeader";
import { IngredientsList } from "./ingredientsList/ingredientsList";
import { StepsList } from "./stepsList/stepsList";
import { RecipeCard } from "../recipeCard/recipeCard";
import { RecipeService } from "../../services/recipeServices";

export const DetailRecipePage = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    const recipeService = new RecipeService();

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
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [id]);

    if (!recipe) {
        return <div>Loading...</div>;
    }

    return (
        <div className={styles.recipeContainer}>
            <RecipeHeader recipe={recipe} onBack={allRecipesPageHandler}/>
            <RecipeCard recipe={recipe} />
            <div className={styles.recipeInfo}>
                <IngredientsList ingredients={recipe.ingredients} />
                <StepsList steps={recipe.steps} />
            </div>
        </div>
    );
};
