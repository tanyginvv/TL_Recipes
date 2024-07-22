import { forwardRef, useImperativeHandle, useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import styles from './recipeForm.module.css';
import { RecipeMainInfo } from './recipeMainInfo/recipeMainInfo';
import { IngredientsSection } from './ingredientsSection/ingredientsSection';
import { StepsSection } from './stepsSection/stepSection';
import { IIngredient, ITag, IStep } from "../../../models/types";

export const RecipeForm = forwardRef((_, ref) => {
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
                        setPortions(recipe.portionCount);
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
                alert('Ошибка при загрузке изображения');
                return null;
            }
        } catch (error) {
            alert('Ошибка: ' + error);
            return null;
        }
    };

    const submitForm = async () => {
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
            portionCount: portions,
            imageUrl: newImageUrl,
            ingredients,
            steps: steps.map((step, index) => ({ stepNumber: index + 1, stepDescription: step.stepDescription })),
            tags
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
            <RecipeMainInfo
                name={name}
                setName={setName}
                description={description}
                setDescription={setDescription}
                cookTime={cookTime}
                setCookTime={setCookTime}
                portions={portions}
                setPortions={setPortions}
                image={image}
                setImage={setImage}
                tags={tags}
                setTags={setTags}
                tagInput={tagInput}
                setTagInput={setTagInput}
            />
            <div className={styles.recipeListsInfo}>
                <IngredientsSection
                    ingredients={ingredients}
                    setIngredients={setIngredients}
                />
                <StepsSection
                    steps={steps}
                    setSteps={setSteps}
                />
            </div>
        </form>
    );
});
