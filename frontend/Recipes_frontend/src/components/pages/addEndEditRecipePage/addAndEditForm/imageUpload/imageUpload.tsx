import React, { ChangeEvent } from 'react';
import styles from './imageUpload.module.css';
import download from "../../../../../assets/images/download.svg";

interface ImageUploadProps {
    image: File | null;
    onImageChange: (file: File | null) => void;
    isImageValid: boolean; // Add validation prop
}

export const ImageUpload: React.FC<ImageUploadProps> = ({ image, onImageChange, isImageValid }) => {
    const handleImageChange = (event: ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            onImageChange(file);
        }
    };

    return (
        <label htmlFor="recipeImage" className={`${styles.recipeImgPlace} ${!isImageValid ? styles.invalid : ''}`}>
            { image ? (<img
                        src={URL.createObjectURL(image)}
                        alt="Selected recipe"
                        className={styles.selectedImage}
                    />): <></>}
            <input
                type="file"
                accept=".png, .jpg, .jpeg, .webp"
                id="recipeImage"
                className={styles.hiddenInput}
                onChange={handleImageChange}
            />
            {image ? (
                    <></>
                ) :
               (<span className={styles.imagePlaceholder}>
                    <>
                        <img src={download} alt="download icon" />
                        <span className={styles.imgLabel}>Загрузите фото готового блюда</span>
                    </>
                
                </span>
                )}

        </label>
    );
};