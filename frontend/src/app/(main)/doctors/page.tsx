import { doctorService } from "@/features/doctors/services/doctorService"
import DoctorsClient from "./DoctorsClient"

export default async function Page() {
  const res = await doctorService.getAllDoctors({
    firstName: "",
    lastName: "",
    speciality: "",
    sortOrder: "ASC",
    sortPropertyName: "FirstName",
    page: 1,
    pageSize: 10,
  })

  const data = res.data.data

  return (
    <DoctorsClient
      initialDoctors={data.items}
      initialTotalPages={Math.ceil(data.totalCount / data.pageSize)}
    />
  )
}