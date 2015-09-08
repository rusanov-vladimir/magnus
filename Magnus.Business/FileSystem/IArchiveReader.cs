namespace Magnus.Business.FileSystem
{
	using System.Collections.Generic;

	public interface IArchiveReader
	{
		void ExtractToTemp(string path, List<string> entriesToExtract);
	}
}