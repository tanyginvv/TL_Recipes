import { forwardRef, useImperativeHandle, useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import styles from './addAndEditForm.module.css';
import { RecipeMainInfo } from './recipeMainInfo/recipeMainInfo';
import { IngredientsSection } from './ingredientsSection/ingredientsSection';
import { StepsSection } from './stepsSection/stepsSection';
import { IIngredient, ITag, IStep, IRecipeSubmit } from "../../../models/types";
import { RecipeService } from '../../../services/recipeServices';
import { ImageService } from '../../../services/imageService';
import useStore from '../../../store/store';

export const AddAndEditForm = forwardRef((_, ref) => {
    const { id } = useParams<{ id: string }>();
    const { userId, setNotification } = useStore();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    const imageService = new ImageService();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    const recipeService = new RecipeService();
    const navigate = useNavigate();
    const [, setIsUpdating] = useState<boolean>(false);
    
    // State for form data
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

    // Validation state for fields
    const [isNameValid, setIsNameValid] = useState<boolean>(true);
    const [isDescriptionValid, setIsDescriptionValid] = useState<boolean>(true);
    const [isImageValid, setIsImageValid] = useState<boolean>(true);
    const [areTagsValid, setAreTagsValid] = useState<boolean>(true);
    const [areIngredientsValid, setAreIngredientsValid] = useState<boolean>(true);
    const [areStepsValid, setAreStepsValid] = useState<boolean>(true);

    useEffect(() => {
        if (id) {
            setIsUpdating(true);
            const fetchRecipe = async () => {
                try {
                    const recipe = await recipeService.fetchRecipeById(id);
                    if (recipe.authorId !== Number(userId)) {
                        navigate('/allRecipesPage');
                        return;
                    }
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
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [id,  userId]);

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
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [imageUrl]);

    const validateForm = () => {
        let isValid = true;
    
        const isNameValid = name.trim() !== '';
        setIsNameValid(isNameValid);
        if (!isNameValid) isValid = false;
    
        const isImageValid = !!image;
        setIsImageValid(isImageValid);
        if (!isImageValid) isValid = false;
    
        const isDescriptionValid = description.trim() !== '';
        setIsDescriptionValid(isDescriptionValid);
        if (!isDescriptionValid) isValid = false;
    
        const areTagsValid = tags.length > 0;
        setAreTagsValid(areTagsValid);
        if (!areTagsValid) isValid = false;
    
        const areIngredientsValid = ingredients.length > 0 &&
            ingredients.every(ingredient => (ingredient.title.trim() !== '' || ingredient.title.length > 50) 
            || (ingredient.description.trim() !== '' && ingredient.description.length > 250 ));
        setAreIngredientsValid(areIngredientsValid);
        if (!areIngredientsValid) isValid = false;
    
        const areStepsValid = steps.length > 0 &&
            steps.every(step => step.stepDescription.trim() !== '' || step.stepDescription.length > 250);
        setAreStepsValid(areStepsValid);
        if (!areStepsValid) isValid = false;
    
        if (!isValid) {
            setNotification("Заполните все поля,", "error");
        }
    
        return isValid;
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
            ...(id && { id }),
            name,
            userId,
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
            setNotification((id ? 'Рецепт успешно обновлен' : 'Рецепт успешно добавлен'), "success");
            navigate('/allRecipesPage');
        } catch (error) {
            setNotification(`${error}`, "error");
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
                isNameValid={isNameValid}
                isDescriptionValid={isDescriptionValid}
                isImageValid={isImageValid}
                areTagsValid={areTagsValid}
            />
            <div className={styles.recipeListsInfo}>
                <IngredientsSection
                    ingredients={ingredients}
                    setIngredients={setIngredients}
                    areIngredientsValid={areIngredientsValid}
                />
                <StepsSection
                    steps={steps}
                    setSteps={setSteps}
                    areStepsValid={areStepsValid}
                />
            </div>
        </form>
    );
});
