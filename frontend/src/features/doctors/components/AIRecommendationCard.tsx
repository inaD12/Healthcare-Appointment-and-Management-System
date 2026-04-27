import { Card, CardContent, CardTitle, CardDescription, CardHeader } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"

type Props = {
  symptoms: string
  setSymptoms: (v: string) => void
  onRecommend: () => void
  aiError: string
  recommendedSpecialities: string[]
}

export function AIRecommendationCard({
  symptoms,
  setSymptoms,
  onRecommend,
  aiError,
  recommendedSpecialities,
}: Props) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>AI Recommendation</CardTitle>
        <CardDescription>
          Describe symptoms to suggest specialities
        </CardDescription>
      </CardHeader>

      <CardContent className="flex gap-3">
        <Input
          value={symptoms}
          onChange={(e) => setSymptoms(e.target.value)}
          placeholder="Describe symptoms..."
        />
        <Button onClick={onRecommend}>Ask AI</Button>
      </CardContent>

      {aiError && (
        <p className="px-6 pb-4 text-sm text-red-500">
          {aiError}
        </p>
      )}

      {recommendedSpecialities.length > 0 && (
        <p className="px-6 pb-4 text-sm text-muted-foreground">
          Recommended: {recommendedSpecialities.join(", ")}
        </p>
      )}
    </Card>
  )
}