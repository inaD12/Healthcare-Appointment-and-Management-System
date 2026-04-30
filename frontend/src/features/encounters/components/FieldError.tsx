import { ErrorIcon } from "./icons"

export function FieldError({ message }: { message?: string }) {
  if (!message) return null

  return (
    <p className="flex items-center gap-1.5 text-xs text-red-600 mt-1.5">
      <ErrorIcon />
      {message}
    </p>
  )
}