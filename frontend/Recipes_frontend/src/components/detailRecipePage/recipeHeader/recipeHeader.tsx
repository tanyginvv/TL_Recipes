import React, { useState } from 'react';
import styles from './recipeHeader.module.css';
import backspace from '../../../assets/images/backspace.svg';
import trash from '../../../assets/images/trash.svg';
import edit from '../../../assets/images/edit.svg';
import { useNavigate } from 'react-router-dom';
import { RecipeService } from '../../../services/recipeServices';
import useStore from '../../../store/store';
import { IRecipe } from '../../../models/types';
import { ConfirmDeleteModal } from './confirmDeleteModal/confirmDeleteModal'; 

interface RecipeHeaderProps {
    recipe: IRecipe;
    onBack: () => void;
}

export const RecipeHeader: React.FC<RecipeHeaderProps> = ({ recipe, onBack }) => {
    const navigate = useNavigate();
    const { userId, setNotification } = useStore();
    const recipeService = new RecipeService();
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

    const editButtonHandler = () => {
        navigate(`/addAndEditRecipePage/${recipe.id}`);
    };

    const deleteRecipeHandler = async () => {
        const isSuccess = await recipeService.deleteRecipe(recipe.id);
        if (isSuccess) {
            setNotification('Рецепт успешно удалён', 'success');
            navigate('/allRecipesPage');
        } else {
            console.error('Ошибка при удалении рецепта');
        }
    };

    const openDeleteModal = () => {
        setIsModalOpen(true);
    };

    const closeDeleteModal = () => {
        setIsModalOpen(false);
    };

    const confirmDelete = () => {
        closeDeleteModal();
        deleteRecipeHandler();
    };

    return (
        <div className={styles.recipeHeader}>
            <button className={styles.buttonBack} onClick={onBack}>
                <img src={backspace} alt="back" />
                <p>Назад</p>
            </button>
            <div className={styles.recipeTitle}>
                <h1 className={styles.title}>{recipe.name}</h1>
                {recipe.authorId === Number(userId) ? (
                    <div className={styles.titleButtons}>
                        <button className={styles.buttonDelete} onClick={openDeleteModal}>
                            <img src={trash} alt="delete" />
                        </button>
                        <button className={styles.buttonEdit} onClick={editButtonHandler}>
                            <img className={styles.editImg} src={edit} alt="edit" />
                            <p>Редактировать</p>
                        </button>
                    </div>
                ) : null}
            </div>
            {isModalOpen && (
                <ConfirmDeleteModal onConfirm={confirmDelete} onCancel={closeDeleteModal} />
            )}
        </div>
    );
};
