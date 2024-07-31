import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { RecipeOfDay } from "./recipeOfDay/recipeOfDay"
import { HomePageIntro } from "./homePageIntro/homePageIntro"
import { TagsPanel } from "./tagsPanel/tagsPanel"
import styles from "./homePage.module.css"
import { SearchComponent } from "../searchComponent/searchComponent"

export const HomePage = () => {
    const navigate = useNavigate();
    const [searchTerms, setSearchTerms] = useState<string[]>([]);

    const handleSearch = () => {
        navigate('/allRecipesPage', { state: { searchTerms } });
    };

    const handleSearchTermsChange = (terms: string[]) => {
        setSearchTerms(terms);
    };
    return (
        <>
            <div className={styles.mainPage}>
                <HomePageIntro/>
                <TagsPanel showCardText={true}/>
                <RecipeOfDay/>
                <div className={styles.searchContainer}>
                    <span className={styles.searchIntro}>                    
                        <h1 className={styles.searchTitle}>Поиск рецептов</h1>
                        <p className={styles.searchSubTitle}>Введите примерное название блюда, а мы по тегам найдем его</p>
                    </span>
                    <SearchComponent onSearchTermsChange={handleSearchTermsChange} onSearch={handleSearch} />
                </div>
            </div>
        </>
    )
}