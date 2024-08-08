import { useEffect, useState } from "react";
import useStore from "../../store/store";
import styles from "./favouritesPage.module.css";
import { useNavigate } from "react-router-dom";
import { UserService } from "../../services/userService";
import { IRecipeAllRecipes } from "../../models/types";
import { RecipeCard } from "../recipeCard/recipeCard";

const PAGE_SIZE = 4;

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
                const userService = new UserService();
                try {
                    const data = await userService.fetchUserFavouriteRecipes(Number(userId), pageNumber);
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
                    setHasMore(data.length === PAGE_SIZE);
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
