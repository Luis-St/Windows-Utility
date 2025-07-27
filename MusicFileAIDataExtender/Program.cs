using System.Text.Json;
using File = System.IO.File;
using Song = MusicFileAIDataExtender.Song;

const string ignoreFile = $"/home/luis/Music/.ignore";

GetAiTasks();
//UpdateMusicFiles();

#region Read
static void GetAiTasks() {
	var ignoredFiles = GetIgnoredFiles();
	
	for (var i = 0; i < 26; i++) {
		var c = (char) ('A' + i);
		
		foreach (var musicFile in GetMusicFiles(c)) {
			if (HasGenres(musicFile) || ignoredFiles.Contains(musicFile.FullName)) {
				continue;
			}

			var name = Path.GetFileNameWithoutExtension(musicFile.Name);
			var albumArtist = musicFile.Directory?.Name;
			Console.WriteLine($"{musicFile.FullName}");
		}
	}
}

static List<string> GetIgnoredFiles() {
	return File.ReadAllLines(ignoreFile).ToList();
}

static List<FileInfo> GetMusicFiles(char letter) {
	var root = new DirectoryInfo("/home/luis/Music/" + letter);
	return root.GetFiles("*.mp3", SearchOption.AllDirectories).ToList();
}

static bool HasGenres(FileInfo file) {
	var audio = TagLib.File.Create(file.FullName);
	return audio.Tag.Genres.Length > 0;
}
#endregion

#region Write
static void UpdateMusicFiles() {
	var genres = new HashSet<string>();
	
	for (var i = 0; i < 26; i++) {
		var c = (char) ('A' + i);
		var content = File.ReadAllText(@$"D:\Temp\Artist - Title\Result\{char.ToUpper(c)}.json");
		if (content.Length == 0) {
			continue;
		}
		
		foreach (var song in JsonSerializer.Deserialize<List<Song>>(content)!) {
			var musicFile = $"D:/Musik/{c}/{song.Artist}/{song.Title}.mp3";
			if (!File.Exists(musicFile)) {
				Console.WriteLine($"File not found: {musicFile}");
				continue;
			}
			
			var audio = TagLib.File.Create(musicFile);
			Console.WriteLine($"Updating genres for {song.Artist} - {song.Title} -> {string.Join(", ", song.Genres.ToArray())}");
			audio.Tag.Genres = song.Genres.Distinct().ToArray();
			audio.Save();
		}
	}
}
#endregion
