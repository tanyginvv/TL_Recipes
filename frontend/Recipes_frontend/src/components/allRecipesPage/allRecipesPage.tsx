import { useEffect, useState, useMemo } from "react";
import { useLocation } from "react-router-dom";
import styles from "./allRecipesPage.module.css";
import { SearchComponent } from "../searchComponent/searchComponent";
import { RecipesList } from "./recipesList/recipesList";
import { RecipesTitle } from "./recipesTitle/recipesTitle";
import { IRecipe } from "../../models/types";

interface LocationState {
    searchTerms?: string[];
}

const PAGE_SIZE = 4;

export const AllRecipesPage = () => {
    const location = useLocation();
    const initialSearchTerms = useMemo(
        () => (location.state as LocationState)?.searchTerms || [],
        [location.state]
    );
    const [searchTerms, setSearchTerms] = useState<string[]>(initialSearchTerms);
    const [recipes, setRecipes] = useState<IRecipe[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);
    const [count, setCount] = useState(1);

    useEffect(() => {
        const fetchAllRecipes = async (pageNumber: number) => {
            try {
                const response = await fetch(`http://localhost:5218/api/recipes?pageNumber=${pageNumber}`);
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const data = await response.json();
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
            const queryString = terms.length > 0
                ? `?${terms.map(term => `searchTerms=${encodeURIComponent(term)}`).join('&')}&pageNumber=${pageNumber}`
                : `?pageNumber=${pageNumber}`;

            const url = `http://localhost:5218/api/recipes${queryString}`;

            const response = await fetch(url);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
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
