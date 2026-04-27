import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"

type Props = {
  selectedSlot: string
  bookingLoading: boolean
  onConfirm: () => void
  rescheduleId: string | null
}

export default function BookingConfirmBar({
  selectedSlot,
  bookingLoading,
  onConfirm,
  rescheduleId,
}: Props) {
  return (
    <Card className="p-3">
      <CardContent>
        <div className="mb-2 flex justify-between items-center">
          <p>Selected Time: {new Date(selectedSlot).toLocaleString()}</p>

          <Button onClick={onConfirm} disabled={bookingLoading}>
            {bookingLoading
              ? "Booking..."
              : rescheduleId
                ? "Confirm Reschedule"
                : "Confirm Booking"}
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}