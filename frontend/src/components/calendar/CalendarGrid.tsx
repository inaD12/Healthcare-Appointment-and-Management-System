type StatusType = "past" | "empty" | "fullyBooked" | "partiallyBooked"

type CalendarDay = {
  date: Date
  isCurrentMonth: boolean
  status: StatusType
}

type CalendarGridProps = {
  days: CalendarDay[]
  selectedDate: Date | null
  onSelect: (date: Date) => void
}

export function CalendarGrid({ days, selectedDate, onSelect }: CalendarGridProps) {
  return (
    <div className="grid grid-cols-7 gap-2">
        {days.map((day) => {
          const isSelected = selectedDate?.toDateString() === day.date.toDateString()
          let className = "text-center p-2 rounded cursor-pointer "
          if (!day.isCurrentMonth) className += "bg-gray-100 text-gray-400 "
          else if (day.status === "past") className += "bg-gray-300 text-gray-500 cursor-not-allowed "
          else if (day.status === "empty") className += "bg-green-400 text-white "
          else if (day.status === "partiallyBooked") className += "bg-yellow-400 text-white "
          else if (day.status === "fullyBooked") className += "bg-red-500 text-white "

          if (isSelected) className += "ring-2 ring-black "

          return (
            <div
              key={day.date.toISOString()}
              className={className}
              onClick={() => day.status !== "past" && onSelect(day.date)}
            >
              {day.date.getDate()}
            </div>
          )
        })}
      </div>
  )
}