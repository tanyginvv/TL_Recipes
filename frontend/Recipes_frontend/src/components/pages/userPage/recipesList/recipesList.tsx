import React, { useState, useEffect } from 'react';
import { IRecipePart, RecipeQueryType } from '../../../../models/types';
import { RecipeCard } from '../../../recipeCard/recipeCard';
import styles from './recipesList.module.css';
import useStore from '../../../../store/store';
import { RecipeService } from '../../../../services/recipeServices';

interface RecipesListProps {
    onLikeChange?: (newLikeCount: number) => void;
    onFavouriteChange?: (newFavouriteCount: number) => void;
}

export const RecipesList: React.FC<RecipesListProps> = ({ onLikeChange, onFavouriteChange }) => {
    const [recipes, setRecipes] = useState<IRecipePart[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [currentPage, setCurrentPage] = useState<number>(1);
    const [hasMore, setHasMore] = useState<boolean>(true);

    const { userId } = useStore();
    const recipeService = new RecipeService();

    useEffect(() => {
        const fetchRecipes = async (page: number) => {
            if (!userId) return;

            setLoading(true);
            setError(null);

            try {
                const fetchedRecipes = await recipeService.fetchRecipes(page, [], RecipeQueryType.My);

                setRecipes(prev => {
                    const newRecipes = [...prev, ...fetchedRecipes.getRecipePartDtos];
                    const uniqueRecipes = newRecipes.filter((recipe, index, self) =>
                        index === self.findIndex((r) => r.id === recipe.id)
                    );
                    return uniqueRecipes;
                });

                setHasMore(fetchedRecipes.isNextPageAvailable);
            } catch (error) {
                console.error('Error fetching recipes:', error);
                setError('Ошибка загрузки рецептов');
            } finally {
                setLoading(false);
            }
        };

        fetchRecipes(currentPage);
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [userId, currentPage]);

    const loadMore = () => {
        if (hasMore) {
            setCurrentPage(prevPage => prevPage + 1);
        }
    };

    const handleLikeChange = (newLikeCount: number) => {
        if (onLikeChange) {
            onLikeChange(newLikeCount);
        }
    };

    const handleFavouriteChange = (newFavouriteCount: number) => {
        if (onFavouriteChange) {
            onFavouriteChange(newFavouriteCount);
        }
    };

    if (loading && currentPage === 1) {
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
                        <RecipeCard
                            key={recipe.id}
                            recipe={recipe}
                            onLikeChange={handleLikeChange}
                            onFavouriteChange={handleFavouriteChange}
                        />
                    ))
                ) : (
                    <p className={styles.noRecipesMessage}>Кажется, у вас пока нет рецептов</p>
                )}
            </div>
            {hasMore && (
                <button onClick={loadMore} className={styles.loadMoreButton}>
                    Показать еще
                </button>
            )}
        </div>
    );
};
