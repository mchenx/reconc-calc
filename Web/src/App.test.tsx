import { render, screen } from '@testing-library/react';
import App from './App';
import { thunk } from 'redux-thunk'
import configureMockStore from 'redux-mock-store'
import { Provider } from 'react-redux';
import { ITradingState } from './store/TradingSlice';
import { BrowserRouter } from 'react-router-dom';
import { ReportTableColumns } from './components/ReportTableColumns';

const middlewares = [ thunk as any ]
const mockStore = configureMockStore(middlewares)
const testTradingData: ITradingState = {
  tradings: [],
  reports: []
}
const initialiState = { data: { ...testTradingData }}

test('renders trading table', () => {

  const testStore = mockStore(initialiState)

  render(
    <BrowserRouter>
      <Provider store={testStore}>
        <App />
      </Provider>
    </BrowserRouter>);

  const tableElement = screen.getByRole(/table/i)
  expect(tableElement).toBeInTheDocument()
});

test('renders trading table columns', () => {
  
  const testStore = mockStore(initialiState)

  render(
    <BrowserRouter>
      <Provider store={testStore}>
        <App />
      </Provider>
    </BrowserRouter>);

  const columnHeaderElements = screen.getAllByRole(/columnheader/i)
  expect(columnHeaderElements.length).toBe(ReportTableColumns.length)

  ReportTableColumns.forEach(column => {
    const columnHeaderElement = screen.getByText(column.header)
    expect(columnHeaderElement).toBeInTheDocument()
  })
});


test('renders empty trading table (no data rows)', () => {

  const testStore = mockStore(initialiState)

  render(
    <BrowserRouter>
      <Provider store={testStore}>
        <App />
      </Provider>
    </BrowserRouter>);

  const cellElements = screen.queryAllByRole(/cell/i)
  expect(cellElements.length).toBe(0)
});