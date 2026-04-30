type DoctorsHeaderProps = {
  title?: string
  subtitle?: string
}

export function DoctorsHeader({
  title = "Doctors",
  subtitle = "Search medical specialists",
}: DoctorsHeaderProps) {
  return (
    <div>
      <h1 className="text-3xl font-semibold">{title}</h1>
      <p className="text-sm text-muted-foreground">{subtitle}</p>
    </div>
  )
}