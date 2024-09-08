import { useState, useMemo, useRef } from "react";
import { useLocation } from "react-router-dom";
import styles from "./allRecipesPage.module.css";
import { SearchComponent } from "../searchComponent/searchComponent";
import { RecipesList } from "../recipesList/recipesList";
import { RecipesTitle } from "./recipesTitle/recipesTitle";
import { RecipeQueryType } from "../../models/types";

interface LocationState {
    searchTerms?: string[];
}

export const AllRecipesPage = () => {
    const location = useLocation();
    const searchComponentRef = useRef<HTMLDivElement>(null);

    const initialSearchTerms = useMemo(
        () => (location.state as LocationState)?.searchTerms || [],
        [location.state]
    );
    const [searchTerms, setSearchTerms] = useState<string[]>(initialSearchTerms);
    
    const handleSearchTermsChange = (terms: string[]) => {
        setSearchTerms(terms);
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

    const handleSearch = () =>{}

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
            <RecipesList searchTerms={searchTerms} recipeQueryType={RecipeQueryType.All} onSearch={handleSearch}/>
        </div>
    );
};