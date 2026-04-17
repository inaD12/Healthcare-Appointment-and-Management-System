"use client"

import { useRouter } from "next/navigation"

import { SearchPage } from "@/components/search/SearchPage"
import { ColumnDef, FilterField } from "@/components/search/types"
import { getAllUsers } from "@/features/users/services/userService"
import {
  GetAllUsersRequest,
  ROLES,
  UserQueryResponse,
} from "@/features/users/types/userTypes"

import { Badge } from "@/components/ui/badge"

export default function AdminUsersPage() {
  const router = useRouter()

  const filters: FilterField[] = [
    { name: "firstName", label: "First Name", type: "text" },
    { name: "lastName", label: "Last Name", type: "text" },
    { name: "email", label: "Email", type: "text" },

    {
      name: "role",
      label: "Role",
      type: "select",
      options: [
        { label: "All", value: "all" },
        { label: "Patient", value: ROLES.PATIENT },
        { label: "Doctor", value: ROLES.DOCTOR },
        { label: "Admin", value: ROLES.ADMIN },
      ],
    },
  ]

  const columns: ColumnDef<UserQueryResponse>[] = [
    { header: "ID", accessor: "id" },
    { header: "First Name", accessor: "firstName" },
    { header: "Last Name", accessor: "lastName" },
    { header: "Email", accessor: "email" },

    {
      header: "Roles",
      accessor: (row) => (
        <div className="flex gap-1 flex-wrap">
          {row.roles.map((r) => (
            <Badge key={r} variant="secondary">
              {r}
            </Badge>
          ))}
        </div>
      ),
    },

    {
      header: "Verified",
      accessor: (row) =>
        row.emailVerified ? (
          <Badge className="bg-green-500">Yes</Badge>
        ) : (
          <Badge variant="destructive">No</Badge>
        ),
    },
  ]

  const defaultQuery: GetAllUsersRequest = {
    firstName: "",
    lastName: "",
    email: "",
    role: undefined,
    phoneNumber: "",
    address: "",
    emailVerified: undefined,
    sortOrder: "ASC",
    sortPropertyName: "Id",
    page: 1,
    pageSize: 10,
  }

  return (
    <div className="p-8 max-w-7xl mx-auto">

      <SearchPage<UserQueryResponse, GetAllUsersRequest>
        title="Users"
        filters={filters}
        columns={columns}
        queryFn={getAllUsers}
        defaultQuery={defaultQuery}

        onRowClick={(user) =>
          router.push(`/admin/users/${user.id}`)
        }
      />

    </div>
  )
}