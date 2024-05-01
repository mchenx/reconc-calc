import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import reducer, { loadTradings, getReports, clearReports, initialState } from './TradingSlice';
import Configurations from '../Configurations';
import { mockStore, testReportForTrading1, testTrading1 } from '../../__tests__/TestTradingData'

const mockAxios = new MockAdapter(axios);

// Remove 'meta' field added by createAsyncThunk
function removeMetaField(actions: any[]): any[] {
    actions?.forEach(a => delete a.meta);
    return actions;
}

describe('async actions', () => {
    afterEach(() => {
        mockAxios.reset();
    });

    it('creates fulfilled action when fetching tradings has been done', () => {
        const expectedTradings = [{ id: 1, code: 'ABC', supplierCode: 123 }];
        mockAxios.onGet(`${Configurations.BACKEND_API_ENDPOINT}/${Configurations.API_PATH_TRADINGS}`).reply(200, expectedTradings);

        const expectedActions = [
            { type: loadTradings.pending.type, payload: undefined },
            { type: loadTradings.fulfilled.type, payload: { tradings: expectedTradings } },
        ];

        const store = mockStore({ tradings: initialState });

        return store.dispatch(loadTradings() as any).then(() => {
            const actualActions = removeMetaField(store.getActions());
            expect(actualActions).toEqual(expectedActions);
        });
    });

    it('creates fulfilled action when fetching reports has been done', () => {
        const expectedReports = [{ reportId: 1, tradingId: 1, hasInsurance: true }];
        mockAxios.onGet(`${Configurations.BACKEND_API_ENDPOINT}/${Configurations.API_PATH_REPORTS}`).reply(200, expectedReports);

        const expectedActions = [
            { type: getReports.pending.type, payload: undefined },
            { type: getReports.fulfilled.type, payload: { reports: expectedReports } },
        ];

        const store = mockStore({ tradings: initialState });

        return store.dispatch(getReports() as any).then(() => {
            const actualActions = removeMetaField(store.getActions());
            expect(actualActions).toEqual(expectedActions);
        });
    });

    it('creates reject action when fetching tradings encounters server error', () => {
        mockAxios.onGet(`${Configurations.BACKEND_API_ENDPOINT}/${Configurations.API_PATH_TRADINGS}`).reply(500);

        const expectedActions = [
            { type: loadTradings.pending.type },
            { type: loadTradings.rejected.type, error: { message: "Rejected" } },
        ];

        const store = mockStore({ tradings: initialState });

        return store.dispatch(loadTradings() as any).then(() => {
            const actualActions = removeMetaField(store.getActions());
            expect(actualActions).toEqual(expectedActions);
        });
    });

    it('creates reject action when fetching reports encounters server error', () => {
        mockAxios.onGet(`${Configurations.BACKEND_API_ENDPOINT}/${Configurations.API_PATH_REPORTS}`).reply(500);

        const expectedActions = [
            { type: getReports.pending.type },
            { type: getReports.rejected.type, error: { message: "Rejected" } },
        ];

        const store = mockStore({ tradings: initialState });

        return store.dispatch(getReports() as any).then(() => {
            const actualActions = removeMetaField(store.getActions());
            expect(actualActions).toEqual(expectedActions);
        });
    });
});

describe('reducer', () => {
  it('should handle loadTradings.fullfilled and generate new state', () => {
    const previousState = { tradings: [], reports: [] };
    const payload = {  tradings: [{  ...testTrading1, id: 1, code: 'ABC' }] };
    const expectedState = { tradings: [{ ...testTrading1, id: 1, code: 'ABC' }], reports: [] };
    const action = loadTradings.fulfilled(payload, 'loadTradings.fulfilled-request-id');

    expect(reducer(previousState, action)).toEqual(expectedState);
  });

  it('should handle getReports.fullfilled and generate new state', () => {
    const previousState = { tradings: [], reports: [{ ...testReportForTrading1, reportId: 0, tradingId: 1 }] };
    const payload = { reports: [{...testReportForTrading1, reportId: 123, tradingId: 1, expectedLoss: 123445.98, extExpose: 524568.38 }] };
    const expectedState = { tradings: [], reports: [{ ...testReportForTrading1, reportId: 123, tradingId: 1, expectedLoss: 123445.98, extExpose: 524568.38  }] };
    const action = getReports.fulfilled(payload, 'getReports.fulfilled-request-id');

    expect(reducer(previousState, action)).toEqual(expectedState);
  });

  it('should handle loadTradings.rejected and the state remains unchanged', () => {
      const previousState = { tradings: [{ ...testTrading1, id: 1, code: 'ABC' }], reports: [] };
      const expectedState = { ...previousState };
      const error = { cause: 'server is not responding' } as Error;

    const action = loadTradings.rejected(error, 'loadTradings.rejected-request-id')
    expect(reducer(previousState, action)).toEqual(expectedState);
  });

  it('should handle getReports.rejected and the state remains unchanged', () => {
    const previousState = { tradings: [], reports: [{ ...testReportForTrading1, reportId: 0, tradingId: 0 }] };
    const expectedState = { ...previousState };
    const error = { cause: 'timeout' } as Error;

    const action = getReports.rejected(error, 'getReports.rejected-request-id')
    expect(reducer(previousState, action)).toEqual(expectedState);
  });

  it('should handle clearReports', () => {
    const previousState = { tradings: [], reports: [{ ...testReportForTrading1, reportId: 1, tradingId: 1 }] };
    const expectedState = { tradings: [], reports: [] };
    const action = clearReports();
      
    expect(reducer(previousState, action)).toEqual(expectedState);
  });
});