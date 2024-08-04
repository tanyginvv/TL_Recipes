import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { PageHeader } from "./pageHeader/pageHeader";
import styles from "./userPage.module.css";
import useStore from "../../store/store";
import { UserForm } from "./userForm/userForm";

export const UserPage = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id?: string }>();
    const { userId } = useStore();

    useEffect(() => {
        if (Number(id) !== Number(userId)) {
            navigate("/allRecipesPage");
        }
    }, [id, userId, navigate]);

    return (
        <div className={styles.userPageContainer}>
            <PageHeader />
            <UserForm userId={Number(id)} />
        </div>
    );
};
