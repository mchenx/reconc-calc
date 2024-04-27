import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { toast } from 'react-toastify';
import Configurations from '../Configurations';
import axios from 'axios';

export interface ITrading {
    id: number,
    code: string,
    supplierCode: number,
    supplierName: string,
    contractNo: string,
    dueDate: Date,
    amountInCTRM?: number,
    amountInJDE: number,
    pdRate: number,
    expectedLoss: number,
    acctTitle: string
    sfAccountTitle: string,   // same as counterparty name in JDE
    insuranceRate: number,
    insuranceLimit: number,
    netExposure: number,
    // isLoading?: boolean,
    // isError?: boolean
}

export interface ITradingState {
    // isLoading: boolean;
    // isError: boolean;
    tradings: ITrading[];
    reports: IReport[];

}

export interface IReport {
    reportId: number,
    tradingId: number,
    hasInsurance: boolean,
    pdRate: number,
    expectedLoss: number,
    insuranceRate?: number,
    insuranceLimit: number,
    netExpose: number
}

export const initialState: ITradingState  = {
    // isLoading: false,
    // isError: false,
    tradings: [],
    reports: []
}

const loadTradings = createAsyncThunk<{ tradings: ITrading[] }, void>(
    'Tradings/loadTradings',
    async (data: any, { getState, rejectWithValue }) => {
        try {
            const url = `${Configurations.BACKEND_API_ENDPOINT}/${Configurations.API_PATH_TRADINGS}`;
            const response = await axios.get<ITrading[]>(url);

            return { tradings: response.data };
        } catch (error: any) {
            return rejectWithValue(error.response?.data);
        }
    }
);

const getReports = createAsyncThunk<{ reports: IReport[] }, void>(
    'Tradings/getReports',
    async (data: any, { getState, rejectWithValue }) => {
        try {
            const url = `${Configurations.BACKEND_API_ENDPOINT}/${Configurations.API_PATH_REPORTS}`;
            const response = await axios.get<IReport[]>(url);

            return { reports: response.data };
        } catch (error: any) {
            return rejectWithValue(error.response?.data);
        }
    }
);

const TradingSlice = createSlice({
    name: 'Tradings',
    initialState: initialState,
    reducers: {
        clearReports: (state) => {
            state.reports = []
        }        
    },
    extraReducers: (builder) => {
        builder
            // .addCase(loadTradings.pending, (state) => {
            //     state.isLoading = true;
            //     state.isError = false;
            // })
            .addCase(loadTradings.fulfilled, (state, action) => {
                // state.isLoading = false;
                // state.isError = false;
                state.tradings = action.payload?.tradings || [];
            })
            .addCase(loadTradings.rejected, (state) => {
            //     state.isLoading = false;
            //     state.isError = true;
                toast.error("Unable to load tradings, something wrong with the server !")
            })
            // .addCase(getReports.pending, (state) => {
            //     state.isLoading = true;
            //     state.isError = false;
            // })
            .addCase(getReports.fulfilled, (state, action) => {
                // state.isLoading = false;
                // state.isError = false;
                state.reports = action.payload?.reports || [];
                toast.success("Report generated successfully.", { autoClose: 2000 })
            })
            .addCase(getReports.rejected, (state) => {
            //     state.isLoading = false;
            //     state.isError = true;
                toast.error("Cannot generate report, something wrong on server side !")
            })
    }
});

export { loadTradings, getReports };
export const { clearReports } = TradingSlice.actions;
export default TradingSlice.reducer;