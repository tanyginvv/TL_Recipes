import React from 'react';
import styles from './stepsSection.module.css';
import { IStep } from "../../../../models/types";
import close from "../../../../assets/images/close.svg";

interface StepsSectionProps {
    steps: IStep[];
    setSteps: (steps: IStep[]) => void;
    areStepsValid: boolean;
}

export const StepsSection: React.FC<StepsSectionProps> = ({ steps, setSteps, areStepsValid }) => {
    const handleStepChange = (index: number, value: string) => {
        const newSteps = [...steps];
        newSteps[index] = {
            ...newSteps[index],
            stepDescription: value
        };
        setSteps(newSteps);
    };

    const handleAddStep = () => {
        setSteps([...steps, { stepNumber: steps.length + 1, stepDescription: '' }]);
    };

    const handleRemoveStep = (index: number) => {
        const newSteps = steps.filter((_, i) => i !== index).map((step, i) => ({ ...step, stepNumber: i + 1 }));
        setSteps(newSteps);
    };

    return (
        <div className={styles.stepsInfo}>
            {steps.map((step, index) => (
                <span key={index} className={styles.stepItem}>
                    <span className={styles.closeBtnStep} onClick={() => handleRemoveStep(index)}>
                        <p className={styles.stepNumber}>Шаг {index + 1}</p>
                        <img className={styles.closeIcon} onClick={() => handleRemoveStep(index)} src={close} alt="Remove step" />
                    </span>
                    <textarea
                        maxLength={250}
                        placeholder="Описание шага"
                        value={step.stepDescription}
                        onChange={(e) => handleStepChange(index, e.target.value)}
                        className={`${styles.stepDescription} ${!areStepsValid && !step.stepDescription.trim() ? styles.invalid : ''}`}
                    />
                </span>
            ))}
            <button type="button" onClick={handleAddStep} className={styles.addStepButton}>+   Добавить шаг</button>
        </div>
    );
};
