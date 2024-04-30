import { Suspense, useEffect } from 'react';
import { loadTradings, getReports, clearReports } from '../store/TradingSlice';
import { useAppDispatch } from '../App';
import { ReportTableContainer } from './ReportTableContainer';


export function ReportPage () {

    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(loadTradings());
    }, []);

    return (
    <>
        <Suspense fallback={"loading..."}>
            <ReportTableContainer />
        </Suspense>
        <button onClick={() => dispatch(getReports())}>Calculate</button>
        <button onClick={() => dispatch(clearReports())}>Clear</button>
    </>);
}