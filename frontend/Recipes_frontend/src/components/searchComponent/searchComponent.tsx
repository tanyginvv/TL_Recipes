import { useEffect, useState } from 'react';
import styles from './searchComponent.module.css';
import { ITag } from '../../models/types';

interface SearchComponentProps {
    initialSearchTerms?: string[];
    onSearchTermsChange: (terms: string[]) => void;
    onSearch: () => void;
}

export const SearchComponent = ({ initialSearchTerms = [], onSearchTermsChange, onSearch }: SearchComponentProps) => {
    const [tags, setTags] = useState<ITag[]>([]);
    const [userInputs, setUserInputs] = useState<string[]>(initialSearchTerms);
    const [currentInput, setCurrentInput] = useState('');

    const fetchTags = async () => {
        try {
            const response = await fetch('http://localhost:5218/api/tags');
            if (response.ok) {
                const data = await response.json();
                setTags(data);
            } else {
                console.error('Failed to fetch tags:', response.statusText);
            }
        } catch (error) {
            console.error('Error fetching tags:', error);
        }
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
