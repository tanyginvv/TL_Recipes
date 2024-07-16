import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import { HomePage } from './components/homePage/homePage'
import { Header } from './components/header/header'
import Footer from './components/footer/footer'
import { AllRecipesPage } from './components/allRecipesPage/allRecipesPage'
import { DetailRecipePage } from './components/detailRecipePage/detailRecipePage'

function App() {

  return (
    <>
    <Header/> 
    <div>
      <BrowserRouter>
        <Routes>
          <Route path='/detailRecipesPage/:id' element={<DetailRecipePage/>}/>
          <Route path='/allRecipesPage' element={<AllRecipesPage/>}/>
          <Route path='/homePage' element={<HomePage/>}/>
          <Route path="*" element={<Navigate to="/homePage" replace/>}/>
        </Routes>
      </BrowserRouter>
    </div>
    <Footer/>
    </>
  )
}

export default App