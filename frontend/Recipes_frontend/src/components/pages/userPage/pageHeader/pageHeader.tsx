import styles from "./pageHeader.module.css";
import backspace from "../../../../assets/images/backspace.svg";
import { useNavigate } from "react-router-dom";

interface Props {
    onPublish: () => void;
    isEditing: boolean;
    onEdit: () => void ;
    onCancel: () => void ;
}

export const PageHeader = ({ onPublish, isEditing, onEdit, onCancel }: Props) => {
    const navigate = useNavigate();

    const allRecipesPageHandler = () => {
        navigate("/allRecipesPage");
    };

    return (
        <div className={styles.header}>
            <button className={styles.buttonBack} onClick={allRecipesPageHandler}>
                <img src={backspace} alt="back" />
                <p>Назад</p>
            </button>
            <div className={styles.titleContainer}>
                <h1 className={styles.title}>Мой профиль</h1>
                <div className={styles.titleButton}>
                    {isEditing ? (
                        <>
                            <button className={styles.buttonСancel} onClick={onCancel}>
                                <p>Отменить</p>
                            </button>
                            <button className={styles.buttonPost} onClick={onPublish}>
                                <p>Сохранить изменения</p>
                            </button>
                         </>
                    ) : (
                        <button className={styles.buttonPost} onClick={onEdit}>
                            <p>Редактировать данные</p>
                        </button>
                    )}
                </div>
            </div>
        </div>
    );
};
