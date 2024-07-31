import React, { useState, useEffect } from 'react';
import styles from './recipeCard.module.css';
import cookingTimeIcon from "../../assets/images/cookingTime.svg";
import countPortionIcon from "../../assets/images/personCount.svg";
import { IRecipeAllRecipes } from '../../models/types';
import { useNavigate } from 'react-router-dom';
import { ImageService } from '../../services/imageService';

interface RecipeCardProps {
    recipe: IRecipeAllRecipes;
}

const imageService = new ImageService();

export const RecipeCard: React.FC<RecipeCardProps> = ({ recipe }) => {
    const navigate = useNavigate();
    const [imageSrc, setImageSrc] = useState<string>("");

    useEffect(() => {
        const fetchImage = async () => {
            if (recipe.imageUrl) {
                const imageObjectURL = await imageService.fetchImage(recipe.imageUrl);
                if (imageObjectURL) {
                    setImageSrc(imageObjectURL);
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
                            <p className={styles.value}>{recipe.portionCount} персон</p>
                        </span>
                    </span>
                </span>
            </span>
        </div>
    );
};
