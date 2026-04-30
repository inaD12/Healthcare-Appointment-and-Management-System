import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"

import { DoctorSpecialitiesFilter } from "@/features/doctors/components/DoctorSpecialitiesFilter"

export function DoctorsFiltersCard({
  filters,
  setFilters,
  onSearch,
}: any) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Filters</CardTitle>
      </CardHeader>

      <CardContent>
        <div className="flex flex-wrap gap-3 items-center w-full">

          <Input
            placeholder="First name"
            className="flex-1 min-w-[140px]"
            value={filters.firstName}
            onChange={(e) =>
              setFilters((prev: any) => ({
                ...prev,
                firstName: e.target.value,
                page: 1,
              }))
            }
          />

          <Input
            placeholder="Last name"
            className="flex-1 min-w-[140px]"
            value={filters.lastName}
            onChange={(e) =>
              setFilters((prev: any) => ({
                ...prev,
                lastName: e.target.value,
                page: 1,
              }))
            }
          />

          <div className="flex-[1.5] min-w-[200px]">
            <DoctorSpecialitiesFilter
              value={filters.speciality}
              onChange={(val) =>
                setFilters((prev: any) => ({
                  ...prev,
                  speciality: val,
                  page: 1,
                }))
              }
            />
          </div>

          <div className="flex-1 min-w-[140px]">
            <Select
              value={filters.sortOrder}
              onValueChange={(value) =>
                setFilters((prev: any) => ({
                  ...prev,
                  sortOrder: value,
                }))
              }
            >
              <SelectTrigger className="w-full">
                <SelectValue placeholder="Sort" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="ASC">ASC</SelectItem>
                <SelectItem value="DESC">DESC</SelectItem>
              </SelectContent>
            </Select>
          </div>

          <Button className="shrink-0 px-6" onClick={onSearch}>
            Search
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}