import React, {useEffect} from 'react';
import { loadTradings, getReports, clearReports } from '../store/TradingSlice';
import { useAppDispatch, useAppSelector } from '..';
import { ReportTable } from './ReportTable';


export function ReportPage () {

    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(loadTradings());
    }, []);

    const { tradings, reports } = useAppSelector((state) => state.data)

    return (
    <>
        <ReportTable tradings={tradings} reports={reports} />
        <button onClick={() => dispatch(getReports())}>Calculate</button>
        <button onClick={() => dispatch(clearReports())}>Clear</button>
    </>);
}