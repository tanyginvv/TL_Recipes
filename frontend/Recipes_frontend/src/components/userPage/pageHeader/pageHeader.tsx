import styles from "./pageHeader.module.css"
import backspace from "./../../../assets/images/backspace.svg"
import { useNavigate } from "react-router-dom"

export const PageHeader = () => {
    const navigate = useNavigate();

    const allRecipesPageHandler = () => {
        navigate("/allRecipesPage");
    }
    return (
        <div className={styles.header}>
            <button className={styles.buttonBack} onClick={allRecipesPageHandler}>
                <img src={backspace} alt="back" />
                <p>Назад</p>
            </button>
            <h1 className={styles.title}>Мой профиль</h1>
        </div>
    )
}