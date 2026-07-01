export const ENDPOINTS = {
  users: {
    root: "/users-api/users",
    me: "/users-api/users/me",
    verifyEmail: "/users-api/users/verify-email",

    admin: "/users-api/admin/users",
    adminById: (id: string) => `/users-api/admin/users/${id}`,
  },
  ratings: {
    root: "/ratings-api/ratings",
    byId: (id: string) => `/ratings-api/ratings/${id}`,
    byDoctor: (doctorId: string) => `/ratings-api/ratings/doctor/${doctorId}`,
    byAppointment: (appointmentId: string) => `/ratings-api/ratings/appointment/${appointmentId}`,
    statsById: (id: string) => `/ratings-api/ratingsStats/${id}`,

    admin: "/ratings-api/admin/ratings",
    adminById: (id: string) => `/ratings-api/admin/ratings/${id}`,
  },
  patients: {
    graphql: "/patients-api/graphql",
    root: (patientId: string) => `/patients-api/patients/${patientId}`,
    allergies: (patientId: string) => `/patients-api/patients/${patientId}/allergies`,
    chronicConditions: (patientId: string) => `/patients-api/patients/${patientId}/chronic-conditions`,
    encounters: (patientId: string) => `/patients-api/patients/${patientId}/encounters`,

    admin: (patientId: string) => `/patients-api/admin/patients/${patientId}`,
    adminAllergies: (patientId: string) => `/patients-api/admin/patients/${patientId}/allergies`,
    adminChronicConditions: (patientId: string) => `/patients-api/admin/patients/${patientId}/chronic-conditions`,
  },
  encounters: {
    root: `/patients-api/encounters`,
    notes: (encounterId: string) => `/patients-api/encounters/${encounterId}/notes`,
    diagnoses: (encounterId: string) => `/patients-api/encounters/${encounterId}/diagnoses`,
    prescriptions: (encounterId: string) => `/patients-api/encounters/${encounterId}/prescriptions`,
    addendums: (encounterId: string) => `/patients-api/encounters/${encounterId}/addendums`,
    lock: (encounterId: string) => `/patients-api/encounters/${encounterId}/lock`,
    finalize: (encounterId: string) => `/patients-api/encounters/${encounterId}/finalize`,

    unlock: (encounterId: string) => `/patients-api/admin/encounters/${encounterId}/unlock`,
    unfinalize: (encounterId: string) => `/patients-api/admin/encounters/${encounterId}/unfinalize`,
  },
  doctors: {
    root: "/doctors-api/doctors",
    me: "/doctors-api/doctors/me",
    specialities: "/doctors-api/specialities",
    meSpecialities: `/doctors-api/doctors/me/specialities`,
    scheduleWorkdays: `/doctors-api/doctors/me/schedule/workdays`,
    availabilityExtra: `/doctors-api/doctors/me/availability/extra`,
    availabilityUnavailable: `/doctors-api/doctors/me/availability/unavailable`,

    admin: "/doctors-api/admin/doctors",
    adminByUser: (userId: string) => `/doctors-api/admin/doctors/user/${userId}`,
    adminSpecialities: (userId: string) => `/doctors-api/admin/doctors/user/${userId}/specialities`,
  },
  appointments: {
    root: "/appointments-api/appointments",
    mine: "/appointments-api/appointments/mine",
    byDoctor: (doctorUserId: string) => `/appointments-api/appointments/doctor/${doctorUserId}`,
    byId: (id: string) => `/appointments-api/appointments/${id}`,

    admin: "/appointments-api/admin/appointments",
    byIdAdmin: (id: string) => `/appointments-api/admin/appointments/${id}`,
    byUserIdAdmin: (userId: string) => `/appointments-api/admin/appointments/user/${userId}`,
  }
}