using EA.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace EA.Domain.ValueObjects
{
    public class Price
    {
        private decimal _price = 0;
        public Price(decimal price) 
        {
            if(price<=0)
                throw new PriceNotValidException();

            _price = Math.Truncate(100 * price) / 100;
        }

        public static implicit operator decimal(Price price1) 
        {
            return price1._price;
        }

		public static implicit operator Price(decimal price)
		{
			return new Price(price);
		}

        public static bool operator ==(Price price1, Price price2)
        {
            return price1._price == price2._price;
        }

		public static bool operator !=(Price price1, Price price2)
		{
			return price1._price != price2._price;
		}

		public override bool Equals(object price)
		{
			if (price.GetType() != typeof(Price))
			{
				throw new TypeNotSupportedException();
			}

			if (ReferenceEquals(this, price))
			{
				return true;
			}

			if (ReferenceEquals(price, null))
			{
				return false;
			}

			return ((Price)price)._price == this._price;
		}
	}
}
