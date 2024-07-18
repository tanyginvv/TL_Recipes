import styles from "./recipeHeader.module.css";
import backspace from "../../../assets/images/backspace.svg";
import trash from "../../../assets/images/trash.svg";
import edit from "../../../assets/images/edit.svg";
import { useNavigate } from "react-router-dom";

interface RecipeHeaderProps {
    name: string;
    onBack: () => void;
    id: number;
}

export const RecipeHeader: React.FC<RecipeHeaderProps> = ({ name, onBack, id }) => {
    const navigate = useNavigate();

    const editButtonHandler = () => {
        navigate(`/addAndEditRecipePage/${id}`)
    }
    const deleteRecipeHandler = () => {
        fetch(`http://localhost:5218/api/recipes/${id}`, {
            method: "DELETE"
        })
        .then(response => {
            if (response.ok) {
                navigate("/allRecipesPage");
            } else {
                console.error("Ошибка при удалении рецепта");
            }
        })
        .catch(error => console.error('Ошибка:', error));
    };

    return (
        <div className={styles.recipeHeader}>
            <button className={styles.buttonBack} onClick={onBack}>
                <img src={backspace} alt="back" />
                <p>Назад</p>
            </button>
            <div className={styles.recipeTitle}>
                <h1 className={styles.title}>{name}</h1>
                <div className={styles.titleButtons}>
                    <button className={styles.buttonDelete} onClick={deleteRecipeHandler}><img src={trash} alt="delete" /></button>
                    <button className={styles.buttonEdit} onClick={editButtonHandler}><img src={edit} alt="edit" /><p>Редактировать</p></button>
                </div>
            </div>
        </div>
    );
}
