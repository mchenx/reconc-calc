import { useMemo } from 'react'
import { Column, useTable } from 'react-table'
import { ReportTableColumns } from "./ReportTableColumns";
import { IReport, ITrading } from '../store/TradingSlice';
import './ReportTable.css'

interface IReportTableProps {
    tradings: ITrading[];
    reports: IReport[];
}

export function ReportTable(props: IReportTableProps) {
    
    const columns = useMemo<Column[]>(() => ReportTableColumns, [])
    const { tradings, reports } = props

    const reportsDict = reports.reduce((dict, item) => {
        dict[item.tradingId] = item

        return dict
    },{} as {[key:number]:IReport})

    const data = tradings.map((t) => {
        const report = reportsDict[t.id]
       
        // Note: both t and report has 'id'
        // so t.id overwrites report.id (report.id are all zeros for now)
        return {...report, ...t,}
    })
    
    const tableInstance = useTable({
        columns,
        data
    })

    const { getTableProps, getTableBodyProps, rows, headerGroups, prepareRow } = tableInstance

    return (
        <table {...getTableProps()}>
            <thead>
                {headerGroups.map((hg => (
                    <tr {...hg.getHeaderGroupProps()}>
                        {hg.headers.map((col) => (
                            <th {...col.getHeaderProps()}>{col.render('Header')}</th>
                        ))}
                    </tr>
                )))}
            </thead>
            <tbody {...getTableBodyProps()}>
                {rows.map((row) => {
                    prepareRow(row)
                    return (
                        <tr {...row.getRowProps()}>
                            {row.cells.map((cell) => {
                                return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                            })}
                        </tr>
                    )
                })}
            </tbody>
        </table>
    )
}