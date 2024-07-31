import { useEffect, useState } from 'react';
import styles from './searchComponent.module.css';
import { ITag } from '../../models/types';
import { TagService } from '../../services/tagService';

interface SearchComponentProps {
    initialSearchTerms?: string[];
    onSearchTermsChange: (terms: string[]) => void;
    onSearch: () => void;
}

const tagService = new TagService();

export const SearchComponent = ({ initialSearchTerms = [], onSearchTermsChange, onSearch }: SearchComponentProps) => {
    const [tags, setTags] = useState<ITag[]>([]);
    const [userInputs, setUserInputs] = useState<string[]>(initialSearchTerms);
    const [currentInput, setCurrentInput] = useState('');

    const fetchTags = async () => {
        const data = await tagService.fetchTags();
        setTags(data);
    };

    useEffect(() => {
        fetchTags();
    }, []);

    useEffect(() => {
        onSearchTermsChange(userInputs);
    }, [userInputs, onSearchTermsChange]);

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setCurrentInput(event.target.value);
    };

    const handleInputKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter' && currentInput.trim()) {
            event.preventDefault();
            setUserInputs([...userInputs, currentInput.trim()]);
            setCurrentInput('');
        }
    };

    const handleRemoveInput = (index: number) => {
        const newUserInputs = userInputs.filter((_, i) => i !== index);
        setUserInputs(newUserInputs);
        onSearchTermsChange(newUserInputs);
    };

    const handleTagClick = (tagName: string) => {
        if (!userInputs.includes(tagName)) {
            const newUserInputs = [...userInputs, tagName];
            setUserInputs(newUserInputs);
            onSearchTermsChange(newUserInputs);
        }
    };

    const handleSearchClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        onSearch();
    };

    return (
        <div className={styles.searchContainer}>
            <form className={styles.searchForm}>
                <div className={styles.searchArea}>
                    <div className={styles.searchList}>
                        {userInputs.map((input, index) => (
                            <div key={index} className={styles.userInputs}>
                                {input}
                                <button
                                    type="button"
                                    className={styles.removeUserInputButton}
                                    onClick={() => handleRemoveInput(index)}
                                >
                                    x
                                </button>
                            </div>
                        ))}
                    </div>
                    <input
                        type="text"
                        placeholder="Введите тег или название блюда и нажмите Enter"
                        maxLength={30}
                        className={styles.searchInput}
                        value={currentInput}
                        onChange={handleInputChange}
                        onKeyPress={handleInputKeyPress}
                    />
                </div>
                <button type="submit" className={styles.submitButton} onClick={handleSearchClick}>Поиск</button>
            </form>
            <div className={styles.tags}>
                {tags.map((tag, index) => (
                    <div
                        key={index}
                        className={styles.tagExample}
                        onClick={() => handleTagClick(tag.name)}
                    >
                        {tag.name}
                    </div>
                ))}
            </div>
        </div>
    );
};
