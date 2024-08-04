import { useState, useRef, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { PageHeader } from "./pageHeader/pageHeader";
import styles from "./userPage.module.css";
import useStore from "../../store/store";
import { UserForm, UserFormHandle } from "./userForm/userForm";
import { UserRecipesHeader } from './userRecipesHeader/userRecipesHeader';
import { RecipesList } from './recipesList/recipesList';
import { UserService } from '../../services/userService';
import { IUser } from '../../models/types';

export const UserPage = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id?: string }>();
    const { userId, setUser } = useStore();
    const [isEditing, setIsEditing] = useState(false);
    const [user, setUserState] = useState<IUser | null>(null);
    const [loading, setLoading] = useState(true);
    const formRef = useRef<UserFormHandle>(null);

    useEffect(() => {
        if (Number(id) !== Number(userId)) {
            navigate("/allRecipesPage");
        }
    }, [id, userId, navigate]);

    useEffect(() => {
        const fetchUserData = async () => {
            if (userId) {
                const userService = new UserService();
                try {
                    const userData = await userService.fetchUser(userId);
                    setUserState(userData);
                    setUser(userData);
                } catch (error) {
                    console.error("Ошибка при загрузке данных пользователя", error);
                } finally {
                    setLoading(false);
                }
            }
        };

        fetchUserData();
    }, [userId, setUser]);

    const onPublish = () => {
        if (formRef.current) {
            formRef.current.submitForm(); 
            setIsEditing(!isEditing);
        }
    };

    const onEdit = () => {
        setIsEditing(!isEditing);
    }

    if (loading) {
        return <p>Загрузка данных пользователя...</p>;
    }

    return (
        <div className={styles.userPageContainer}>
            <PageHeader isEditing={isEditing} onPublish={onPublish} onEdit={onEdit}/>
            {user && (
                <UserForm 
                    ref={formRef}
                    userId={Number(id)}
                    isEditing={isEditing}
                    setIsEditing={setIsEditing}
                    onUserUpdated={setUser}
                />
            )}
            <UserRecipesHeader recipesCount={Number(user?.recipesCount)}/>
            {userId && <RecipesList />}
        </div>
    );
};
