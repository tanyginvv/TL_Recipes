import { useEffect, useState } from 'react';
import { RecipeListItem } from '../recipeListItem/recipeListItem';
import { IRecipe } from '../../../models/types';
import styles from './recipesList.module.css'

export const RecipesList = () => {
    const [recipes, setRecipes] = useState<IRecipe[]>([]);

    useEffect(() => {
        fetch('http://localhost:5218/api/recipes') 
            .then(response => response.json())
            .then(data => setRecipes(data))
            .catch(error => console.error('Ошибка:', error));
    }, []);

    return (
        <div className={styles.recipesListContainer}>
            {recipes.map(recipe => (
                <RecipeListItem key={recipe.id} recipe={recipe} />
            ))}
        </div>
    );
};
