import React, { useState, useEffect, MouseEvent } from 'react';
import styles from "./likeAndFavouriteButtons.module.css";
import filledLike from "../../../assets/images/flledLike.svg";
import unfilledLike from "../../../assets/images/unfilledLike.svg";
import filledStar from "../../../assets/images/filledStar.svg";
import unfilledStar from "../../../assets/images/unfilledStar.svg";
import { LikeService } from '../../../services/likeService';
import { FavouriteService } from '../../../services/favouritesService';
import useStore from '../../../store/store';

interface ButtonsProps {
    recipeId: number;
}

export const LikeAndFavouriteButtons: React.FC<ButtonsProps> = ({ recipeId }) => {
    const [isLiked, setIsLiked] = useState<boolean>(false);
    const [isFavourite, setIsFavourite] = useState<boolean>(false);
    const [likesCount, setLikesCount] = useState<number>(0);
    const [favouritesCount, setFavouritesCount] = useState<number>(0);
    const { userId, setAuthOrRegistrWindowOpen } = useStore();

    useEffect(() => {
        const fetchInitialData = async () => {
            try {
                const likeService = new LikeService();
                const favouriteService = new FavouriteService();

                const likeStatus = await likeService.getLikeStatus(Number(userId), recipeId);
                setIsLiked(likeStatus.value.isLiked);

                const favouriteStatus = await favouriteService.getFavouriteStatus(Number(userId), recipeId);
                setIsFavourite(favouriteStatus.value.isFavourite);
            } catch (error) {
                console.error('Error fetching initial data:', error);
            }
        };

        const fetchInitialCountData = async () =>{
            try{
                const likeService = new LikeService();
                const favouriteService = new FavouriteService();
                const likeCount = await likeService.getLikesCount(recipeId);
                setLikesCount(likeCount);

                const favouriteCount = await favouriteService.getFavouritesCount(recipeId);
                setFavouritesCount(favouriteCount);

            }catch(error) {
                console.error('Error fetching initial data:', error);
            }
        }

        if( recipeId && userId){
            fetchInitialData();
        }

        if( recipeId ){
            fetchInitialCountData();
        }
        
    }, [recipeId, userId]);

    
    const handleLikeClick = async (event: MouseEvent<HTMLButtonElement>) => {
        event.stopPropagation(); 
        try {
            if( userId ) {
                const likeService = new LikeService();
                if (isLiked) {
                    await likeService.deleteLike(recipeId, Number(userId));
                    setLikesCount(likesCount - 1);
                } else {
                    await likeService.createLike(recipeId, Number(userId));
                    setLikesCount(likesCount + 1);
                }
                setIsLiked(!isLiked);
            } else {
                setAuthOrRegistrWindowOpen(true)
            }
        } catch (error) {
            console.error('Error handling like click:', error);
        }
    };

    const handleFavouriteClick = async (event: MouseEvent<HTMLButtonElement>) => {
        event.stopPropagation(); 
        try {
            if( userId ){
                const favouriteService = new FavouriteService();
                if (isFavourite) {
                    await favouriteService.deleteFavourite(recipeId, Number(userId));
                    setFavouritesCount(favouritesCount - 1);
                } else {
                    await favouriteService.createFavourite(recipeId, Number(userId));
                    setFavouritesCount(favouritesCount + 1);
                }
                setIsFavourite(!isFavourite);
            }else {
                setAuthOrRegistrWindowOpen(true)
            }
        } catch (error) {
            console.error('Error handling favourite click:', error);
        }
    };

    return (
        <div className={styles.buttonsContainer} >
            <button className={styles.button} onClick={handleFavouriteClick}>
                <img
                    className={styles.btnIcon}
                    src={isFavourite ? filledStar : unfilledStar}
                    alt="Favourite"
                />
                <p>{favouritesCount}</p>
            </button>
            <button className={styles.button} onClick={handleLikeClick}>
                <img
                    className={styles.btnIcon}
                    src={isLiked ? filledLike : unfilledLike}
                    alt="Like"
                />
                <p>{likesCount}</p>
            </button>
        </div>
    );
};
