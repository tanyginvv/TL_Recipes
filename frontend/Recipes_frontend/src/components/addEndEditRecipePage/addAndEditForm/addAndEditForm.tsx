import { forwardRef, useImperativeHandle, useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import styles from './addAndEditForm.module.css';
import { RecipeMainInfo } from './recipeMainInfo/recipeMainInfo';
import { IngredientsSection } from './ingredientsSection/ingredientsSection';
import { StepsSection } from './stepsSection/stepsSection';
import { IIngredient, ITag, IStep, IRecipeSubmit } from "../../../models/types";
import { RecipeService } from '../../../services/recipeServices';
import { ImageService } from '../../../services/imageService';

const recipeService = new RecipeService();
const imageService = new ImageService();

export const AddAndEditForm = forwardRef((_, ref) => {
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
                    const recipe = await recipeService.fetchRecipeById(id);
                    setName(recipe.name);
                    setDescription(recipe.description);
                    setCookTime(recipe.cookTime);
                    setPortions(recipe.portionCount);
                    setTags(recipe.tags);
                    setIngredients(recipe.ingredients);
                    setSteps(recipe.steps);
                    setImageUrl(recipe.imageUrl);
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
                    const imageObjectURL = await imageService.fetchImage(imageUrl);
                    if (imageObjectURL) {
                        const response = await fetch(imageObjectURL);
                        const blob = await response.blob();
                        const file = new File([blob], imageUrl, { type: blob.type });
                        setImage(file);
                    }
                } catch (error) {
                    console.error('Ошибка при загрузке изображения:', error);
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

    const submitForm = async () => {
        if (!validateForm()) {
            return;
        }

        let newImageUrl = imageUrl;
        if (image) {
            try {
                newImageUrl = await imageService.uploadImage(image);
            } catch (error) {
                alert('Ошибка: ' + error);
                return;
            }
        }

        const recipeData: IRecipeSubmit = {
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
            await recipeService.submitRecipe(recipeData, id);
            alert(id ? 'Рецепт успешно обновлен' : 'Рецепт успешно добавлен');
            navigate('/allRecipesPage');
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
