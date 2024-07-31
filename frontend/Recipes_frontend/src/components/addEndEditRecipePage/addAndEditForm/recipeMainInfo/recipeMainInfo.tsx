import React from 'react';
import styles from './recipeMainInfo.module.css';
import { ITag } from "../../../../models/types";
import { TagInput } from '../tagInput/tagInput';
import { ImageUpload } from '../imageUpload/imageUpload';

interface RecipeMainInfoProps {
    name: string;
    setName: (name: string) => void;
    description: string;
    setDescription: (description: string) => void;
    cookTime: number;
    setCookTime: (cookTime: number) => void;
    portions: number;
    setPortions: (portions: number) => void;
    image: File | null;
    setImage: (file: File | null) => void;
    tags: ITag[];
    setTags: (tags: ITag[]) => void;
    tagInput: string;
    setTagInput: (tagInput: string) => void;
}

export const RecipeMainInfo: React.FC<RecipeMainInfoProps> = ({
    name,
    setName,
    description,
    setDescription,
    cookTime,
    setCookTime,
    portions,
    setPortions,
    image,
    setImage,
    tags,
    setTags,
    tagInput,
    setTagInput
}) => {
    return (
        <div className={styles.recipeMainInfo}>
            <ImageUpload image={image} onImageChange={setImage} />
            <div className={styles.textInfo}>
                <input
                    type="text"
                    placeholder="Название рецепта"
                    className={styles.inputText}
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />
                <textarea
                    className={styles.inputTextarea}
                    maxLength={150}
                    placeholder="Краткое описание рецепта (150 символов)"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                />
                <TagInput
                    tags={tags}
                    setTags={setTags}
                    tagInput={tagInput}
                    setTagInput={setTagInput}
                />
                <div className={styles.selectWrapper}>
                    <span className={styles.selectItem}>
                        <select
                            value={cookTime}
                            onChange={(e) => setCookTime(Number(e.target.value))}
                            className={styles.selectInput}
                        >
                            <option value="" disabled>Время готовки</option>
                            {Array.from({ length: 12 }, (_, i) => (i + 1) * 10).map((option) => (
                                <option key={option} value={option}>{option}</option>
                            ))}
                        </select>
                        <p className={styles.itemText}>Минут</p>
                    </span>
                    <span className={styles.selectItem}>
                        <select
                            value={portions}
                            onChange={(e) => setPortions(Number(e.target.value))}
                            className={styles.selectInput}
                        >
                            <option value="" disabled>Порций в блюде</option>
                            {Array.from({ length: 20 }, (_, i) => i + 1).map((option) => (
                                <option key={option} value={option}>{option}</option>
                            ))}
                        </select>
                        <p className={styles.itemText}>Персон</p>
                    </span>
                </div>
            </div>
        </div>
    );
};