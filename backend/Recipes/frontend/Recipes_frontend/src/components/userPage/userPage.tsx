import { useState, useRef, useEffect } from 'react';
import { PageHeader } from "./pageHeader/pageHeader";
import styles from "./userPage.module.css";
import useStore from "../../store/store";
import { UserForm, UserFormHandle } from "./userForm/userForm";
import { UserRecipesHeader } from './userRecipesHeader/userRecipesHeader';
import { RecipesList } from './recipesList/recipesList';
import { UserService } from '../../services/userService';
import { IUser } from '../../models/types';
import { useNavigate } from 'react-router-dom';

export const UserPage = () => {
    const navigate = useNavigate()
    const { userId } = useStore();
    const [isEditing, setIsEditing] = useState(false);
    const [user, setUserState] = useState<IUser | null>(null);
    const [loading, setLoading] = useState(true);
    const formRef = useRef<UserFormHandle>(null);

    useEffect(() => {
        const fetchUserData = async () => {
            if (userId) {
                const userService = new UserService();
                try {
                    const userData = await userService.fetchUser();
                    setUserState(userData);
                } catch (error) {
                    console.error("Ошибка при загрузке данных пользователя", error);
                } finally {
                    setLoading(false);
                }
            }
            else {
                navigate("homePage")
            }
        };

        fetchUserData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [userId]);

    function onPublish() {
        if (formRef.current) {
            formRef.current.submitForm();
        }
    }

    const onEdit = () => {
        setIsEditing(true);
    };

    const onCancel = () => {
        setIsEditing(false); 
        if (formRef.current) {
            formRef.current.resetForm(); 
        }
    };

    const handleLikeChange = (difference: number) => {
        if (user) {
            setUserState(prevUser => {
                if (!prevUser) return prevUser;
                return {
                    ...prevUser,
                    likeCount: prevUser.likeCount + difference,
                };
            });
        }
    };
    
    const handleFavouriteChange = (difference: number) => {
        if (user) {
            setUserState(prevUser => {
                if (!prevUser) return prevUser;
                return {
                    ...prevUser,
                    favouriteCount: prevUser.favouriteCount + difference,
                };
            });
        }
    };
        

    if (loading) {
        return <p>Загрузка данных пользователя...</p>;
    }

    return (
        <div className={styles.userPageContainer}>
            <PageHeader isEditing={isEditing} onPublish={onPublish} onEdit={onEdit} onCancel={onCancel}/>
            {user && (
                <UserForm 
                    ref={formRef}
                    user={user}
                    isEditing={isEditing}
                    setIsEditing={setIsEditing}
                />
            )}
            <UserRecipesHeader 
                recipesCount={user?.recipeCount || 0}
                favouritesCount={user?.favouriteCount || 0} 
                likesCount={user?.likeCount || 0}
            />
            {userId && (
                <RecipesList 
                    onLikeChange={handleLikeChange} 
                    onFavouriteChange={handleFavouriteChange}
                />
            )}
        </div>
    );
};
