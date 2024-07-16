import styles from "./recipeHeader.module.css";
import backspace from "../../../assets/images/backspace.svg";
import trash from "../../../assets/images/trash.svg";
import edit from "../../../assets/images/edit.svg";

interface RecipeHeaderProps {
    name: string;
    onBack: () => void;
}

export const RecipeHeader: React.FC<RecipeHeaderProps> = ({ name, onBack }) => {
    return (
        <div className={styles.recipeHeader}>
            <button className={styles.buttonBack} onClick={onBack}>
                <img src={backspace} alt="back" />
                <p>Назад</p>
            </button>
            <div className={styles.recipeTitle}>
                <h1 className={styles.title}>{name}</h1>
                <div className={styles.titleButtons}>
                    <button className={styles.buttonDelete}><img src={trash} alt="delete" /></button>
                    <button className={styles.buttonEdit}><img src={edit} alt="edit" /><p>Редактировать</p></button>
                </div>
            </div>
        </div>
    );
}
