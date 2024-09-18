import React from 'react';
import styles from './confirmDeleteModal.module.css';

interface ConfirmDeleteModalProps {
    onConfirm: () => void;
    onCancel: () => void;
}

export const ConfirmDeleteModal: React.FC<ConfirmDeleteModalProps> = ({ onConfirm, onCancel }) => {
    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent}>
                <h2>Подтвердите удаление</h2>
                <p>Вы точно хотите удалить этот рецепт? Это действие необратимо.</p>
                <div className={styles.modalButtons}>
                    <button className={styles.cancelButton} onClick={onCancel}>
                        Отмена
                    </button>
                    <button className={styles.confirmButton} onClick={onConfirm}>
                        Удалить
                    </button>
                </div>
            </div>
        </div>
    );
};
