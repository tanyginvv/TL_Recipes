import styles from "./addAndEditRecipeHeader.module.css";
import backspace from "../../../assets/images/backspace.svg";

interface Props {
    onPublish: () => void;
    isEditing: boolean; 
}

export const AddAndEditRecipeHeader = ({ onPublish, isEditing }: Props) => {
    return (
        <div className={styles.recipeHeader}>
            <button className={styles.buttonBack}>
                <img src={backspace} alt="back" />
                <p>Назад</p>
            </button>
            <div className={styles.recipeTitle}>
                <h1 className={styles.title}>{isEditing ? 'Редактировать рецепт' : 'Добавить новый рецепт'}</h1>
                <div className={styles.titleButton}>
                    <button className={styles.buttonPost} onClick={onPublish}>
                        <p>{isEditing ? 'Сохранить изменения' : 'Опубликовать'}</p>
                    </button>
                </div>
            </div>
        </div>
    );
};
