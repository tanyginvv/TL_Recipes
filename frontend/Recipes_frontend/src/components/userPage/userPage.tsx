import { useState, useRef, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { PageHeader } from "./pageHeader/pageHeader";
import styles from "./userPage.module.css";
import useStore from "../../store/store";
import { UserForm, UserFormHandle } from "./userForm/userForm";

export const UserPage = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id?: string }>();
    const { userId } = useStore();
    const [isEditing, setIsEditing] = useState(false);
    const formRef = useRef<UserFormHandle>(null);

    const onPublish = () => {
        if (formRef.current) {
            formRef.current.submitForm(); 
            setIsEditing(!isEditing);
        }
    };

    const onEdit = () => {
        setIsEditing(!isEditing);
    }

    useEffect(() => {
        if (Number(id) !== Number(userId)) {
            navigate("/allRecipesPage");
        }
    }, [id, userId, navigate]);

    return (
        <div className={styles.userPageContainer}>
            <PageHeader isEditing={isEditing} onPublish={onPublish} onEdit={onEdit}/>
            <UserForm ref={formRef} userId={Number(id)} isEditing={isEditing} setIsEditing={setIsEditing} />
        </div>
    );
};
