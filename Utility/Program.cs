CreateAlphabetFiles();

static void CreateAlphabetFiles() {
	for (var i = 0; i < 26; i++) {
		var c = (char)('A' + i);
		var characterFile = @$"D:\Temp\Artist - Title\Result\{char.ToUpper(c)}.json";
		using var stream = File.Create(characterFile);
	}
}