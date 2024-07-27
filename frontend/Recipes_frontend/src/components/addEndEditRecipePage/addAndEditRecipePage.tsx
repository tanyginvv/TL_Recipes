import { useRef } from "react";
import { useParams } from "react-router-dom";
import { AddAndEditForm } from "./addAndEditForm/addAndEditForm";
import styles from "./addAndEditRecipePage.module.css";
import { AddAndEditHeader } from "./addAndEditHeader/addAndEditHeader";

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
            <AddAndEditHeader onPublish={handlePublish} isEditing={isEditing} />
            <AddAndEditForm ref={formRef} />
        </div>
    );
};