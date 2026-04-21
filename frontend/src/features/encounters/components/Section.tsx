import React from "react"

export function Section({
  icon,
  title,
  count,
  children,
}: {
  icon: React.ReactNode
  title: string
  count: number
  children: React.ReactNode
}) {
  return (
    <div className="group">
      <div className="flex items-center gap-2.5 mb-4">
        <span className="flex items-center justify-center w-7 h-7 rounded-lg bg-slate-100 text-slate-500 group-hover:bg-blue-50 group-hover:text-blue-600 transition-colors duration-200">
          {icon}
        </span>

        <h3
          className="text-sm font-semibold text-slate-800"
          style={{ letterSpacing: "0.06em", textTransform: "uppercase" }}
        >
          {title}
        </h3>

        {count > 0 && (
          <span className="ml-auto text-xs font-medium text-slate-400 bg-slate-100 px-2 py-0.5 rounded-full">
            {count}
          </span>
        )}
      </div>

      {children}
    </div>
  )
}