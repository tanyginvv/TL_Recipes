import { useState, useEffect, ChangeEvent, FormEvent, forwardRef, useImperativeHandle } from 'react';
import styles from "./userForm.module.css";
import { UserService } from "../../../services/userService";
import { IUserUpdate } from '../../../models/types';

interface UserFormProps {
    userId: number;
    isEditing: boolean;
    setIsEditing: (isEditing: boolean) => void;
}

export interface UserFormHandle {
    submitForm: () => void;
}

export const UserForm = forwardRef<UserFormHandle, UserFormProps>(({ userId, isEditing, setIsEditing }, ref) => {
    const [formData, setFormData] = useState({
        name: '',
        login: '',
        description: '',
        oldPassword: '',
        newPassword: ''
    });

    useImperativeHandle(ref, () => ({
        submitForm: () => {
            const formElement = document.querySelector(`form.${styles.userForm}`) as HTMLFormElement;
            if (formElement) {
                formElement.requestSubmit();
            }
        }
    }));

    useEffect(() => {
        const fetchUserData = async () => {
            if (userId) {
                const userService = new UserService();
                try {
                    const user = await userService.fetchUser(userId);
                    setFormData({
                        name: user.name,
                        login: user.login,
                        description: user.description || '',
                        oldPassword: '', 
                        newPassword: ''  
                    });
                } catch (error) {
                    console.error("Ошибка при загрузке данных пользователя", error);
                }
            }
        };

        fetchUserData();
    }, [userId]);

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prevData => ({
            ...prevData,
            [name]: value
        }));
    };

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault(); 
        const userService = new UserService();
        try {
            const body: IUserUpdate = {
                name: formData.name,
                description: formData.description,
                login: formData.login,
                oldPasswordHash: formData.oldPassword,
                newPasswordHash: formData.newPassword
            };

            await userService.updateUser(Number(userId), body);
            setIsEditing(false);
            alert("Ваши данные успешно обновлены");
        } catch (error) {
            console.error("Ошибка при обновлении данных пользователя", error);
        }
    };

    return (
        <form className={styles.userForm} onSubmit={handleSubmit}>
            <div className={styles.formsInputs}>
                <span className={styles.formInput}>
                    <label htmlFor="name" className={styles.inputLabel}>Имя</label>
                    <input
                        type="text"
                        className={styles.inputText}
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        disabled={!isEditing}
                    />
                </span>
                <span className={styles.formInput}>
                    <label htmlFor="login" className={styles.inputLabel}>Логин</label>
                    <input
                        type="text"
                        className={styles.inputText}
                        id="login"
                        name="login"
                        value={formData.login}
                        onChange={handleChange}
                        disabled={!isEditing}
                    />
                </span>
                {!isEditing ? (
                    <span className={styles.formInput}>
                        <label htmlFor="oldPassword" className={styles.inputLabel}>Пароль</label>
                        <input
                            type="password"
                            className={styles.inputText}
                            id="oldPassword"
                            disabled
                            value="********"
                        />
                    </span>
                ) : (
                    <>
                        <span className={styles.formInput}>
                            <label htmlFor="oldPassword" className={styles.inputLabel}>Старый пароль</label>
                            <input
                                type="password"
                                className={styles.inputText}
                                id="oldPassword"
                                name="oldPassword"
                                value={formData.oldPassword}
                                onChange={handleChange}
                            />
                        </span>
                        <span className={styles.formInput}>
                            <label htmlFor="newPassword" className={styles.inputLabel}>Новый пароль</label>
                            <input
                                type="password"
                                className={styles.inputText}
                                id="newPassword"
                                name="newPassword"
                                value={formData.newPassword}
                                onChange={handleChange}
                                minLength={8}
                            />
                        </span>
                    </>
                )}
            </div>
            <div className={styles.formsInputs}>
                <textarea
                    className={styles.descriptionInput}
                    name="description"
                    maxLength={200}
                    placeholder="Напишите немного о себе"
                    value={formData.description}
                    onChange={handleChange}
                    disabled={!isEditing}
                />
            </div>
        </form>
    );
});
