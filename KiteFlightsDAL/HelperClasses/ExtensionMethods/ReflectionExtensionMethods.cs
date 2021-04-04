using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.HelperClasses.ExtensionMethods
{
	// todo: when writing the xml doc comment, copy and paste in oneNote
	public static class ReflectionExtensionMethods
	{
		public static bool TryGetAttributeValue<TAttribute, TValue>(
			this MemberInfo memberInfo,
			Func<TAttribute, TValue> valueSelector,
			out TValue result
		) where TAttribute : Attribute
		{
			var attribute = memberInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;

			var success = attribute != null;

			if (success)
			{
				result = valueSelector(attribute);
			}
			else
			{
				result = default;
			}

			return success;
		}
	}
}
