namespace Magnus.Business.Services
{
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using Interfaces;

	public class FileManagerService : IFileManagerService
	{
		#region Implementation of IFileManagerService

		private int _currentFileIndex;
		private readonly IDynamicFieldService _dynamicFieldService;
		private List<string> _extentions = new List<string>
		{
			".tiff",
			".tif"
		};

		public FileManagerService(IDynamicFieldService dynamicFieldService)
		{
			_dynamicFieldService = dynamicFieldService;
			Catalog = Path.GetTempPath() + "EasyType";
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
				ext = fileEtentions;
			Files = Directory.GetFiles(Catalog)
							.OrderBy(x => x)
							.Where(entry => ext.Any(e => entry.ToLower().EndsWith(e)))
							.ToList();

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

		public void ExtractArchiveToTemp(string path, List<string> entriesToExtract = null)
		{
			using (ZipArchive archive = ZipFile.OpenRead(path))
			{
				var temp = Path.GetTempPath() + "EasyType";

				if (Directory.Exists(temp))
					Directory.Delete(temp, true);
				Directory.CreateDirectory(temp);

				IEnumerable<ZipArchiveEntry> zipArchiveEntries;
				if (entriesToExtract == null)
					zipArchiveEntries = archive.Entries;
				else
				{
					zipArchiveEntries =
						archive.Entries.Where(
							x => entriesToExtract.Select(f => Path.GetFileName(f.ToLowerInvariant())).Contains(x.Name.ToLowerInvariant()));
				}
				foreach (var entry in zipArchiveEntries)
					entry.ExtractToFile(Path.Combine(temp, entry.FullName));
			}
		}

		public void ExportFields(bool useStaticExport)
		{
			var dataToExport = _dynamicFieldService.Export(useStaticExport);

			foreach (var pair in dataToExport)
			{
				using (Stream stream = new FileStream(
					pair.Key + (useStaticExport ? " Static.txt" : " Dynamic.txt"),
					FileMode.Create,
					FileAccess.Write))
				{
					using (var writer = new StreamWriter(stream))
					{
						writer.WriteAsync(pair.Value);
					}
				}
			}
		}

		#endregion
	}
}