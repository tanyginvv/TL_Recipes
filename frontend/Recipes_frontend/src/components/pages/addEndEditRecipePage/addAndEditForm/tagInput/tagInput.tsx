import React from 'react';
import styles from './tagInput.module.css';
import { ITag } from "../../../../../models/types";
import useStore from '../../../../../store/store';

interface TagInputProps {
    tags: ITag[];
    setTags: (tags: ITag[]) => void;
    tagInput: string;
    setTagInput: (tagInput: string) => void;
    areTagsValid: boolean;
}

export const TagInput: React.FC<TagInputProps> = ({ tags, setTags, tagInput, setTagInput, areTagsValid }) => {
    const { setNotification } = useStore();

    const handleTagInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTagInput(event.target.value);
    };

    const handleTagInputKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter' && tagInput.trim() !== '') {
            event.preventDefault();
            if (tags.length >= 5) {
                setNotification('Максимально вы можете добавить 5 тегов', "error");
                return;
            }
            if (!tags.some(tag => tag.name.toLowerCase() === tagInput.trim().toLowerCase())) {
                setTags([...tags, { name: tagInput.trim() }]);
                setTagInput("");
            }
        }
    }

    const handleRemoveTag = (tagToRemove: string) => {
        setTags(tags.filter(tag => tag.name !== tagToRemove));
    };

    return (
        <div className={`${styles.tagInputContainer} ${!areTagsValid ? styles.invalid : ''}`}>
            <div className={styles.tagsContainer}>
                {tags.map((tag, index) => (
                    <div key={index} className={styles.tag}>
                        {tag.name}
                        <button type="button" className={styles.removeTagButton} onClick={() => handleRemoveTag(tag.name)}>x</button>
                    </div>
                ))}
            </div>
            <input
                type="text"
                placeholder="Добавить теги"
                className={styles.inputTag}
                value={tagInput}
                onChange={handleTagInputChange}
                onKeyDown={handleTagInputKeyPress}
            />
        </div>
    );
};
