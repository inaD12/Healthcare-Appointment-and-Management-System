import { Card, CardContent } from "@/components/ui/card"
import { TimeSlots} from "@/components/calendar/TimeSlots"
import { TimeSlotItem } from "@/features/appointments/hooks/useDoctorCalendar"

type Props = {
  selectedDate: Date
  timeSlots: TimeSlotItem[]
  selectedSlot: string | null
  setSelectedSlot: (s: string | null) => void
}

export default function TimeSlotSection({
  selectedDate,
  timeSlots,
  selectedSlot,
  setSelectedSlot,
}: Props) {
  return (
    <Card className="p-4">
      <CardContent>
        <h3 className="font-medium mb-2">
          Available Slots for {selectedDate.toDateString()}
        </h3>

        <TimeSlots
            slots={timeSlots}
            selectedSlot={selectedSlot}
            onSelect={setSelectedSlot}
            />
      </CardContent>
    </Card>
  )
}