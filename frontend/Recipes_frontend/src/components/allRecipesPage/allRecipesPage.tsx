import { useEffect, useState, useMemo } from "react";
import { useLocation } from "react-router-dom";
import styles from "./allRecipesPage.module.css";
import { SearchComponent } from "../searchComponent/searchComponent";
import { RecipesList } from "./recipesList/recipesList";
import { RecipesTitle } from "./recipesTitle/recipesTitle";
import { IRecipeAllRecipes } from "../../models/types";
import { RecipeService } from "../../services/recipeServices";

interface LocationState {
    searchTerms?: string[];
}

const PAGE_SIZE = 4;

const recipeService = new RecipeService();

export const AllRecipesPage = () => {
    const location = useLocation();
    const initialSearchTerms = useMemo(
        () => (location.state as LocationState)?.searchTerms || [],
        [location.state]
    );
    const [searchTerms, setSearchTerms] = useState<string[]>(initialSearchTerms);
    const [recipes, setRecipes] = useState<IRecipeAllRecipes[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);
    const [count, setCount] = useState(1);

    useEffect(() => {
        const fetchAllRecipes = async (pageNumber: number) => {
            try {
                const data = await recipeService.fetchRecipes(pageNumber);
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
                console.error('Ошибка:', error);
            }
        };

        fetchAllRecipes(currentPage);
    }, [currentPage]);

    useEffect(() => {
        if (initialSearchTerms.length > 0) {
            setCurrentPage(1);
            setCount(1);
            fetchFilteredRecipes(initialSearchTerms, 1);
        }
    }, [initialSearchTerms]);

    const fetchFilteredRecipes = async (terms: string[], pageNumber: number) => {
        try {
            const data = await recipeService.fetchRecipes(pageNumber, terms);
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
            console.error('Ошибка:', error);
        }
    };

    const handleSearchTermsChange = (terms: string[]) => {
        setSearchTerms(terms);
    };

    const handleSearch = () => {
        setCurrentPage(1);
        setCount(1);
        fetchFilteredRecipes(searchTerms, 1);
    };

    const loadMore = () => {
        const nextPage = count + 1;
        setCount(nextPage);
        setCurrentPage(nextPage);
        if (searchTerms.length > 0) {
            fetchFilteredRecipes(searchTerms, nextPage);
        }
    };

    return (
        <div className={styles.recipesContainer}>
            <RecipesTitle />
            <div className={styles.recipeSearcher}>
                <h3 className={styles.searcherTitle}>Поиск рецептов</h3>
                <SearchComponent
                    initialSearchTerms={initialSearchTerms}
                    onSearchTermsChange={handleSearchTermsChange}
                    onSearch={handleSearch}
                />
            </div>
            <RecipesList recipes={recipes} />
            {hasMore && (
                <button onClick={loadMore} className={styles.loadMoreButton}>
                    Показать еще
                </button>
            )}
        </div>
    );
};
