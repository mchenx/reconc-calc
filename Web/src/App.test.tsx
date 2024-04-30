import { render, screen } from '@testing-library/react';
import App from './App';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { ReportTableColumns } from './components/ReportTableColumns';
import { mockStore, initialState } from '../__tests__/TestTradingData'

test('renders trading table', () => {

  const testStore = mockStore(initialState)

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
  
  const testStore = mockStore(initialState)

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

  const testStore = mockStore(initialState)

  render(
    <BrowserRouter>
      <Provider store={testStore}>
        <App />
      </Provider>
    </BrowserRouter>);

  const cellElements = screen.queryAllByRole(/cell/i)
  expect(cellElements.length).toBe(0)
});