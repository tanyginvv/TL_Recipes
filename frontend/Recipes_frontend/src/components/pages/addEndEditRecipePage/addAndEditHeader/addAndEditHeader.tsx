import styles from "./addAndEditHeader.module.css";
import backspace from "../../../../assets/images/backspace.svg";
import { useNavigate } from "react-router-dom";

interface Props {
    onPublish: () => void;
    isEditing: boolean; 
}

export const AddAndEditHeader = ({ onPublish, isEditing }: Props) => {
    const navigate = useNavigate();

    const backButtonHandler = () => {
        navigate("/allRecipesPage")
    }
    return (
        <div className={styles.recipeHeader}>
            <button className={styles.buttonBack} onClick={backButtonHandler}>
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