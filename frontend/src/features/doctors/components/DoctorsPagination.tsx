import { Button } from "@/components/ui/button"

export function DoctorsPagination({
  page,
  totalPages,
  onPrev,
  onNext,
}: any) {
  return (
    <div className="flex justify-center gap-4 items-center">
      <Button variant="outline" disabled={page === 1} onClick={onPrev}>
        Previous
      </Button>

      <span className="text-sm text-muted-foreground">
        Page {page} / {totalPages}
      </span>

      <Button
        variant="outline"
        disabled={page === totalPages}
        onClick={onNext}
      >
        Next
      </Button>
    </div>
  )
}