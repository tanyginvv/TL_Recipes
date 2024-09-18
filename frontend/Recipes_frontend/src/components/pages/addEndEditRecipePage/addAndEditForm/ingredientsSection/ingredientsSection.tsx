import React from 'react';
import styles from './ingredientsSection.module.css';
import { IIngredient } from "../../../../../models/types";
import close from "../../../../../assets/images/close.svg";

interface IngredientsSectionProps {
    ingredients: IIngredient[];
    setIngredients: (ingredients: IIngredient[]) => void;
    areIngredientsValid: boolean;
}

export const IngredientsSection: React.FC<IngredientsSectionProps> = ({ ingredients, setIngredients, areIngredientsValid }) => {
    const handleIngredientChange = (index: number, key: keyof IIngredient, value: string) => {
        const newIngredients = [...ingredients];
        newIngredients[index] = {
            ...newIngredients[index],
            [key]: value
        };
        setIngredients(newIngredients);
    };

    const handleAddIngredient = () => {
        setIngredients([...ingredients, { title: '', description: '' }]);
    };

    const handleRemoveIngredient = (index: number) => {
        const newIngredients = ingredients.filter((_, i) => i !== index);
        setIngredients(newIngredients);
    };

    return (
        <div className={styles.ingredientsInfo}>
            <h5 className={styles.ingredientTitle}>Ингредиенты</h5>
            {ingredients.map((ingredient, index) => (
                <span key={index} className={styles.ingredientItem}>
                    <span className={styles.closeBtnIngredient} onClick={() => handleRemoveIngredient(index)}>
                        <img className={styles.closeIcon} src={close} alt="Remove ingredient" />
                    </span>
                    <input
                        type="text"
                        placeholder="Заголовок"
                        value={ingredient.title}
                        onChange={(e) => handleIngredientChange(index, 'title', e.target.value)}
                        className={`${styles.ingredientsTitle} ${!areIngredientsValid && !ingredient.title.trim() ? styles.invalid : ''}`}
                    />
                    <textarea
                        placeholder="Описание"
                        value={ingredient.description}
                        onChange={(e) => handleIngredientChange(index, 'description', e.target.value)}
                        className={`${styles.ingredientsDescription} ${!areIngredientsValid && !ingredient.description.trim() ? styles.invalid : ''}`}
                    />
                </span>
            ))}
            <button type="button" onClick={handleAddIngredient} className={styles.addIngredientButton}>+   Добавить ингредиент</button>
        </div>
    );
};