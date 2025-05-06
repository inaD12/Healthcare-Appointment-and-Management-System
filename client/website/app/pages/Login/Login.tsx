const Login = () => {

    return (
        <>
            <div className="flex items-center justify-center h-screen">
                <div className="flex flex-col gap-4 bg-white w-screen h-screen sm:h-auto sm:min-w-80 sm:max-w-[30%] sm:rounded-xl justify-center text-center text-black p-4 outline-3 outline-blue-700">
                    <h1 className="text-4xl my-10">Login</h1>
                    <input
                        type="text"
                        placeholder="Username"
                        className="bg-gray-300 p-2 mb-3 rounded-md"
                    />
                    <input
                        type="password"
                        placeholder="Password"
                        className="bg-gray-300 p-2 mb-3 rounded-md"
                    />
                    <button
                        type="submit"
                        className="mx-auto bg-blue-500 text-white p-2 h-[50px] w-[150px] rounded-md hover:bg-blue-600 transition"
                    >
                        Submit
                    </button>
                </div>
            </div>
        </>
    );

}

export default Login;
