type TimeSlotItem = {
  start: string
  isBlocked: boolean
  isPast: boolean
}

type TimeSlotsProps = {
  slots: TimeSlotItem[]
  selectedSlot: string | null
  onSelect: (slot: string) => void
}

export function TimeSlots({ slots, selectedSlot, onSelect }: TimeSlotsProps) {
  return (
    <div className="flex flex-wrap gap-2">
            {slots.map(slot => {
              const time = new Date(slot.start).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })
              const isSelected = selectedSlot === slot.start
              let className = "px-2 py-1 rounded cursor-pointer "
              if (slot.isPast) className += "bg-gray-200 text-gray-500 cursor-not-allowed "
              else if (slot.isBlocked) className += "bg-gray-600 text-white line-through cursor-not-allowed "
              else className += "bg-gray-400 text-white "
              if (isSelected) className += "ring-2 ring-black "

              return (
                <div
                  key={slot.start}
                  className={className}
                  onClick={() => !slot.isPast && !slot.isBlocked && onSelect(slot.start)}
                >
                  {time}
                </div>
              )
            })}
          </div>
  )
}