using System.Text.Json;
using TagLib;
using File = System.IO.File;
using Song = MusicFileAIDataExtender.Song;

const string header = @"
List all genres of the following songs, i need the result as an json array of json objects of format: { ""title"": <title>, ""artist"": <artist>, ""genres"": [ ""genre_1"", ""genre_2"", ... ] }.
Please exclude duplicate genres like: Hard Rock, since its included in Rock
All songs should include genre pop:
";

//GetAiTasks(true);
//UpdateMusicFiles();

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

#region Read
static void GetAiTasks(bool generateTaskHeader) {
	for (var i = 0; i < 26; i++) {
		var c = (char) ('A' + i);
		var characterFile = @$"D:\Temp\Artist - Title\{char.ToUpper(c)}.txt";
		using var stream = new StreamWriter(File.Create(characterFile));

		if (generateTaskHeader) {
			stream.WriteLine(header.Trim());
		}

		var wroteAny = false;
		foreach (var musicFile in GetMusicFiles(c)) {
			if (HasGenres(musicFile)) {
				continue;
			}
			wroteAny = true;

			var name = Path.GetFileNameWithoutExtension(musicFile.Name);
			var albumArtist = musicFile.Directory?.Name;
			stream.WriteLine($"{albumArtist} - {name}");
			Console.WriteLine($"{albumArtist} - {name}");
		}
		if (wroteAny) {
			continue;
		}
		stream.Close();
		File.Delete(characterFile);
	}
}

static List<FileInfo> GetMusicFiles(char letter) {
	var root = new DirectoryInfo("D:/Musik/" + letter);
	return root.GetFiles("*.mp3", SearchOption.AllDirectories).ToList();
}

static bool HasGenres(FileInfo file) {
	var audio = TagLib.File.Create(file.FullName);
	return audio.Tag.Genres.Length > 0;
}
#endregion