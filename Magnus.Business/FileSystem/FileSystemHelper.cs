namespace Magnus.Business.FileSystem
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class FileSystemHelper : IFileSystemHelper
	{
		private int _currentFileIndex;
		private List<string> _extentions = new List<string>
		{
			".tiff",
			".tif"
		};

		public FileSystemHelper()
		{
			
			Catalog = Path.GetTempPath() + "\\EasyType";
		}

		public string Catalog { get; set; }
		public List<string> Files { get; set; }
		public string CurrentFile { get; set; }
		public int FilesCount { get; set; }
		public int CurrentFileNumber
		{
			get { return _currentFileIndex + 1; }
			set { _currentFileIndex = value - 1; }
		}

		public void ReadFileNames(List<string> fileEtentions = null)
		{
			var ext = _extentions;
			if (fileEtentions != null)
			{
				ext = fileEtentions;
			}
			Files = Directory.GetFiles(Catalog).Where(entry => ext.Any(e=>entry.ToLower().EndsWith(e))).ToList();

			CurrentFile = Files[_currentFileIndex];
			FilesCount = Files.Count;
		}

		public void GetNextFile()
		{
			CurrentFile = _currentFileIndex < Files.Count ? Files[++_currentFileIndex] : null;
		}

		public void GetPreviousFile()
		{
			CurrentFile = _currentFileIndex > 0 ? Files[--_currentFileIndex] : null;
		}
	}
}