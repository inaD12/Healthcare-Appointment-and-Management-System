export interface ColumnDef<T> {
  header: string
  accessor: keyof T | ((row: T) => React.ReactNode)
}

export interface FilterField {
  name: string
  label: string
  type: "text" | "select"
  options?: { label: string; value: string }[]
}

export type PaginatedQuery = {
  page: number
  pageSize: number
}