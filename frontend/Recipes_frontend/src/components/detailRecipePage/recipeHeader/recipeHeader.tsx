import React from 'react';
import styles from './recipeHeader.module.css';
import backspace from '../../../assets/images/backspace.svg';
import trash from '../../../assets/images/trash.svg';
import edit from '../../../assets/images/edit.svg';
import { useNavigate } from 'react-router-dom';
import { RecipeService } from '../../../services/recipeServices'

interface RecipeHeaderProps {
    name: string;
    onBack: () => void;
    id: number;
}

export const RecipeHeader: React.FC<RecipeHeaderProps> = ({ name, onBack, id }) => {
    const navigate = useNavigate();
    const recipeService = new RecipeService();

    const editButtonHandler = () => {
        navigate(`/addAndEditRecipePage/${id}`);
    };

    const deleteRecipeHandler = async () => {
        const isSuccess = await recipeService.deleteRecipe(id);
        if (isSuccess) {
            navigate('/allRecipesPage');
        } else {
            console.error('Ошибка при удалении рецепта');
        }
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
                    <button className={styles.buttonDelete} onClick={deleteRecipeHandler}>
                        <img src={trash} alt="delete" />
                    </button>
                    <button className={styles.buttonEdit} onClick={editButtonHandler}>
                        <img className={styles.editImg} src={edit} alt="edit" />
                        <p>Редактировать</p>
                    </button>
                </div>
            </div>
        </div>
    );
};
