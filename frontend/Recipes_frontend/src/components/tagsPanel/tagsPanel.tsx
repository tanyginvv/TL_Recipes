import styles from './tagsPanel.module.css';

interface TagsPanelProps {
    showCardText: boolean;
    onTagClick: (tag: string) => void; 
}

export const TagsPanel = ({ showCardText, onTagClick }: TagsPanelProps) => {
    const handleCardClick = (tag: string) => {
        onTagClick(tag);
    };

    return (
        <div className={styles.sorting}>
            {showCardText && 
            <div className={styles.sortingTitle}>
                <span className={styles.sortingHeader}>
                Умная сортировка по тегам
                </span>
                <span className={styles.sortingText}>
                    Добавляй рецепты и указывай наиболее популярные теги. 
                    Это позволит быстро находить любые категории.
                </span>
            </div>
            }
            <div className={`${styles.cards} ${!showCardText ? styles.cardsCondensed : ''}`}>
                <div className={`${styles.card} ${styles.card1}`} onClick={() => handleCardClick('Простые блюда')}>
                    <span className={styles.cardHeader}>Простые блюда</span>
                    {showCardText && (
                        <span className={styles.cardText}>
                            Время приготовления таких блюд не более 1 часа
                        </span>
                    )}
                </div>
                <div className={`${styles.card} ${styles.card2}`} onClick={() => handleCardClick('Детское')}>
                    <span className={styles.cardHeader}>Детское</span>
                    {showCardText && (
                        <span className={styles.cardText}>
                            Самые полезные блюда, которые можно детям любого возраста
                        </span>
                    )}
                </div>
                <div className={`${styles.card} ${styles.card3}`} onClick={() => handleCardClick('От шеф-поваров')}>
                    <span className={styles.cardHeader}>От шеф-поваров</span>
                    {showCardText && (
                        <span className={styles.cardText}>
                            Требуют умения, времени и терпения, зато как в ресторане
                        </span>
                    )}
                </div>
                <div className={`${styles.card} ${styles.card4}`} onClick={() => handleCardClick('На Праздник')}>
                    <span className={styles.cardHeader}>На Праздник</span>
                    {showCardText && (
                        <span className={styles.cardText}>
                            Чем удивить гостей, чтобы все были сыты за праздничным столом
                        </span>
                    )}
                </div>
            </div>
        </div>
    );
};