using Healthcare_Appointment_and_Management_System.EndPoints;

namespace Healthcare_Appointment_and_Management_System.Extentions
{
	public static class EndpointMapper
	{
		public static void MapAllEndpoints(IEndpointRouteBuilder endpoints)
		{
			var endpointTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => typeof(IEndPoints).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

			foreach (var endpointType in endpointTypes)
			{
				var endpointInstance = Activator.CreateInstance(endpointType) as IEndPoints;

				endpointInstance?.RegisterEndpoints(endpoints);
			}
		}
	}
}
