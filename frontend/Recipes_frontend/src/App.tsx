import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import { HomePage } from './components/homePage/homePage'
import { Header } from './components/header/header'
import { Footer } from './components/footer/footer'
function App() {

  return (
    <>
    <div className='container'>
      <BrowserRouter>
        <Header/> 
          <Routes>
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