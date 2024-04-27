import { IReport } from '../store/TradingSlice';
import './ReportTable.css'
import { useAppSelector } from '..';
import { ReportTable } from './ReportTable';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export function ReportTableContainer() {

    const { tradings, reports } = useAppSelector((state) => state.data)

    const reportsDict = reports.reduce((dict, item) => {
        dict[item.tradingId] = item

        return dict
    },{} as {[key:number]:IReport})

    const data = tradings.map((t) => {
        const report = reportsDict[t.id]
       
        // Note: both t and report has 'id'
        // so t.id overwrites report.id (report.id are all zeros for now)
        return { ...report, ...t }
    })

    return (
        <>
        <ReportTable data = {data} />
        <div>
           <ToastContainer theme='dark' autoClose={9000} />
        </div>
        </>
    )
}