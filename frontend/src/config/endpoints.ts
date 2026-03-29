export const ENDPOINTS = {
  users: {
    root: "/users-api/api/users",
    byId: (id: string) => `/users-api/api/users/${id}`,
    me: "/users-api/api/users/me",
    verifyEmail: "/users-api/api/users/verify-email",
  },
  ratings: {
    root: "/ratings-api/api/ratings",
    byId: (id: string) => `/ratings-api/api/ratings/${id}`,
    byDoctor: (doctorId: string) => `/ratings-api/api/ratings/by-doctor/${doctorId}`,
    byAppointment: (appointmentId: string) => `/ratings-api/api/ratings/by-appointment/${appointmentId}`,
    statsById: (id: string) => `/ratings-api/api/ratingsStats/${id}`,
  },
  patients: {
    graphql: "/patients-api/graphql",
    root: (patientId: string) => `/patients-api/api/patients/${patientId}`,
    allergies: (patientId: string) => `/patients-api/api/patients/${patientId}/allergies`,
    chronicConditions: (patientId: string) => `/patients-api/api/patients/${patientId}/chronic-conditions`,
    encounters: (patientId: string) => `/patients-api/api/patients/${patientId}/encounters`,
  },
  encounters: {
    root: `/patients-api/api/encounters`,
    notes: (encounterId: string) => `/patients-api/api/encounters/${encounterId}/notes`,
    diagnoses: (encounterId: string) => `/patients-api/api/encounters/${encounterId}/diagnoses`,
    prescriptions: (encounterId: string) => `/patients-api/api/encounters/${encounterId}/prescriptions`,
    addendums: (encounterId: string) => `/patients-api/api/encounters/${encounterId}/addendums`,
    lock: (encounterId: string) => `/patients-api/api/encounters/${encounterId}/lock`,
    finalize: (encounterId: string) => `/patients-api/api/encounters/${encounterId}/finalize`,
  },
  doctors: {
    me: "/doctors-api/api/doctors/me",
    admin: "/doctors-api/api/doctors",
    specialities: "/doctors-api/api/specialities",
    meSpecialities: `/doctors-api/api/doctors/me/specialities`,
    scheduleWorkdays: `/doctors-api/api/doctors/me/schedule/workdays`,
    availabilityExtra: `/doctors-api/api/doctors/me/availability/extra`,
    availabilityUnavailable: `/doctors-api/api/doctors/me/availability/unavailable`,
  },
  appointments: {
    root: "/appointments-api/api/appointments",
    mine: "/appointments-api/api/appointments/mine",
    byId: (id: string) => `/appointments-api/api/appointments/${id}`,
    byDoctor: (doctorUserId: string) => `/appointments-api/api/appointments/by-doctor/${doctorUserId}`,
  }
}