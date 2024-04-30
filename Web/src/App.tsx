import { Routes, Route } from 'react-router-dom'
import './App.css';
import { ReportPage } from './components/ReportPage';
import { useDispatch, useSelector } from 'react-redux';
import { AppDispatch, RootState } from '.';

export const useAppDispatch = useDispatch.withTypes<AppDispatch>()
export const useAppSelector = useSelector.withTypes<RootState>()

function App() {
  return (
    <Routes>
      <Route path='/' element={<ReportPage />} />
      {/* <Route path='/tradings' element={<TradingPage />} />
      <Route path='/counterparties' element={<CounterpartyPage />} />
      <Route path='/insurance' element={<InsurancePage />} /> */}
    </Routes>
  );
}

export default App;
