import styles from './example.module.css';

export const RecipeOfDay= () => {
    return (
        <div className={styles.example}>
            <div className={styles.exampleContent}>
                <span className={styles.exampleHeader}>
                    Тыквенный супчик на кокосовом молоке
                </span>
                <span className={styles.exampleText}>
                    Если у вас осталась тыква, и вы не знаете что с ней сделать, 
                    то это решение для вас! Ароматный, согревающий суп-пюре на 
                    кокосовом молоке. Можно даже в Пост!
                </span>
            </div>
            <span className={styles.exampleTag}>@glazest</span>
        </div>
    );
};
