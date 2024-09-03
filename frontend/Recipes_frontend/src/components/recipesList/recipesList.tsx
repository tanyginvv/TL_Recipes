import { IRecipeAllRecipes } from '../../models/types';
import { RecipeCard } from '../recipeCard/recipeCard';
import styles from './recipesList.module.css';

interface RecipesListProps {
    recipes: IRecipeAllRecipes[];
}

export const RecipesList = ({ recipes }: RecipesListProps) => {
    return (
        <div className={styles.recipesListContainer}>
            {recipes.length > 0 ? (
                recipes.map(recipe => (
                    <RecipeCard key={recipe.id} recipe={recipe} />
                ))
            ) : (
                <p className={styles.noRecipesMessage}>По вашему запросу ничего не найдено</p>
            )}
        </div>
    );
};