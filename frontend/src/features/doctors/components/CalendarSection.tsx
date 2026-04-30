import { Card, CardContent} from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { CalendarGrid } from "@/components/calendar/CalendarGrid"

type Props = {
  currentMonth: Date
  setCurrentMonth: (d: Date) => void
  weekdays: string[]
  calendarDays: any[]
  selectedDate: Date | null
  setSelectedDate: (d: Date | null) => void
  rescheduleId: string | null
}

export default function CalendarSection({
  currentMonth,
  setCurrentMonth,
  weekdays,
  calendarDays,
  selectedDate,
  setSelectedDate,
  rescheduleId,
}: Props) {
  return (
    <Card className="p-4">
      <CardContent>
        <h2 className="text-xl font-semibold mb-4">
          {rescheduleId ? "Reschedule Appointment" : "Book an Appointment"}
        </h2>

        <div className="flex justify-between items-center mb-2">
          <Button onClick={() =>
            setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1))
          }>
            Previous
          </Button>

          <h3 className="text-lg font-medium">
            {currentMonth.toLocaleString("default", { month: "long", year: "numeric" })}
          </h3>

          <Button onClick={() =>
            setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1))
          }>
            Next
          </Button>
        </div>

        <div className="grid grid-cols-7 text-center font-medium mb-2">
          {weekdays.map(d => <div key={d}>{d}</div>)}
        </div>

        <CalendarGrid
          days={calendarDays}
          selectedDate={selectedDate}
          onSelect={setSelectedDate}
        />
      </CardContent>
    </Card>
  )
}