namespace Magnus.Business.FileSystem
{
	using System.Collections.Generic;

	public interface IFileSystemHelper
	{
		string Catalog { get; set; }
		List<string> Files { get; set; }
		string CurrentFile { get; set; }
		int FilesCount { get; set; }
		int CurrentFileNumber { get; set; }
		void ReadFileNames(List<string> fileEtentions = null);
		void GetNextFile();
		void GetPreviousFile();
	}
}