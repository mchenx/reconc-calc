import { format } from "date-fns"

export const ReportTableColumns = [
    {
        header: 'A/C Code',
        accessorKey: 'code',
    },
    {
        header: 'Description',
        accessorKey: 'description'
    },
    {
        header: 'Supplier code',
        accessorKey: 'supplierCode'
    },
    {
        header: 'Supplier name',
        accessorKey: 'supplierName'
    },
    {
        header: 'contract no.',
        accessorKey: 'contractNo'
    },
    {
        header: 'Due date',
        accessorKey: 'dueDate',
        cell: ({ getValue }: any) => format(getValue(), 'MM/dd/yyyy')
    },
    {
        header: 'AMOUNT IN CTRM (USD)',
        accessorKey: 'amountInCTRM'
    },
    {
        header: 'Amount In JDE',
        accessorKey: 'amountInJDE',
    },
    {
        header: 'PD Rate',
        accessorKey: 'pdRate'
    },
    {
        header: 'Expected Loss',
        accessorKey: 'expectedLoss',
        cell: ({ getValue }: any) => getValue()?.toFixed(2)
    },
    {
        header: 'SF Acct Title',
        accessorKey: 'sfAccountTitle'
    },
    // {
    //     header: 'Insurance',
    //     accessorKey: 'hasInsurance'
    // },
    {
        header: 'Insurance Rate',
        accessorKey: 'insuranceRate'
    },
    {
        header: 'Insurance Limit USD',
        accessorKey: 'insuranceLimit'
    },
    {
        header: 'Net Exposure',
        accessorKey: 'netExposure'
    }
]