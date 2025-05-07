
const apiRoutes = {
    baseUrl: 'http://localhost:5000',
    users: {
        login: `/users-api/api/users/login`,
        register: `/users-api/api/users/register`,
        updateCurrent: `/users-api/api/users/update-current`,
        update: (id: string) => `/users-api/api/users/update${id}`,
        getAll: `/users-api/api/users/get-all`,
        get: (id: string) => `/users-api/api/users/get${id}`,
        deleteCurrent: `/users-api/api/users/delete-current`,
        delete: (id: string) => `/users-api/api/users/delete${id}`
    }
};

export default apiRoutes;
