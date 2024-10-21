namespace KoiVM.Core.RT;

public interface IChunk
{
	uint Length { get; }

	void OnOffsetComputed(uint offset);

	byte[] GetData();
}
