import Example from "./example/example"
import { HomePageIntro } from "./homePageIntro/homePageIntro"
// import Search from "./search/search"
import Sorting from "./sorting/sorting"
import styles from "./homePage.module.css"

export const HomePage = () => {
    return (
        <>
            <div className={styles.mainPage}>
                <HomePageIntro/>
                <Sorting/>
                <Example/>
                {/* <Search/> */}
            </div>
        </>
    )
}