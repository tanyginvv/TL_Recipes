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

export const AllRecipesPage = () => {
    const location = useLocation();
    const initialSearchTerms = useMemo(
        () => (location.state as LocationState)?.searchTerms || [],
        [location.state]
    );
    const [searchTerms, setSearchTerms] = useState<string[]>(initialSearchTerms);
    const [recipes, setRecipes] = useState<IRecipe[]>([]);
    const [, setAllRecipes] = useState<IRecipe[]>([]);

    useEffect(() => {
        const fetchAllRecipes = async () => {
            try {
                const response = await fetch('http://localhost:5218/api/recipes');
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const data = await response.json();
                setAllRecipes(data);
                if (initialSearchTerms.length === 0) {
                    setRecipes(data);
                }
            } catch (error) {
                console.error('Ошибка:', error);
            }
        };

        fetchAllRecipes();
    });

    useEffect(() => {
        if (initialSearchTerms.length > 0) {
            fetchFilteredRecipes(initialSearchTerms);
        }
    }, [initialSearchTerms]);

    const fetchFilteredRecipes = async (terms: string[]) => {
        try {
            const queryString = terms.length > 0
                ? `?${terms.map(term => `searchTerms=${encodeURIComponent(term)}`).join('&')}`
                : '';

            const url = `http://localhost:5218/api/recipes/search${queryString}`;

            const response = await fetch(url);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            setRecipes(data);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    };

    const handleSearchTermsChange = (terms: string[]) => {
        setSearchTerms(terms);
    };

    const handleSearch = () => {
        fetchFilteredRecipes(searchTerms);
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
        </div>
    );
};
