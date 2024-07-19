import React, { ChangeEvent } from 'react';
import styles from './imageUpload.module.css';
import download from "../../../../assets/images/download.svg";

interface ImageUploadProps {
    image: File | null;
    onImageChange: (file: File | null) => void;
}

export const ImageUpload: React.FC<ImageUploadProps> = ({ image, onImageChange }) => {
    const handleImageChange = (event: ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            onImageChange(file);
        }
    };

    return (
        <label htmlFor="recipeImage" className={styles.recipeImgPlace}>
            <input
                type="file"
                accept=".png, .jpg, .jpeg"
                id="recipeImage"
                className={styles.hiddenInput}
                onChange={handleImageChange}
            />
            <span className={styles.imagePlaceholder}>
                {image ? (
                    <img
                        src={URL.createObjectURL(image)}
                        style={{ width: "100%", height: "100%", objectFit: "contain" }}
                        alt="Selected recipe"
                        className={styles.selectedImage}
                    />
                ) : (
                    <>
                        <img src={download} alt="download icon" />
                        <span className={styles.imgLabel}>Загрузите фото готового блюда</span>
                    </>
                )}
            </span>
        </label>
    );
};