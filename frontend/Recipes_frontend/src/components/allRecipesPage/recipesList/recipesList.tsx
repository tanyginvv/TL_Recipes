import { IRecipe } from '../../../models/types';
import { RecipeListItem } from '../recipeListItem/recipeListItem';
import styles from './recipesList.module.css';

interface RecipesListProps {
    recipes: IRecipe[];
}

export const RecipesList = ({ recipes }: RecipesListProps) => {
    return (
        <div className={styles.recipesListContainer}>
            {recipes.length > 0 ? (
                recipes.map(recipe => (
                    <RecipeListItem key={recipe.id} recipe={recipe} />
                ))
            ) : (
                <p className={styles.noRecipesMessage}>По вашему запросу ничего не найдено</p>
            )}
        </div>
    );
};
