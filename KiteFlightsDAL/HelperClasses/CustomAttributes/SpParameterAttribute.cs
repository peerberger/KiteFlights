using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.HelperClasses.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class SpParameterAttribute : Attribute
	{
		public string Name { get; set; }

		public SpParameterAttribute(string name)
		{
			Name = name;
		}
	}
}
