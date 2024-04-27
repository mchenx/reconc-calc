import { useMemo } from 'react'
import { ReportTableColumns } from "./ReportTableColumns";
import { ITrading } from '../store/TradingSlice';
import './ReportTable.css'
import { useReactTable, flexRender, getCoreRowModel } from '@tanstack/react-table';

export function ReportTable(props: {data: ITrading[]}) {


    const columns = useMemo(() => ReportTableColumns, [])
    const { data } = props

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
                            <th key={col.index} colSpan={col.colSpan}>
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