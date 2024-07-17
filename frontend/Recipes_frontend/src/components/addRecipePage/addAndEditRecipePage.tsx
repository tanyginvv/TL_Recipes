import { useRef } from "react";
import { useParams } from "react-router-dom";
import { RecipeForm } from "./recipeForm/recipeForm";
import styles from "./addAndEditRecipePage.module.css";
import { AddAndEditRecipeHeader } from "./addAndEditRecipeHeader/addAndEditRecipeHeader";

interface AddRecipeFormHandle {
    submitForm: () => void;
}

export const AddAndEditRecipePage = () => {
    const formRef = useRef<AddRecipeFormHandle>(null);
    const { id } = useParams<{ id?: string }>();
    const isEditing = !!id;

    const handlePublish = () => {
        if (formRef.current) {
            formRef.current.submitForm();
        }
    };

    return (
        <div className={styles.addRecipeContainer}>
            <AddAndEditRecipeHeader onPublish={handlePublish} isEditing={isEditing} />
            <RecipeForm ref={formRef} />
        </div>
    );
};
