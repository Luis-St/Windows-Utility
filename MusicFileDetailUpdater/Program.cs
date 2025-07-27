try {
	if (HasNoDuplicates()) {
		FixAlbumArtist();
	} else {
		Console.WriteLine("Duplicates found!");
	}
} catch (Exception ex) {
	Console.WriteLine($"An error occurred: {ex.Message}");
}

static bool HasNoDuplicates() {
	var duplicates = new List<string>();
	foreach (var file in GetMusicFiles()) {
		var name = Path.GetFileNameWithoutExtension(file.Name);
		if (name.Equals(GetAlbumArtist(file))) {
			duplicates.Add(name);
			Console.WriteLine($"Found duplicate file in {file.DirectoryName}");
		} else if ("output".Equals(name)) {
			Console.WriteLine($"Found raw ffmpeg output file in {file.DirectoryName}");
		}
	}
	return duplicates.Count == 0;
}

static void FixAlbumArtist() {
	foreach (var file in GetMusicFiles()) {
		var audio = TagLib.File.Create(file.FullName);
		if (audio.Tag.AlbumArtists.Length != 0) {
			continue;
		}
		var albumArtist = GetAlbumArtist(file);
		if (albumArtist != null) {
			audio.Tag.AlbumArtists = new[] {
				albumArtist
			};
		}
		audio.Save();
		Console.WriteLine($"Updated: {file.FullName} -> {string.Join(", ", audio.Tag.AlbumArtists)}");
	}
}

static List<FileInfo> GetMusicFiles() {
	var root = new DirectoryInfo("/home/luis/Music");
	return root.GetFiles("*.mp3", SearchOption.AllDirectories).ToList();
}

static string? GetAlbumArtist(FileInfo file) {
	return file.Directory?.Name;
}
