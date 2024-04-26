import React from 'react';
import { Routes, Route } from 'react-router-dom'
import './App.css';
import { ReportPage } from './components/ReportPage';

function App() {
  return (
    <Routes>
      <Route path='/reports' element={<ReportPage />} />
      {/* <Route path='/tradings' element={<TradingPage />} />
      <Route path='/counterparties' element={<CounterpartyPage />} />
      <Route path='/insurance' element={<InsurancePage />} /> */}
    </Routes>
  );
}

export default App;
