import { thunk } from 'redux-thunk'
import configureMockStore from 'redux-mock-store'
import { IReport, ITrading, ITradingState } from '../src/store/TradingSlice';

const middlewares = [ thunk as any ]
export const mockStore = configureMockStore(middlewares)
const testTradingData: ITradingState = {
  tradings: [],
  reports: []
}
export const initialState = { data: { ...testTradingData }}

export const testTrading1: ITrading = {
    id: 0,
    code: '',
    supplierCode: 1233,
    supplierName: 'supplier1',
    contractNo: 'c1234',
    amountInJDE: 12345.67,
    dueDate: new Date('2023-12-4'),
    pdRate: 0.31,
    sfAccountTitle: 'test title 1',
    expectedLoss: 0.0,
    insuranceLimit: 125687.62,
    insuranceRate: 0.9,
    netExposure: 0.0
}

export const testReportForTrading1: IReport = {
    reportId: 0,
    tradingId: 0,
    expectedLoss: 12456.85,
    netExpose: 45124.66,
    pdRate: 0.33,
    hasInsurance : true,
    insuranceLimit: 449688.21    
}