import styles from './sorting.module.css';

const Sorting = () => {
    return (
        <div className={styles.sorting}>
            <span className={styles.sortingHeader}>
                Умная сортировка по тегам
            </span>
            <span className={styles.sortingText}>
                Добавляй рецепты и указывай наиболее популярные теги. 
                Это позволит быстро находить любые категории.
            </span>
            <div className={styles.cards}>
                <div className={`${styles.card} ${styles.card1}`}>
                    <span className={styles.cardHeader}>Простые блюда</span>
                    <span className={styles.cardText}>
                        Время приготовления таких блюд не более 1 часа
                    </span>
                </div>
                <div className={`${styles.card} ${styles.card2}`}>
                    <span className={styles.cardHeader}>Детское</span>
                    <span className={styles.cardText}>
                        Самые полезные блюда которые можно детям любого возраста
                    </span>
                </div>
                <div className={`${styles.card} ${styles.card3}`}>
                    <span className={styles.cardHeader}>От шеф-поваров</span>
                    <span className={styles.cardText}>
                        Требуют умения, времени и терпения, зато как в ресторане
                    </span>
                </div>
                <div className={`${styles.card} ${styles.card4}`}>
                    <span className={styles.cardHeader}>На Праздник</span>
                    <span className={styles.cardText}>
                        Чем удивить гостей, чтобы все были сыты за праздничным столом
                    </span>
                </div>
            </div>
        </div>
    );
};

export default Sorting;