import { useState, useEffect, ChangeEvent, FormEvent, forwardRef, useImperativeHandle } from 'react';
import styles from "./userForm.module.css";
import { UserService } from "../../../services/userService";
import { IUser, IUserUpdate } from '../../../models/types';
import useStore from '../../../store/store';

interface UserFormProps {
    user: IUser | null;
    isEditing: boolean;
    setIsEditing: (isEditing: boolean) => void;
}

export interface UserFormHandle {
    submitForm: () => void;
    resetForm: () => void;
}

export const UserForm = forwardRef<UserFormHandle, UserFormProps>(({ user, isEditing, setIsEditing }, ref) => {
    const initialState = {
        name: '',
        login: '',
        description: '',
        oldPassword: '',
        newPassword: ''
    };

    const [formData, setFormData] = useState(initialState);
    const [initialFormData, setInitialFormData] = useState(initialState);
    const [error, setError] = useState<string | null>(null);
    const [fieldErrors, setFieldErrors] = useState({
        name: false,
        login: false,
        oldPassword: false,
        newPassword: false,
    });
    
    const { setNotification } = useStore();

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
        if (user) {
            const userData = {
                name: user.name,
                login: user.login,
                description: user.description || '',
                oldPassword: '',
                newPassword: '',
            };
            setFormData(userData);
            setInitialFormData(userData);
        }
    }, [user]);

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prevData => ({ ...prevData, [name]: value }));
    };

    const resetFieldErrors = () => {
        setFieldErrors({
            name: false,
            login: false,
            oldPassword: false,
            newPassword: false,
        });
        setError(null)
    };

    const validateForm = () => {
        resetFieldErrors();
        if (!formData.name || !formData.login) {
            setError("Пожалуйста, заполните все обязательные поля.");
            setFieldErrors({
                name: !formData.name,
                login: !formData.login,
                oldPassword: false,
                newPassword: false,
            });
            return false;
        }

        if (formData.oldPassword && !formData.newPassword) {
            setError("Пожалуйста, введите новый пароль.");
            setFieldErrors(prev => ({ ...prev, oldPassword: true, newPassword: true }));
            return false;
        }

        if (!formData.oldPassword && formData.newPassword) {
            setError("Пожалуйста, введите старый пароль.");
            setFieldErrors(prev => ({ ...prev, oldPassword: true, newPassword: true }));
            return false;
        }

        return true;
    };

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!validateForm()) return;

        const updatedData : IUserUpdate= {};
        if (formData.name !== initialFormData.name) {
            updatedData.name = formData.name;
        }
        if (formData.login !== initialFormData.login) {
            updatedData.login = formData.login;
        }
        if (formData.oldPassword && formData.oldPassword !== "") {
            updatedData.oldPassword = formData.oldPassword;
            updatedData.newPassword = formData.newPassword;
        }
        if (formData.description !== initialFormData.description) {
            updatedData.description = formData.description;
        }

        if (Object.keys(updatedData).length === 0) {
            setNotification("Нет изменений для сохранения.", "info");
            return;
        }
        const userService = new UserService();
        try {
            const body: IUserUpdate = updatedData;

            const error = await userService.updateUser(body);

            if ( error.errorMessage === undefined) {
                setIsEditing(false);
                setNotification("Данные успешно обновлены", "success");
            } else {
                setNotification(`${error.errorMessage}`, "error");
            }
          
        } catch (error) {
            console.error("Ошибка при обновлении данных пользователя", error);
            setError("Ошибка при обновлении данных. Попробуйте снова.");
        }
    };

    return (
        <form className={styles.userForm} onSubmit={handleSubmit}>
            <div className={styles.formsInputs}>
                <div className={`${styles.formInput} ${fieldErrors.name ? styles.error : ''}`}>
                    <label htmlFor="name" className={styles.inputLabel}>Имя</label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        className={styles.inputText}
                        value={formData.name}
                        onChange={handleChange}
                        disabled={!isEditing}
                    />
                </div>
                <div className={`${styles.formInput} ${fieldErrors.login ? styles.error : ''}`}>
                    <label htmlFor="login" className={styles.inputLabel}>Логин</label>
                    <input
                        type="text"
                        id="login"
                        name="login"
                        className={styles.inputText}
                        value={formData.login}
                        onChange={handleChange}
                        disabled={!isEditing}
                    />
                </div>

                {!isEditing ? (
                    <div className={styles.formInput}>
                        <label htmlFor="oldPassword" className={styles.inputLabel}>Пароль</label>
                        <input type="password" id="oldPassword" value="********" className={styles.inputText} disabled />
                    </div>
                ) : (
                    <>
                        <div className={`${styles.formInput} ${fieldErrors.oldPassword ? styles.error : ''}`}>
                            <label htmlFor="oldPassword" className={styles.inputLabel}>Старый пароль</label>
                            <input
                                type="password"
                                id="oldPassword"
                                name="oldPassword"
                                className={styles.inputText}
                                value={formData.oldPassword}
                                onChange={handleChange}
                            />
                        </div>

                        <div className={`${styles.formInput} ${fieldErrors.newPassword ? styles.error : ''}`}>
                            <label htmlFor="newPassword" className={styles.inputLabel}>Новый пароль</label>
                            <input
                                type="password"
                                id="newPassword"
                                name="newPassword"
                                className={styles.inputText}
                                value={formData.newPassword}
                                onChange={handleChange}
                                minLength={8}
                                disabled={!formData.oldPassword}
                            />
                        </div>
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
