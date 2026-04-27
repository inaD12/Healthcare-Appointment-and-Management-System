import { Card, CardContent, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"

type Props = {
  duration: 15 | 30 | 60
  setDuration: (d: 15 | 30 | 60) => void
}

export default function DurationSelector({ duration, setDuration }: Props) {
  return (
    <Card className="p-4">
      <CardTitle>Select Duration</CardTitle>
      <CardContent className="flex gap-2 mt-3">
        {[15, 30, 60].map(d => (
          <Button
            key={d}
            size="sm"
            variant={duration === d ? "default" : "outline"}
            onClick={() => setDuration(d as 15 | 30 | 60)}
          >
            {d} min
          </Button>
        ))}
      </CardContent>
    </Card>
  )
}