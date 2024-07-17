import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import { HomePage } from './components/homePage/homePage'
import { Header } from './components/header/header'
import Footer from './components/footer/footer'
import { AllRecipesPage } from './components/allRecipesPage/allRecipesPage'
import { DetailRecipePage } from './components/detailRecipePage/detailRecipePage'
import { AddAndEditRecipePage } from './components/addRecipePage/addAndEditRecipePage'
function App() {

  return (
    <>
    <div>
      <BrowserRouter>
        <Header/> 
          <Routes>
            <Route path='/detailRecipesPage/:id' element={<DetailRecipePage/>}/>
            <Route path='/addAndEditRecipePage/:id?' element={<AddAndEditRecipePage/>}/>
            <Route path='/allRecipesPage' element={<AllRecipesPage/>}/>
            <Route path='/homePage' element={<HomePage/>}/>
            <Route path="*" element={<Navigate to="/homePage" replace/>}/>
          </Routes>
        <Footer/>
      </BrowserRouter>
    </div>
    </>
  )
}

export default App