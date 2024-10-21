namespace XVM.Runtime.Execution;

public struct TypedRefPtr
{
	public unsafe void* ptr;

	public unsafe static implicit operator TypedRefPtr(void* ptr)
	{
		TypedRefPtr result = default(TypedRefPtr);
		result.ptr = ptr;
		return result;
	}

	public unsafe static implicit operator void*(TypedRefPtr ptr)
	{
		return ptr.ptr;
	}
}
