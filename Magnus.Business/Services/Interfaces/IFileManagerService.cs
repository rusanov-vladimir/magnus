namespace Magnus.Business.Services.Interfaces
{
	using System.Collections.Generic;

	public interface IFileManagerService
	{
		string Catalog { get; set; }
		List<string> Files { get; set; }
		string CurrentFile { get; set; }
		int FilesCount { get; set; }
		int CurrentFileNumber { get; set; }
		void ReadFileNames(List<string> fileEtentions = null);
		void GetNextFile();
		void GetPreviousFile();
		void ExtractArchiveToTemp(string path, List<string> entriesToExtract = null);
		void ExportFields(bool useStaticExport);
	}
}