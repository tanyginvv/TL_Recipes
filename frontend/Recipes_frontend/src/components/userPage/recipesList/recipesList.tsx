import { useState, useEffect } from 'react';
import { UserService } from '../../../services/userService';
import { IRecipeAllRecipes } from '../../../models/types';
import { RecipeCard } from '../../recipeCard/recipeCard';
import styles from './recipesList.module.css';
import useStore from '../../../store/store';

export const RecipesList = () => {
    const [recipes, setRecipes] = useState<IRecipeAllRecipes[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    
    const { userId } = useStore();
    const userService = new UserService();

    useEffect(() => {
        const fetchRecipes = async () => {
            if (!userId) return;

            setLoading(true);
            setError(null);

            try {
                const fetchedRecipes = await userService.fetchUserRecipes(Number(userId), 1);
                setRecipes(fetchedRecipes);
            } catch (error) {
                console.error('Error fetching recipes:', error);
                setError('Ошибка загрузки рецептов');
            } finally {
                setLoading(false);
            }
        };

        fetchRecipes();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [userId]);

    if (loading) {
        return <p className={styles.loadingMessage}>Загрузка рецептов...</p>;
    }

    if (error) {
        return <p className={styles.errorMessage}>{error}</p>;
    }

    return (
        <div className={styles.recipesListContainer}>
            <h3 className={styles.listHeader}>Мои рецепты</h3>
            <div className={styles.recipeList}>
                {recipes.length > 0 ? (
                    recipes.map(recipe => (
                        <RecipeCard key={recipe.id} recipe={recipe} />
                    ))
                ) : (
                    <p className={styles.noRecipesMessage}>Кажется, у вас пока нет рецептов</p>
                )}
            </div>
        </div>
    );
};
