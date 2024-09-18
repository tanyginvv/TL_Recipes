import styles from "./stepsList.module.css";
import { IStep } from "../../../../models/types";

interface StepsListProps {
    steps: IStep[];
}

export const StepsList: React.FC<StepsListProps> = ({ steps }) => {
    const renderSteps = (steps: IStep[]) => {
        const sortedSteps = [...steps].sort((a, b) => a.stepNumber - b.stepNumber);
        return sortedSteps.map((step, index) => (
            <li className={styles.stepListItem} key={index}>
                <h5 className={styles.stepTitle}>{"Шаг " + step.stepNumber}</h5>
                <p className={styles.stepDescription}>{step.stepDescription}</p>
            </li>
        ));
    };

    return (
        <div className={styles.infoSteps}>
            <ul className={styles.stepsList}>
                {renderSteps(steps)}
            </ul>
            <p className={styles.stepEnd}>Приятного аппетита!</p>
        </div>
    );
}