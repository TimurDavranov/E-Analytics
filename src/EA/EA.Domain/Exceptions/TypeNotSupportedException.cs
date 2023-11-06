using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Domain.Exceptions
{
	public class TypeNotSupportedException : Exception
	{

		public TypeNotSupportedException():base("Wrong type")
		{
			
		}
	}
}
