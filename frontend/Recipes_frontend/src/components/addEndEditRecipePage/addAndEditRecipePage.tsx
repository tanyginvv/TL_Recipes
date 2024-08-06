import { useRef, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { AddAndEditForm } from "./addAndEditForm/addAndEditForm";
import styles from "./addAndEditRecipePage.module.css";
import { AddAndEditHeader } from "./addAndEditHeader/addAndEditHeader";
import useStore from "../../store/store";

interface AddRecipeFormHandle {
    submitForm: () => void;
}

export const AddAndEditRecipePage = () => {
    const navigate = useNavigate();
    const formRef = useRef<AddRecipeFormHandle>(null);
    const { id } = useParams<{ id?: string }>();
    const { userId } = useStore();
    const isEditing = !!id;

    const handlePublish = () => {
        if (formRef.current) {
            formRef.current.submitForm();
        }
    };

    useEffect(()=> {
        if(userId === null){
            navigate("*")
        }
    })

    return (
        <div className={styles.addRecipeContainer}>
            <AddAndEditHeader onPublish={handlePublish} isEditing={isEditing} />
            <AddAndEditForm ref={formRef} />
        </div>
    );
};