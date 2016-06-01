using System.Collections.Generic;
using System.Dynamic;
using System.Web.Routing;

namespace Web.Infrastructure.Helpers
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Convert an anonymous object into <see cref="ExpandoObject"/>
		/// </summary>
		/// <remarks>http://stackoverflow.com/a/5670899/59563</remarks>
		public static ExpandoObject ToExpando(this object anonymousObject)
		{
			var anonymousDictionary = new RouteValueDictionary(anonymousObject);
			var expando = new ExpandoObject();
			
			foreach (var item in anonymousDictionary)
			{
				expando.Add(item);
			}
				
			return (ExpandoObject)expando;
		}
	}
}