import { useEffect, useState, useMemo, useRef } from "react";
import { useLocation } from "react-router-dom";
import styles from "./allRecipesPage.module.css";
import { SearchComponent } from "../searchComponent/searchComponent";
import { RecipesList } from "../recipesList/recipesList";
import { RecipesTitle } from "./recipesTitle/recipesTitle";
import { IRecipePart, RecipeQueryType } from "../../models/types";
import { RecipeService } from "../../services/recipeServices";

interface LocationState {
    searchTerms?: string[];
}

const recipeService = new RecipeService();

export const AllRecipesPage = () => {
    const location = useLocation();
    const searchComponentRef = useRef<HTMLDivElement>(null);

    const initialSearchTerms = useMemo(
        () => (location.state as LocationState)?.searchTerms || [],
        [location.state]
    );
    const [searchTerms, setSearchTerms] = useState<string[]>(initialSearchTerms);
    const [recipes, setRecipes] = useState<IRecipePart[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [hasMore, setHasMore] = useState<boolean>(true);
    const [count, setCount] = useState(1);

    useEffect(() => {
        if (initialSearchTerms.length > 0) {
            setCurrentPage(1);
            setCount(1);
        }
    }, [initialSearchTerms]);
    
    useEffect(() => {
        const fetchAllRecipes = async (pageNumber: number, terms: string[]) => {
            try {
                const data = await recipeService.fetchRecipes(pageNumber, terms, RecipeQueryType.All);
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
            }
        };

        fetchAllRecipes(currentPage, searchTerms);
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [currentPage, searchTerms]);

    const handleSearchTermsChange = (terms: string[]) => {
        setSearchTerms(terms);
    };

    const handleSearch = () => {
        setCurrentPage(1);
        setCount(1);
    };

    const handleTagClick = (tag: string) => {
        if (!searchTerms.includes(tag)) {
            const newTerms = [...searchTerms, tag];
            setSearchTerms(newTerms);
        }
        if (searchComponentRef.current) {
            searchComponentRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    };

    const loadMore = () => {
        const nextPage = count + 1;
        setCount(nextPage);
        setCurrentPage(nextPage);
    };

    return (
        <div className={styles.recipesContainer}>
            <RecipesTitle onTagClick={handleTagClick} />
            <div ref={searchComponentRef} className={styles.recipeSearcher}>
                <h3 className={styles.searcherTitle}>Поиск рецептов</h3>
                <SearchComponent
                    initialSearchTerms={searchTerms}
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
