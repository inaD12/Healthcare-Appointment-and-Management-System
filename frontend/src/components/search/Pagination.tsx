import { Button } from "@/components/ui/button"

interface SearchPaginationProps {
  page?: number
  totalCount?: number
  pageSize?: number
  onPageChange: (page: number) => void
}

export function SearchPagination({
  page = 1,
  totalCount = 0,
  pageSize = 10,
  onPageChange,
}: SearchPaginationProps) {
  const totalPages = Math.ceil(totalCount / pageSize)

  return (
    <div className="flex justify-center items-center gap-4">

      <Button
        variant="outline"
        disabled={page <= 1}
        onClick={() => onPageChange(page - 1)}
      >
        Previous
      </Button>

      <span className="text-sm">
        {page} / {totalPages || 1}
      </span>

      <Button
        variant="outline"
        disabled={page >= totalPages}
        onClick={() => onPageChange(page + 1)}
      >
        Next
      </Button>

    </div>
  )
}