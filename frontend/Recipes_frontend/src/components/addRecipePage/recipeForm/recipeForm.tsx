import { useState, useEffect, useImperativeHandle, forwardRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import styles from "./recipeForm.module.css";
import download from "../../../assets/images/download.svg";
import close from "../../../assets/images/close.svg";
import { IIngredient, ITag, IStep } from "../../../models/types";

export const RecipeForm = forwardRef((props, ref) => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [, setIsUpdating] = useState<boolean>(false);
    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [image, setImage] = useState<File | null>(null);
    const [imageUrl, setImageUrl] = useState<string>("");
    const [portions, setPortions] = useState<number>(1);
    const [cookTime, setCookTime] = useState<number>(10);
    const [tags, setTags] = useState<ITag[]>([]);
    const [tagInput, setTagInput] = useState<string>("");
    const [ingredients, setIngredients] = useState<IIngredient[]>([{ title: '', description: '' }]);
    const [steps, setSteps] = useState<IStep[]>([{ stepNumber: 1, stepDescription: '' }]);

    useEffect(() => {
        if (id) {
            setIsUpdating(true);
            const fetchRecipe = async () => {
                try {
                    const response = await fetch(`http://localhost:5218/api/recipes/${id}`);
                    if (response.ok) {
                        const recipe = await response.json();
                        setName(recipe.name);
                        setDescription(recipe.description);
                        setCookTime(recipe.cookTime);
                        setPortions(recipe.countPortion);
                        setTags(recipe.tags);
                        setIngredients(recipe.ingredients);
                        setSteps(recipe.steps);
                        setImageUrl(recipe.imageUrl);
                    } else {
                        console.error('Ошибка при загрузке рецепта');
                    }
                } catch (error) {
                    console.error('Ошибка:', error);
                }
            };

            fetchRecipe();
        }
    }, [id]);

    useEffect(() => {
        if (imageUrl) {
            const fetchImage = async () => {
                try {
                    const response = await fetch(`http://localhost:5218/api/images/${imageUrl}`);
                    if (response.ok) {
                        const blob = await response.blob();
                        const file = new File([blob], imageUrl, { type: blob.type });
                        setImage(file);
                    } else {
                        console.error('Ошибка при загрузке изображения');
                    }
                } catch (error) {
                    console.error('Ошибка:', error);
                }
            };

            fetchImage();
        }
    }, [imageUrl]);

    const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            setImage(file);
        }
    };

    const handleTagInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setTagInput(event.target.value);
    };

    const handleTagInputKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter' && tagInput.trim() !== '') {
            event.preventDefault();
            if (!tags.some(tag => tag.name === tagInput.trim())) {
                setTags([...tags, { name: tagInput.trim() }]);
                setTagInput("");
            }
        }
    };

    const handleRemoveTag = (tagToRemove: string) => {
        setTags(tags.filter(tag => tag.name !== tagToRemove));
    };

    const handleIngredientChange = (index: number, key: keyof IIngredient, value: string) => {
        const newIngredients = [...ingredients];
        newIngredients[index] = {
            ...newIngredients[index],
            [key]: value
        };
        setIngredients(newIngredients);
    };

    const handleAddIngredient = () => {
        setIngredients([...ingredients, { title: '', description: '' }]);
    };

    const handleRemoveIngredient = (index: number) => {
        const newIngredients = ingredients.filter((_, i) => i !== index);
        setIngredients(newIngredients);
    };

    const handleStepChange = (index: number, value: string) => {
        const newSteps = [...steps];
        newSteps[index].stepDescription = value;
        setSteps(newSteps);
    };

    const handleAddStep = () => {
        setSteps([...steps, { stepNumber: steps.length + 1, stepDescription: '' }]);
    };

    const handleRemoveStep = (index: number) => {
        const newSteps = steps.filter((_, i) => i !== index);
        setSteps(newSteps);
    };

    const validateForm = () => {
        if (!name || !image || !description || !tags.length || !ingredients.length || !steps.length) {
            alert("Заполните все поля.");
            return false;
        }
        return true;
    };

    const submitImage = async () => {
        if (!image) return null;

        const formData = new FormData();
        formData.append('image', image);

        try {
            const response = await fetch('http://localhost:5218/api/images/upload', {
                method: 'POST',
                body: formData,
            });

            if (response.ok) {
                const data = await response.json();
                return data.fileName;
            } else {
                alert('Ошибка при загрузке изображения 2');
                return null;
            }
        } catch (error) {
            alert('Ошибка: ' + error);
            return null;
        }
    };

    const submitForm =  async () => {
        if (!validateForm()) {
            return;
        }

        let newImageUrl = imageUrl;
        if (image) {
            newImageUrl = await submitImage();
            if (!newImageUrl) {
                return;
            }
        }

        const recipeData = {
            name,
            description,
            cookTime,
            countPortion: portions,
            tags,
            ingredients,
            steps,
            imageUrl: newImageUrl
        };

        try {
            const response = await fetch(`http://localhost:5218/api/recipes/${id ? id : ''}`, {
                method: id ? 'PUT' : 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(recipeData),
            });

            if (response.ok) {
                alert(id ? 'Рецепт успешно обновлен' : 'Рецепт успешно добавлен');
                navigate('/allRecipesPage');
            } else {
                alert('Ошибка при обработке рецепта');
            }
        } catch (error) {
            alert('Ошибка: ' + error);
        }
    };

    useImperativeHandle(ref, () => ({
        submitForm
    }));

    return (
        <form className={styles.addRecipeForm} onSubmit={(e) => { e.preventDefault(); submitForm(); }}>
            <div className={styles.recipeMainInfo}>
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
                            <img src={URL.createObjectURL(image)} style={{width: "100%", height: "100%", objectFit: "contain"}} 
                            alt="Selected recipe" className={styles.selectedImage} onLoad={() => URL.createObjectURL(image)} />
                        ) : (
                            <>
                                <img src={download} alt="download icon" />
                                <span className={styles.imgLabel}>Загрузите фото готового блюда</span>
                            </>
                        )}
                    </span>
                </label>
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
                    <div className={styles.tagInputContainer}>
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
                            onKeyPress={handleTagInputKeyPress}
                        />
                    </div>
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
            <div className={styles.recipeListsInfo}>
                <div className={styles.ingredientsInfo}>
                    <h5 className={styles.ingredientTitle}>Ингредиенты</h5>
                    {ingredients.map((ingredient, index) => (
                        <span key={index} className={styles.ingredientItem}>
                            <span className={styles.closeBtnIngredient} onClick={() => handleRemoveIngredient(index)}>
                                <img className={styles.closeIcon} src={close} alt="Remove ingredient" />
                            </span>
                            <input 
                                type="text" 
                                placeholder="Заголовок" 
                                value={ingredient.title}
                                onChange={(e) => handleIngredientChange(index, 'title', e.target.value)}
                                className={styles.ingredientsTitle}
                            />
                            <textarea 
                                placeholder="Описание" 
                                value={ingredient.description}
                                onChange={(e) => handleIngredientChange(index, 'description', e.target.value)}
                                className={styles.ingredientsDescription}
                            />
                        </span>
                    ))}
                    <button type="button" onClick={handleAddIngredient} className={styles.addIngredientButton}>Добавить ингредиент</button>
                </div>
                <div className={styles.stepsInfo}>
                    {steps.map((step, index) => (
                        <span key={index} className={styles.stepItem}>
                            <span className={styles.closeBtnStep} onClick={() => handleRemoveStep(index)}>
                                <p className={styles.stepNumber}>Шаг {index + 1}</p>
                                <img className={styles.closeIcon} src={close} alt="Remove step" />
                            </span>
                            <textarea 
                                placeholder="Описание шага" 
                                value={step.stepDescription}
                                onChange={(e) => handleStepChange(index, e.target.value)}
                                className={styles.stepDescription}
                            />
                        </span>
                    ))}
                    <button type="button" onClick={handleAddStep} className={styles.addStepButton}>Добавить шаг</button>
                </div>
            </div>
        </form>
    );
});
