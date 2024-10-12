using System.Globalization;
using System.Text.RegularExpressions;

var directory = @"C:\Users\Luis\Desktop\OneDrive-2024-08-11";
var files = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly);
foreach (var file in files) {
	var name = Path.GetFileName(file);
	try {
		DateTime? date = null;
		if (name.StartsWith("IMG_")) {
			if (DateTime.TryParseExact(name.Substring(4, 15), "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)) {
				if (!name.StartsWith("IMG_" + parsedDate.ToString("yyyyMMdd_HHmmss"))) {
					Console.WriteLine($"{name} -> Filename does not match date");
					continue;
				}
				date = parsedDate;
			}
		} else if (name.StartsWith("IMG-") || name.StartsWith("VID-")) {
			if (DateTime.TryParseExact(name.Substring(4, 8) + "-120000", "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)) {
				if (!name.StartsWith("IMG-" + parsedDate.ToString("yyyyMMdd")) && !name.StartsWith("VID-" + parsedDate.ToString("yyyyMMdd"))) {
					Console.WriteLine($"{name} -> Filename does not match date");
					continue;
				}
				date = parsedDate;
			}
		} else if (name.StartsWith("PANO_")) {
			if (DateTime.TryParseExact(name.Substring(5, 15), "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)) {
				if (!name.StartsWith("PANO_" + parsedDate.ToString("yyyyMMdd_HHmmss"))) {
					Console.WriteLine($"{name} -> Filename does not match date");
					continue;
				}
				date = parsedDate;
			}
		} else if (name.StartsWith("Screenshot_")) {
			if (DateTime.TryParseExact(name.Substring(11, 15), "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)) {
				if (!name.StartsWith("Screenshot_" + parsedDate.ToString("yyyyMMdd-HHmmss"))) {
					Console.WriteLine($"{name} -> Filename does not match date");
					continue;
				}
				date = parsedDate;
			}
		} else {
			if (DateTime.TryParseExact(name.Substring(0, 15), "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)) {
				if (!name.StartsWith(parsedDate.ToString("yyyyMMdd_HHmmss"))) {
					Console.WriteLine($"{name} -> Filename does not match date");
					continue;
				}
				date = parsedDate;
			}
		}

		if (date != null) {
			Console.WriteLine($"{name} -> {date}");
			//File.SetCreationTime(file, date.Value);
			//File.SetLastWriteTime(file, date.Value);
		} else {
			Console.WriteLine($"{name} -> No date found");
		}
	} catch (Exception e) {
		Console.WriteLine($"{name} -> Error: {e.Message}");
		throw;
	}
}