import { useMemo } from 'react'
import { ReportTableColumns } from "./ReportTableColumns";
import { IReport, ITrading } from '../store/TradingSlice';
import './ReportTable.css'
import { useReactTable, flexRender, getCoreRowModel } from '@tanstack/react-table';

interface IReportTableProps {
    tradings: ITrading[];
    reports: IReport[];
}

export function ReportTable(props: IReportTableProps) {
    
    const columns = useMemo(() => ReportTableColumns, [])
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
    
    const tableInstance = useReactTable({
        columns,
        data,
        getCoreRowModel: getCoreRowModel()
    })

    const { getHeaderGroups, getRowModel } = tableInstance

    return (
        <table>
            <thead>
                {getHeaderGroups().map((hg => (
                    <tr key={hg.id}>
                        {hg.headers.map((col) => (
                            <th key={hg.id} colSpan={col.colSpan}>
                                { flexRender(col.column.columnDef.header, col.getContext())}
                            </th>
                        ))}
                    </tr>
                )))}
            </thead>
            <tbody>
                {getRowModel().rows.map((row) => {
                    return (
                        <tr key={row.id}>
                            {row.getVisibleCells().map((cell) => {
                                return (
                                    <td key={cell.id}>
                                        {flexRender(cell.column.columnDef.cell, cell.getContext())}
                                    </td>)
                            })}
                        </tr>
                    )
                })}
            </tbody>
        </table>
    )
}