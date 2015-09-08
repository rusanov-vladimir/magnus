namespace Client.FileSystem
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class FileSystemHelper : IFileSystemHelper
	{
		private int _currentFileIndex;

		public FileSystemHelper()
		{
			Catalog = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Source\";
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

		public void ReadFileNames()
		{
			Files = Directory.GetFiles(Catalog).ToList();

			CurrentFile = Files[_currentFileIndex];
			FilesCount = Files.Count;
		}

		public void GetNextFile()
		{
			if (_currentFileIndex < Files.Count)
			{
				_currentFileIndex++;
				CurrentFile = _currentFileIndex < Files.Count ? Files[_currentFileIndex] : null;
			}
		}

		public void GetPreviousFile()
		{
			if (_currentFileIndex > 0)
			{
				_currentFileIndex--;
				if (_currentFileIndex < Files.Count)
					CurrentFile = Files[_currentFileIndex];
				else
					CurrentFile = null;
			}
		}
	}
}