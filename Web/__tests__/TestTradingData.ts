import { thunk } from 'redux-thunk'
import configureMockStore from 'redux-mock-store'
import { ITradingState } from '../src/store/TradingSlice';

const middlewares = [ thunk as any ]
export const mockStore = configureMockStore(middlewares)
const testTradingData: ITradingState = {
  tradings: [],
  reports: []
}
export const initialState = { data: { ...testTradingData }}