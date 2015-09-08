namespace Magnus.Business.FileSystem
{
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;

	public class ArchiveReader : IArchiveReader
	{
		public void ExtractToTemp(string archivePath, List<string> entriesToExtract)
		{
			using (ZipArchive archive = ZipFile.OpenRead(archivePath))
			{

				var temp = Path.GetTempPath() + "\\EasyType";

				if (Directory.Exists(temp))
					Directory.Delete(temp, true);
				Directory.CreateDirectory(temp);
				IEnumerable<ZipArchiveEntry> zipArchiveEntries;
				if (entriesToExtract == null)
				{
					zipArchiveEntries = archive.Entries;
				}
				else
				{
					zipArchiveEntries = archive.Entries.Where(x => entriesToExtract.Select(f => Path.GetFileName(f.ToLowerInvariant())).Contains(x.Name.ToLowerInvariant()));
				}
				
				foreach (var entry in zipArchiveEntries)
				{
					entry.ExtractToFile(Path.Combine(temp, entry.FullName));
				}
			}
		}
	}
}