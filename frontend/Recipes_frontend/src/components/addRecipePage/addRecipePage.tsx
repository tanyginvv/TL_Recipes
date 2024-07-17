import { useRef } from "react";
import { AddRecipeForm } from "./addRecipeForm/addRecipeForm";
import { AddRecipeHeader } from "./addRecipeHeader/addRecipeHeader";
import styles from "./addRecipePage.module.css";

interface AddRecipeFormHandle {
    submitForm: () => void;
}

export const AddRecipePage = () => {
    const formRef = useRef<AddRecipeFormHandle>(null);

    const handlePublish = () => {
        if (formRef.current) {
            formRef.current.submitForm();
        }
    };

    return (
        <div className={styles.addRecipeContainer}>
            <AddRecipeHeader onPublish={handlePublish} />
            <AddRecipeForm ref={formRef} />
        </div>
    );
};
