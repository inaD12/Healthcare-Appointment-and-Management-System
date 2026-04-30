export function EmptyState({ label }: { label: string }) {
  return (
    <p className="text-sm text-slate-400 italic py-2 pl-1">
      No {label} recorded
    </p>
  )
}