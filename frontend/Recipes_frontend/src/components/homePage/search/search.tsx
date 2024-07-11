import styles from './search.module.css';

const Search = () => {
    return (
        <div className={styles.search}>
            <span className={styles.searchHeader}>Поиск рецептов</span>
            <span className={styles.searchText}>
                Введите примерное название блюда, а мы по тегам найдем его
            </span>
            <div className={styles.searchDirective}>
                <input type="text" placeholder="Введите название блюда" />
            </div>
        </div>
    );
};

export default Search;
