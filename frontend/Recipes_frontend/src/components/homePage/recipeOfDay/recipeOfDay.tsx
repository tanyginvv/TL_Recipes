import { useState, useEffect } from 'react';
import styles from './recipeOfDay.module.css';
import like from "../../../assets/images/like.svg";
import time from "../../../assets/images/time.svg";
import recipeOfDay from "../../../assets/images/recipeDay.svg";
import { RecipeService } from '../../../services/recipeServices';
import { ImageService } from '../../../services/imageService';
import { UserService } from '../../../services/userService';
import { LikeService } from '../../../services/likeService';
import { IRecipe } from '../../../models/types';
import { useNavigate } from 'react-router-dom';

export const RecipeOfDay = () => {
    const navigate = useNavigate();

    const recipeService = new RecipeService();
    const imageService = new ImageService();
    const userService = new UserService();
    const likeService = new LikeService();

    const [recipe, setRecipe] = useState<IRecipe | null>(null);
    const [imageSrc, setImageSrc] = useState<string>("");
    const [login, setLogin] = useState<string>("");
    const [likes, setLikes] = useState(0);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await recipeService.fetchRecipeOfDay();
                setRecipe(data);

                if (data) {
                    if (data.imageUrl) {
                        const imageObjectURL = await imageService.fetchImage(data.imageUrl);
                        setImageSrc(imageObjectURL || "");
                    }

                    if (data.userId) {
                        const userLogin = await userService.fetchUserLogin(data.userId);
                        setLogin(userLogin.login);
                    }

                    const recipesLikes = await likeService.getLikesCount(data.id);
                    setLikes(recipesLikes);
                }
            } catch (error) {
                console.error('Ошибка:', error);
            }
        };

        fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <div className={styles.example} onClick={()=> navigate(`/detailRecipesPage/${recipe?.id}`)}>
            <img className={styles.img} src={imageSrc} alt="Recipe of the Day" />
            <div className={styles.exampleContent}>
                <span className={styles.exampleLikeAndFavourites}>
                    <span className={styles.exampleInfo}>
                        <img className={styles.infoImg} src={like} alt="Likes" />
                        <p className={styles.infoText}>{likes}</p>
                    </span>
                    <span className={styles.exampleInfo}>
                        <img className={styles.infoImg} src={time} alt="Time" />
                        <p className={styles.infoText}>{recipe?.cookTime + " минут"}</p>
                    </span>
                </span>
                <img className={styles.recipeDayImg} src={recipeOfDay} alt="Recipe of the Day" />
                <span className={styles.exampleHeader}>
                    {recipe?.name}
                </span>
                <span className={styles.exampleText}>
                    {recipe?.description}
                </span>
            </div>
            <span className={styles.exampleTag}>{"@" + login}</span>
        </div>
    );
};
