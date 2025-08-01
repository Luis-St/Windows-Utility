﻿using YoutubeExplode;
using YoutubeExplode.Converter;

const string folder = @"/home/luis/.tmp/out";
const string url = "https://www.youtube.com/playlist?list=PL8i73892gT1rqHX-cZ9ZRDqGP8YWGylqB";

var yt = new YoutubeClient();
Console.WriteLine($"Enter playlist url: {url}");
Console.Write($"Enter output folder, press enter to use the music folder: {folder}");
Console.WriteLine();

await foreach (var video in yt.Playlists.GetVideosAsync(url)) {
	try {
		var title = string.Join(" ", video.Title.Split(GetInvalidChars())).Trim();
		Console.Write(title);

		await yt.Videos.DownloadAsync(video.Url, @$"{folder}/{title}.mp3", progress: new Progress<double>(d => {
			var percent = (int)(d * 10);
			Console.Write($"\r{title} [{new string('#', percent)}{new string(' ', 10 - percent)}] {(int)(d * 100)}%");
		}));
		Console.Write($"\r{title} {new string(' ', 50)}\n\r");
	} catch (Exception e) {
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"\rDownload of {video.Title} failed: {e}");
		Console.ResetColor();
	}
}

static char[] GetInvalidChars() {
	var invalidChars = Path.GetInvalidPathChars().ToList();
	invalidChars.AddRange(Path.GetInvalidFileNameChars().AsEnumerable()); 
	return invalidChars.ToArray();
}
