using System.Reflection;

namespace Application.Models.ComponentModel
{
	public class EnumerationAttributeRetriever<TAttribute, TEnumeration> where TAttribute : Attribute where TEnumeration : struct, Enum
	{
		#region Fields

		private IDictionary<TEnumeration, TAttribute> _dictionary;
		private readonly object _lock = new();

		#endregion

		#region Properties

		public virtual IDictionary<TEnumeration, TAttribute> Dictionary
		{
			get
			{
				// ReSharper disable All
				if(this._dictionary == null)
				{
					lock(this._lock)
					{
						if(this._dictionary == null)
						{
							var dictionary = new Dictionary<TEnumeration, TAttribute>();
							var type = typeof(TEnumeration);

							foreach(var enumeration in Enum.GetValues<TEnumeration>())
							{
								var field = type.GetField(enumeration.ToString());
								var attribute = (TAttribute)field.GetCustomAttribute(typeof(TAttribute), false);
								dictionary.Add(enumeration, attribute);
							}

							this._dictionary = dictionary;
						}
					}
				}
				// ReSharper restore All

				return this._dictionary;
			}
		}

		#endregion

		#region Methods

		public virtual TAttribute GetAttribute(TEnumeration enumeration)
		{
			return this.Dictionary.TryGetValue(enumeration, out var attribute) ? attribute : null;
		}

		#endregion
	}
}