import styles from "./addRecipeHeader.module.css";
import backspace from "../../../assets/images/backspace.svg";

interface Props {
    onPublish: () => void;
}

export const AddRecipeHeader = ({ onPublish }: Props) => {
    return (
        <div className={styles.recipeHeader}>
            <button className={styles.buttonBack}>
                <img src={backspace} alt="back" />
                <p>Назад</p>
            </button>
            <div className={styles.recipeTitle}>
                <h1 className={styles.title}>Добавить новый рецепт</h1>
                <div className={styles.titleButton}>
                    <button className={styles.buttonPost} onClick={onPublish}><p>Опубликовать</p></button>
                </div>
            </div>
        </div>
    );
};