using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Domain.Exceptions
{
	public class PriceNotValidException : Exception
	{

		public PriceNotValidException():base("Entered Price not valid, check the price validness")
		{
			
		}
	}
}
