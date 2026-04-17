"use client"

import { useState } from "react"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from "@/components/ui/select"

export function SearchFilters({ filters, onSearch }: any) {
  const [values, setValues] = useState<any>({})

  const updateValue = (name: string, value: any) => {
    setValues((v: any) => ({ ...v, [name]: value }))
  }

  return (
    <div className="flex flex-wrap gap-4 items-end">

      {filters.map((filter: any) => {
        if (filter.type === "text") {
          return (
            <Input
              key={filter.name}
              placeholder={filter.label}
              className="w-48"
              onChange={(e) => updateValue(filter.name, e.target.value)}
            />
          )
        }

        if (filter.type === "select") {
          return (
            <Select
                key={filter.name}
                onValueChange={(val) =>
                    updateValue(
                    filter.name,
                    val === "all" ? undefined : val
                    )
                }
            >
              <SelectTrigger className="w-48">
                <SelectValue placeholder={filter.label} />
              </SelectTrigger>

              <SelectContent>
                {filter.options?.map((o: any) => (
                  <SelectItem key={o.value} value={o.value}>
                    {o.label}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          )
        }
      })}

      <Button onClick={() => onSearch(values)}>
        Search
      </Button>

    </div>
  )
}