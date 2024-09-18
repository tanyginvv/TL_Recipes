import React, { useState, useEffect, MouseEvent } from 'react';
import styles from "./likeAndFavouriteButtons.module.css";
import filledLike from "../../../assets/images/flledLike.svg";
import unfilledLike from "../../../assets/images/unfilledLike.svg";
import filledStar from "../../../assets/images/filledStar.svg";
import unfilledStar from "../../../assets/images/unfilledStar.svg";
import { LikeService } from '../../../services/likeService';
import { FavouriteService } from '../../../services/favouritesService';
import useStore from '../../../store/store';
import { IRecipePart } from '../../../models/types';

interface ButtonsProps {
    recipe: IRecipePart;
    onLikeChange?: (difference: number) => void;
    onFavouriteChange?: (difference: number) => void;
}

export const LikeAndFavouriteButtons: React.FC<ButtonsProps> = ({ recipe, onLikeChange, onFavouriteChange }) => {
    const [isLiked, setIsLiked] = useState<boolean>(false);
    const [isFavourite, setIsFavourite] = useState<boolean>(false);
    const [likeCount, setLikesCount] = useState<number>(recipe.likeCount);
    const [favouriteCount, setFavouritesCount] = useState<number>(recipe.favouriteCount);
    const { userId, setAuthOrRegistrWindowOpen } = useStore();

    useEffect(() => {
        const fetchInitialData = async () => {
            try {
                if (userId) {
                    setIsLiked(recipe?.isLiked ? recipe.isLiked : false);
                    setIsFavourite(recipe.isFavourited ? recipe.isFavourited : false);
                }                
            } catch (error) {
                console.error('Error fetching initial data:', error);
            }
        };

        if (recipe.id) {
            fetchInitialData();
        }
        
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [recipe.id, userId]);

    const handleLikeClick = async (event: MouseEvent<HTMLButtonElement>) => {
        event.stopPropagation();
        try {
            if (userId) {
                const likeService = new LikeService();
                let newLikesCount = likeCount;
                
                if (isLiked) {
                    await likeService.deleteLike(recipe.id);
                    newLikesCount -= 1;
                } else {
                    await likeService.createLike(recipe.id);
                    newLikesCount += 1;
                }
                
                setLikesCount(newLikesCount);
                setIsLiked(prev => !prev);
                
                if (onLikeChange) {
                    onLikeChange(newLikesCount - likeCount);
                }
            } else {
                setAuthOrRegistrWindowOpen(true);
            }
        } catch (error) {
            console.error('Error handling like click:', error);
        }
    };

    const handleFavouriteClick = async (event: MouseEvent<HTMLButtonElement>) => {
        event.stopPropagation();
        try {
            if (userId) {
                const favouriteService = new FavouriteService();
                let newFavouritesCount = favouriteCount;
                
                if (isFavourite) {
                    await favouriteService.deleteFavourite(recipe.id);
                    newFavouritesCount -= 1;
                } else {
                    await favouriteService.createFavourite(recipe.id);
                    newFavouritesCount += 1;
                }
                
                setFavouritesCount(newFavouritesCount);
                setIsFavourite(prev => !prev);
                
                if (onFavouriteChange) {
                    onFavouriteChange(newFavouritesCount - favouriteCount);
                }
            } else {
                setAuthOrRegistrWindowOpen(true);
            }
        } catch (error) {
            console.error('Error handling favourite click:', error);
        }
    };

    return (
        <div className={styles.buttonsContainer}>
            <button className={styles.button} onClick={handleFavouriteClick}>
                <img
                    className={styles.btnIcon}
                    src={isFavourite ? filledStar : unfilledStar}
                    alt="Favourite"
                />
                <p>{favouriteCount}</p>
            </button>
            <button className={styles.button} onClick={handleLikeClick}>
                <img
                    className={styles.btnIcon}
                    src={isLiked ? filledLike : unfilledLike}
                    alt="Like"
                />
                <p>{likeCount}</p>
            </button>
        </div>
    );
};
