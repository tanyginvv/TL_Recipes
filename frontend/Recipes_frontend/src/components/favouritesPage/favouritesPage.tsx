import { useEffect, useState } from "react";
import useStore from "../../store/store";
import styles from "./favouritesPage.module.css";
import { useNavigate } from "react-router-dom";
import { IRecipeAllRecipes } from "../../models/types";
import { RecipeCard } from "../recipeCard/recipeCard";
import { RecipeService } from "../../services/recipeServices";

export const FavouritesPage = () => {
    const { userId } = useStore();
    const navigate = useNavigate();
    const [recipes, setRecipes] = useState<IRecipeAllRecipes[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);
    const [count, setCount] = useState(1);

    useEffect(() => {
        const fetchRecipes = async (pageNumber: number) => {
            if (userId) {
                const recipeService = new RecipeService();
                try {
                    const data = await recipeService.fetchRecipes(pageNumber, [], true, true);
                    if (pageNumber === 1) {
                        setRecipes(data);
                    } else {
                        setRecipes(prev => {
                            const newRecipes = [...prev, ...data];
                            const uniqueRecipes = newRecipes.filter((recipe, index, self) =>
                                index === self.findIndex((r) => r.id === recipe.id)
                            );
                            return uniqueRecipes;
                        });
                    }
                    const nextData = await recipeService.fetchRecipes(pageNumber + 1, [], true, true);
                    setHasMore(nextData.length > 0);
                } catch (error) {
                    console.error('Error fetching favourite recipes:', error);
                }
            } else {
                navigate("/");
            }
        };

        fetchRecipes(currentPage);
    }, [currentPage, userId, navigate]);

    const loadMore = () => {
        const nextPage = count + 1;
        setCount(nextPage);
        setCurrentPage(nextPage);
    };

    return (
        <div className={styles.favouritesContainer}>
           <h1 className={styles.title}>Избранное</h1>
            {recipes.length > 0 ? (
                <div className={styles.recipesContainer}>
                    {recipes.map(recipe => (
                        <RecipeCard key={recipe.id} recipe={recipe} />
                    ))}
                    {hasMore && (
                        <button onClick={loadMore} className={styles.loadMoreButton}>
                            Показать еще
                        </button>
                    )}
                </div>
            ) : (
                <div className={styles.emptyContainer}>
                    <p className={styles.emptyText}>Ваш список пуст</p>
                </div>
            )}
        </div>
    );
};
