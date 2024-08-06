import { useState, useEffect, ChangeEvent, FormEvent, forwardRef, useImperativeHandle } from 'react';
import styles from "./userForm.module.css";
import { UserService } from "../../../services/userService";
import { IUser, IUserUpdate } from '../../../models/types';

interface UserFormProps {
    userId: number;
    isEditing: boolean;
    setIsEditing: (isEditing: boolean) => void;
    onUserUpdated: (updatedUser: IUser) => void;
}

export interface UserFormHandle {
    submitForm: () => void;
    resetForm: () => void;
}

export const UserForm = forwardRef<UserFormHandle, UserFormProps>(({ userId, isEditing, setIsEditing, onUserUpdated }, ref) => {
    const [formData, setFormData] = useState({
        name: '',
        login: '',
        description: '',
        oldPassword: '',
        newPassword: ''
    });

    const [initialFormData, setInitialFormData] = useState(formData);
    const [error, setError] = useState<string | null>(null);
    const [fieldErrors, setFieldErrors] = useState({
        name: false,
        login: false,
        oldPassword: false,
        newPassword: false,
    });

    useImperativeHandle(ref, () => ({
        submitForm: () => {
            const formElement = document.querySelector(`form.${styles.userForm}`) as HTMLFormElement;
            if (formElement) {
                formElement.requestSubmit();
            }
        },
        resetForm: () => {
            setFormData(initialFormData);
            setError(null);
            setFieldErrors({
                name: false,
                login: false,
                oldPassword: false,
                newPassword: false,
            });
        }
    }));

    useEffect(() => {
        const fetchUserData = async () => {
            if (userId) {
                const userService = new UserService();
                try {
                    const user = await userService.fetchUser(userId);
                    const userData = {
                        name: user.name,
                        login: user.login,
                        description: user.description || '',
                        oldPassword: '', 
                        newPassword: ''  
                    };
                    setFormData(userData);
                    setInitialFormData(userData);
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
        setError(null);
        setFieldErrors({
            name: false,
            login: false,
            oldPassword: false,
            newPassword: false,
        });

        if (!formData.name || !formData.login) {
            setError("Пожалуйста, заполните все обязательные поля.");
            setFieldErrors({
                name: !formData.name,
                login: !formData.login,
                oldPassword: false,
                newPassword: false,
            });
            return;
        }

        if (formData.oldPassword && !formData.newPassword) {
            setError("Пожалуйста, введите новый пароль.");
            setFieldErrors(prevErrors => ({
                ...prevErrors,
                oldPassword: true,
                newPassword: true,
            }));
            return;
        }

        const userService = new UserService();
        try {
            const body: IUserUpdate = {
                name: formData.name,
                description: formData.description,
                login: formData.login,
                oldPasswordHash: formData.oldPassword,
                newPasswordHash: formData.newPassword
            };

            await userService.updateUser(userId, body);

            const updatedUser = await userService.fetchUser(userId);
            onUserUpdated(updatedUser);

            setIsEditing(false);
            alert("Ваши данные успешно обновлены");
        } catch (error) {
            console.error("Ошибка при обновлении данных пользователя", error);
            setError("Ошибка при обновлении данных. Попробуйте снова.");
        }
    };

    return (
        <form className={styles.userForm} onSubmit={handleSubmit}>
            <div className={styles.formsInputs}>
                <span className={`${styles.formInput} ${fieldErrors.name ? styles.error : ''}`}>
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
                <span className={`${styles.formInput} ${fieldErrors.login ? styles.error : ''}`}>
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
                        <span className={`${styles.formInput} ${fieldErrors.oldPassword ? styles.error : ''}`}>
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
                        <span className={`${styles.formInput} ${fieldErrors.newPassword ? styles.error : ''}`}>
                            <label htmlFor="newPassword" className={styles.inputLabel}>Новый пароль</label>
                            <input
                                type="password"
                                className={styles.inputText}
                                id="newPassword"
                                name="newPassword"
                                value={formData.newPassword}
                                onChange={handleChange}
                                minLength={8}
                                disabled={!formData.oldPassword}
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
                {error && <p className={styles.formError}>{error}</p>}
            </div>      
        </form>
    );
});
