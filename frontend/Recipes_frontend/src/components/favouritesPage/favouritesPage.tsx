import { useEffect } from "react";
import useStore from "../../store/store";
import styles from "./favouritesPage.module.css";
import { useNavigate } from "react-router-dom";
import { RecipeQueryType } from "../../models/types";
import { RecipesList } from "../recipesList/recipesList";

export const FavouritesPage = () => {
    const { userId } = useStore();
    const navigate = useNavigate();

    useEffect(() => {
        const fetchRecipes = async () => {
            if (!userId) {
                navigate("/");
            }
        };

        fetchRecipes();
    }, [ userId, navigate]);

    return (
        <div className={styles.favouritesContainer}>
           <h1 className={styles.title}>Избранное</h1>
           <RecipesList recipeQueryType={RecipeQueryType.Starred}/>
        </div>
    );
};