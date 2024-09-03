import { BrowserRouter, Routes, Route } from 'react-router-dom'
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
import { Notification } from './components/notification/notification'
import { NotFoundPage } from './components/notFoundPage/notFoundPage'
function App() {
  const {
    isRegistrationWindowOpen,
    isAuthorizationWindowOpen,
    isAuthOrRegistrWindowOpen,
    notification,
    notificationText,
    notificationStatus,
    setCloseNotification
  } = useStore();

  return (
    <>
    <div className='container'>
      <BrowserRouter>
        <Header/> 
          <Routes>
            <Route path='/userPage' element={<UserPage/>}/>
            <Route path='/addAndEditRecipePage/:id?' element={<AddAndEditRecipePage/>}/>
            <Route path='/detailRecipesPage/:id' element={<DetailRecipePage/>}/>
            <Route path='/allRecipesPage' element={<AllRecipesPage/>}/>
            <Route path='/homePage' element={<HomePage/>}/>
            <Route path="*" element={<NotFoundPage/>}/>
            <Route path='/favourites' element={<FavouritesPage/>}/>
          </Routes>
        <Footer/>
          {isRegistrationWindowOpen && <RegistrationWindow />}
          {isAuthorizationWindowOpen && <AuthorizationWindow />}
          {isAuthOrRegistrWindowOpen && <AuthOrRegistrWindow />}
          {notification && (
          <Notification 
            message={notificationText} 
            status={notificationStatus}
            onClose={() => setCloseNotification()}
          />
        )}
      </BrowserRouter>
    </div>
    </>
  )
}

export default App