import { useState, useEffect } from 'react';
import { IRecipePart, RecipeQueryType } from '../../models/types';
import { RecipeService } from '../../services/recipeServices';
import { RecipeCard } from '../recipeCard/recipeCard';
import styles from './recipesList.module.css';

interface RecipesListProps {
    recipeQueryType: RecipeQueryType;
    searchTerms?: string[];
    onSearch?: () => void;
}

const recipeService = new RecipeService();

export const RecipesList = ({ recipeQueryType, searchTerms, onSearch }: RecipesListProps) => {
    const [recipes, setRecipes] = useState<IRecipePart[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [hasMore, setHasMore] = useState<boolean>(true);
    const [isLoading, setIsLoading] = useState(false);

    const fetchAllRecipes = async (pageNumber: number, terms?: string[]) => {
        setIsLoading(true);
        try {
            const data = await recipeService.fetchRecipes(pageNumber, terms, recipeQueryType);
            if (pageNumber === 1) {
                setRecipes(data.getRecipePartDtos);
            } else {
                setRecipes(prev => {
                    const newRecipes = [...prev, ...data.getRecipePartDtos];
                    const uniqueRecipes = newRecipes.filter((recipe, index, self) =>
                        index === self.findIndex((r) => r.id === recipe.id)
                    );
                    return uniqueRecipes;
                });
            }
            setHasMore(data.isNextPageAvailable);
        } catch (error) {
            console.error('Ошибка:', error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        setCurrentPage(1);
        setRecipes([]);
        fetchAllRecipes(1, searchTerms);
        onSearch && onSearch();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [searchTerms, recipeQueryType]);

    useEffect(() => {
        if (currentPage > 1) {
            fetchAllRecipes(currentPage, searchTerms);
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [currentPage]);

    const loadMore = () => {
        if (!isLoading && hasMore) {
            setCurrentPage(prevPage => prevPage + 1);
        }
    };

    return (
        <div className={styles.recipesListContainer}>
            {recipes.length > 0 ? (
                recipes.map(recipe => (
                    <RecipeCard key={recipe.id} recipe={recipe} />
                ))
            ) : (
                <p className={styles.noRecipesMessage}>Кажется, здесь пока нет рецептов</p>
            )}
             {hasMore && !isLoading && (
            <button onClick={loadMore} className={styles.loadMoreButton}>
                Показать еще
            </button>
        )}
        {isLoading && <p>Загрузка...</p>}
        </div>
    );
};