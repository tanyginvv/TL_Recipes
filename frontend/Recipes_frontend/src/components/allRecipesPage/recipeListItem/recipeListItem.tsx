import React, { useState, useEffect } from 'react';
import styles from './recipeListItem.module.css';
import cookingTimeIcon from "../../../assets/images/cookingTime.svg";
import countPortionIcon from "../../../assets/images/countPortion.svg";
import { IRecipe } from '../../../models/types';
import { useNavigate } from 'react-router-dom';

interface RecipeListItemProps {
    recipe: IRecipe;
}

export const RecipeListItem: React.FC<RecipeListItemProps> = ({ recipe }) => {
    const navigate = useNavigate();
    const [imageSrc, setImageSrc] = useState<string>("");

    useEffect(() => {
        const fetchImage = async () => {
            if (recipe.imageUrl) {
                try {
                    const response = await fetch(`http://localhost:5218/api/images/${recipe.imageUrl}`);
                    if (response.ok) {
                        const imageBlob = await response.blob();
                        const imageObjectURL = URL.createObjectURL(imageBlob);
                        setImageSrc(imageObjectURL);
                    } else {
                        console.error('Ошибка при загрузке изображения');
                    }
                } catch (error) {
                    console.error('Ошибка:', error);
                }
            }
        };

        fetchImage();
    }, [recipe.imageUrl]);

    const recipeHandler = () => {
        navigate(`/detailRecipesPage/${recipe.id}`);
    };

    return (
        <div className={styles.recipeItem} onClick={recipeHandler}>
            <img className={styles.recipeImg} src={imageSrc} alt="Recipe" />
            <span className={styles.recipeInfo}>
                <span className={styles.recipeTags}>
                    {recipe.tags.map(tag => (
                        <p key={tag.name} className={styles.recipeTag}>{tag.name}</p>
                    ))}
                </span>
                <span className={styles.recipeNameInfo}>
                    <h3 className={styles.recipeName}>{recipe.name}</h3>
                    <p className={styles.recipeDescription}>{recipe.description}</p>
                </span>
                <span className={styles.recipeExtras}>
                    <span className={styles.extrasItem}>
                        <img className={styles.extrasItemImg} src={cookingTimeIcon} alt="Время приготовления" />
                        <span className={styles.extrasItemText}>
                            <p className={styles.description}>Время приготовления:</p>
                            <p className={styles.value}>{recipe.cookTime} минут</p>
                        </span>
                    </span>
                    <span className={styles.extrasItem}>
                        <img className={styles.extrasItemImg} src={countPortionIcon} alt="Количество порций" />
                        <span className={styles.extrasItemText}>
                            <p className={styles.description}>Рецепт на:</p>
                            <p className={styles.value}>{recipe.countPortion} персон</p>
                        </span>
                    </span>
                </span>
            </span>
        </div>
    );
}
