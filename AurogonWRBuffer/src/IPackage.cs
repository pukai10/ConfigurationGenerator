using System;
namespace AurogonWRBuffer
{
	public interface IPackage
	{
		WRErrorType Pack(ref WriteBuffer writer);
		WRErrorType UnPack(ref ReadBuffer reader);
	}
}

