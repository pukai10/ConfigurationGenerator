using System;
namespace AurogonTools
{
	/// <summary>
	/// KeyValue
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class KeyValue<TKey,TValue> : IEquatable<KeyValue<TKey,TValue>>
	{
		private readonly TKey m_key;
		public TKey Key => m_key;

		private readonly TValue m_value;
		public TValue Value => m_value;

		public KeyValue(TKey key,TValue value)
		{
			m_key = key;
			m_value = value;
		}

        public bool Equals(KeyValue<TKey, TValue> other)
        {
			if(other == null)
			{
				return false;
			}

			return other.Key.Equals(Key) && other.Value.Equals(Value);
        }
    }
}

