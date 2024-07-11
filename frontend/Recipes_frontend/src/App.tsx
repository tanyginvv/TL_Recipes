import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import { HomePage } from './components/homePage/homePage'
import { Header } from './components/header/header'
import Footer from './components/footer/footer'

function App() {

  return (
    <>
    <Header/>
    <div>
      <BrowserRouter>
        <Routes>
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