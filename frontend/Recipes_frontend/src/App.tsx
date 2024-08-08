import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import { HomePage } from './components/homePage/homePage'
import { Header } from './components/header/header'
import { Footer } from './components/footer/footer'
import { AllRecipesPage } from './components/allRecipesPage/allRecipesPage'
import { DetailRecipePage } from './components/detailRecipePage/detailRecipePage'
import { AddAndEditRecipePage } from './components/addEndEditRecipePage/addAndEditRecipePage'
import { AuthorizationWindow } from './components/authorizationWindow/authorizationWindow'
import { AuthOrRegistrWindow } from './components/authOrRegistWindow/authOrRegistrWindow'
import { RegistrationWindow } from './components/registrationWindow/registrationWindow'
import useStore from './store/store'
import { UserPage } from './components/userPage/userPage'
import { FavouritesPage } from './components/favouritesPage/favouritesPage'
function App() {
  const {
    isRegistrationWindowOpen,
    isAuthorizationWindowOpen,
    isAuthOrRegistrWindowOpen
  } = useStore();

  return (
    <>
    <div className='container'>
      <BrowserRouter>
        <Header/> 
          <Routes>
            <Route path='/userPage/:id?' element={<UserPage/>}/>
            <Route path='/addAndEditRecipePage/:id?' element={<AddAndEditRecipePage/>}/>
            <Route path='/detailRecipesPage/:id' element={<DetailRecipePage/>}/>
            <Route path='/allRecipesPage' element={<AllRecipesPage/>}/>
            <Route path='/homePage' element={<HomePage/>}/>
            <Route path="*" element={<Navigate to="/homePage" replace/>}/>
            <Route path='/favourites/:id' element={<FavouritesPage/>}/>
          </Routes>
        <Footer/>
          {isRegistrationWindowOpen && <RegistrationWindow />}
          {isAuthorizationWindowOpen && <AuthorizationWindow />}
          {isAuthOrRegistrWindowOpen && <AuthOrRegistrWindow />}
      </BrowserRouter>
    </div>
    </>
  )
}

export default App