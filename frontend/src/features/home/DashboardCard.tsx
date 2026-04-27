"use client"

import Link from "next/link"
import { Card, CardHeader, CardTitle, CardContent } from "../../components/ui/card"

export function DashboardCard({
  icon,
  title,
  value,
  description,
  href,
}: {
  icon: React.ReactNode
  title: string
  value: string | number
  description: string
  href?: string
}) {
  const content = (
    <Card className="hover:bg-muted/40 transition cursor-pointer">
      <CardHeader className="flex flex-row items-center justify-between pb-2">
        <CardTitle className="text-sm font-medium">
          {title}
        </CardTitle>
        {icon}
      </CardHeader>

      <CardContent>
        <div className="text-2xl font-bold">{value}</div>
        <p className="text-xs text-muted-foreground mt-1">
          {description}
        </p>
      </CardContent>
    </Card>
  )

  if (href) {
    return <Link href={href}>{content}</Link>
  }

  return content
}