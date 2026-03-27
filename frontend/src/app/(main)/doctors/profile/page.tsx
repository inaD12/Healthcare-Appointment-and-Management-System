"use client"

import { useEffect, useState } from "react"
import {
  getMyDoctorInfo,
  updateDoctorInfo,
  addSpeciality,
  removeSpeciality,
  addWorkDaySchedule,
  removeWorkDaySchedule,
  addExtraAvailability,
  deleteExtraAvailability,
  addUnavailability,
  deleteUnavailability
} from "@/features/doctors/services/doctorService"
import { DoctorQueryViewModel, WorkDayDto, DoctorAvailabilityExceptionDto } from "@/features/doctors/types/doctors"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"

import {
  Card,
  CardHeader,
  CardTitle,
  CardContent,
  CardFooter
} from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Badge } from "@/components/ui/badge"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"

export default function DoctorProfilePage() {
  useAuthGuard()

  const [doctor, setDoctor] = useState<DoctorQueryViewModel | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const [editingBio, setEditingBio] = useState(false)
  const [bioInput, setBioInput] = useState("")
  const [newSpeciality, setNewSpeciality] = useState("")
  const [newWorkDay, setNewWorkDay] = useState<WorkDayDto>({ dayOfWeek: 0, workTimes: [{ start: "", end: "" }] })
  const [newAvailability, setNewAvailability] = useState<DoctorAvailabilityExceptionDto>({
    start: "",
    end: "",
    reason: "",
    type: 1
  })

  useEffect(() => {
    const fetchDoctor = async () => {
      try {
        const res = await getMyDoctorInfo()
        setDoctor(res.data.data)
        setBioInput(res.data.data.bio)
      } catch (err) {
        console.error(err)
        setError("Failed to fetch doctor info")
      } finally {
        setLoading(false)
      }
    }
    fetchDoctor()
  }, [])

  if (loading) return <p>Loading...</p>
  if (error) return <p className="text-red-600">{error}</p>
  if (!doctor) return <p>No doctor info found</p>

  const handleSaveBio = async () => {
    try {
      await updateDoctorInfo({ bio: bioInput, timeZoneId: doctor.timeZoneId })
      setDoctor({ ...doctor, bio: bioInput })
      setEditingBio(false)
    } catch (err) {
      console.error(err)
    }
  }

  const handleAddSpeciality = async () => {
    if (!newSpeciality) return
    try {
      await addSpeciality({ speciality: newSpeciality })
      setDoctor({ ...doctor, specialities: [...doctor.specialities, newSpeciality] })
      setNewSpeciality("")
    } catch (err) {
      console.error(err)
    }
  }

  const handleRemoveSpeciality = async (speciality: string) => {
    try {
      await removeSpeciality({ speciality })
      setDoctor({ ...doctor, specialities: doctor.specialities.filter(s => s !== speciality) })
    } catch (err) {
      console.error(err)
    }
  }

  const handleAddWorkDay = async () => {
    try {
      await addWorkDaySchedule(newWorkDay)
      setDoctor({ ...doctor, workDays: [...doctor.workDays, newWorkDay] })
      setNewWorkDay({ dayOfWeek: 0, workTimes: [{ start: "", end: "" }] })
    } catch (err) {
      console.error(err)
    }
  }

  const handleRemoveWorkDay = async (dayOfWeek: number) => {
    try {
      await removeWorkDaySchedule({ dayOfWeek })
      setDoctor({ ...doctor, workDays: doctor.workDays.filter(w => w.dayOfWeek !== dayOfWeek) })
    } catch (err) {
      console.error(err)
    }
  }

  const handleAddAvailability = async () => {
    try {
      if (newAvailability.type === 1) await addExtraAvailability(newAvailability)
      else await addUnavailability(newAvailability)
      setDoctor({ ...doctor, availabilityExceptions: [...doctor.availabilityExceptions, newAvailability] })
      setNewAvailability({ start: "", end: "", reason: "", type: 1 })
    } catch (err) {
      console.error(err)
    }
  }

  const handleRemoveAvailability = async (availability: DoctorAvailabilityExceptionDto) => {
    try {
      if (availability.type === 1) await deleteExtraAvailability({ start: availability.start })
      else await deleteUnavailability({ start: availability.start })
      setDoctor({
        ...doctor,
        availabilityExceptions: doctor.availabilityExceptions.filter(a => a.start !== availability.start)
      })
    } catch (err) {
      console.error(err)
    }
  }

  return (
    <div className="space-y-8 max-w-6xl mx-auto p-6">

      <Card>
        <CardHeader>
          <CardTitle>Doctor Information</CardTitle>
        </CardHeader>
        <CardContent className="space-y-2">
          <p><strong>Name:</strong> {doctor.firstName} {doctor.lastName}</p>
          <p><strong>Time Zone:</strong> {doctor.timeZoneId}</p>
          <p><strong>Rating:</strong> {doctor.averageRating?.toFixed(1)} ({doctor.ratingsCount} ratings)</p>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Bio</CardTitle>
        </CardHeader>
        <CardContent>
          {!editingBio ? (
            <div className="flex justify-between items-start">
              <p>{doctor.bio}</p>
              <Button variant="outline" size="sm" onClick={() => setEditingBio(true)}>Edit</Button>
            </div>
          ) : (
            <div className="space-y-2">
              <Textarea value={bioInput} onChange={e => setBioInput(e.target.value)} />
              <div className="flex gap-2">
                <Button onClick={handleSaveBio}>Save</Button>
                <Button variant="secondary" onClick={() => { setEditingBio(false); setBioInput(doctor.bio) }}>Cancel</Button>
              </div>
            </div>
          )}
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Specialities</CardTitle>
        </CardHeader>
        <CardContent className="space-y-2">
          <div className="flex flex-wrap gap-2">
            {doctor.specialities.map(s => (
              <Badge key={s} variant="outline" className="flex items-center gap-1">
                {s} <Button size="icon" variant="ghost" onClick={() => handleRemoveSpeciality(s)}>×</Button>
              </Badge>
            ))}
          </div>
          <div className="flex gap-2 mt-2">
            <Input placeholder="New speciality" value={newSpeciality} onChange={e => setNewSpeciality(e.target.value)} />
            <Button onClick={handleAddSpeciality}>Add</Button>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Work Days & Hours</CardTitle>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Day</TableHead>
                <TableHead>Times</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {doctor.workDays.map(day => (
                <TableRow key={day.dayOfWeek}>
                  <TableCell>{["Sun","Mon","Tue","Wed","Thu","Fri","Sat"][day.dayOfWeek]}</TableCell>
                  <TableCell>{day.workTimes.map(wt => `${wt.start}-${wt.end}`).join(", ")}</TableCell>
                  <TableCell><Button size="sm" variant="destructive" onClick={() => handleRemoveWorkDay(day.dayOfWeek)}>Remove</Button></TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          <div className="flex gap-2 mt-2">
            <Select onValueChange={v => setNewWorkDay({...newWorkDay, dayOfWeek: Number(v)})}>
              <SelectTrigger className="w-[150px]"><SelectValue placeholder="Day" /></SelectTrigger>
              <SelectContent>
                {["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"].map((d,i)=>(
                  <SelectItem key={i} value={i.toString()}>{d}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            <Input type="time" value={newWorkDay.workTimes[0].start} onChange={e => setNewWorkDay({...newWorkDay, workTimes:[{...newWorkDay.workTimes[0], start:e.target.value}]})} />
            <Input type="time" value={newWorkDay.workTimes[0].end} onChange={e => setNewWorkDay({...newWorkDay, workTimes:[{...newWorkDay.workTimes[0], end:e.target.value}]})} />
            <Button onClick={handleAddWorkDay}>Add</Button>
          </div>
        </CardContent>
      </Card>

        <Card>
        <CardHeader>
            <CardTitle>Availability Exceptions</CardTitle>
        </CardHeader>
        <CardContent>
            <Table>
            <TableHeader>
                <TableRow>
                <TableHead>Type</TableHead>
                <TableHead>Start</TableHead>
                <TableHead>End</TableHead>
                <TableHead>Reason</TableHead>
                <TableHead>Actions</TableHead>
                </TableRow>
            </TableHeader>
            <TableBody>
                {doctor.availabilityExceptions.map(a => (
                <TableRow key={a.start}>
                    <TableCell>{a.type === 1 ? "Extra Availability" : "Unavailability"}</TableCell>
                    <TableCell>{a.start}</TableCell>
                    <TableCell>{a.end}</TableCell>
                    <TableCell>{a.reason}</TableCell>
                    <TableCell>
                    <Button size="sm" variant="destructive" onClick={() => handleRemoveAvailability(a)}>Remove</Button>
                    </TableCell>
                </TableRow>
                ))}
            </TableBody>
            </Table>

            <div className="flex flex-wrap gap-2 mt-4 items-end">
            <Select
                value={newAvailability.type.toString()}
                onValueChange={v => setNewAvailability({ ...newAvailability, type: Number(v) })}
            >
                <SelectTrigger className="w-[180px]">
                <SelectValue placeholder="Type" />
                </SelectTrigger>
                <SelectContent>
                <SelectItem value="1">Extra Availability</SelectItem>
                <SelectItem value="2">Unavailability</SelectItem>
                </SelectContent>
            </Select>

            <Input
                type="datetime-local"
                value={newAvailability.start}
                onChange={e => setNewAvailability({ ...newAvailability, start: e.target.value })}
            />
            <Input
                type="datetime-local"
                value={newAvailability.end}
                onChange={e => setNewAvailability({ ...newAvailability, end: e.target.value })}
            />
            <Input
                placeholder="Reason"
                value={newAvailability.reason}
                onChange={e => setNewAvailability({ ...newAvailability, reason: e.target.value })}
            />
            <Button onClick={handleAddAvailability}>Add Exception</Button>
            </div>
        </CardContent>
        </Card>

    </div>
  )
}