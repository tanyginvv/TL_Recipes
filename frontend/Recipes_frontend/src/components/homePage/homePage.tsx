import { useNavigate } from "react-router-dom";
import { useState } from "react";
import Example from "./example/example";
import { HomePageIntro } from "./homePageIntro/homePageIntro";
import Sorting from "./sorting/sorting";
import styles from "./homePage.module.css";
import { SearchComponent } from "../searchComponent/searchComponent";

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
        <div className={styles.mainPage}>
            <HomePageIntro />
            <Sorting showCardText={true} />
            <Example />
            <div className={styles.searchContainer}>
                <h1 className={styles.searchTitle}>Поиск рецептов</h1>
                <p className={styles.searchSubTitle}>Введите примерное название блюда, а мы по тегам найдем его</p>
                <SearchComponent onSearchTermsChange={handleSearchTermsChange} onSearch={handleSearch} />
            </div>
        </div>
    );
};
