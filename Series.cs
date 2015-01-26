using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xwt.Drawing;

namespace ScreenMate
{
	public class Series
	{
		public readonly string Name;
		public readonly Vector Shift;
		public readonly int Timeout;
		public readonly ActionType ActionType;

		private readonly Image[] states;
		private int current = 0;

		public Series (string directory)
		{
			states = Directory.GetFiles(directory, "*.png")
				.OrderBy(file => file)
				.Select(Image.FromFile)
				.ToArray();
			var conf = ParseConfigurationFile(directory);
			Name = conf.ContainsKey("name") ? conf["name"] : Path.GetFileName(directory);
			var dx = conf.ContainsKey("dx") ? Int32.Parse(conf["dx"]) : 0;
			var dy = conf.ContainsKey("dy") ? Int32.Parse(conf["dy"]) : 0;
			Shift = new Vector(dx, dy);
			Timeout = conf.ContainsKey("time") ? Int32.Parse(conf["time"]) : 150;
			ActionType = conf.ContainsKey("type") ? ParseActionType(conf["type"]) : ActionType.Default;
		}

		public bool HasStates
		{
			get { return states.Any(); }
		}

		public Image CurrentState
		{
			get { return states[current]; }
		}

		public void Reset()
		{
			current = 0;
		}

		public void Next ()
		{
			current++;
			if (current == states.Length)
				current = 0;
		}

		private static Dictionary<string, string> ParseConfigurationFile(string directory)
		{
			var configurationFile = Path.Combine(directory, "conf");
			if (!File.Exists(configurationFile)) return new Dictionary<string, string>();
			return File.ReadAllLines(configurationFile)
				.Select(line => line.Split(new[] {'='}, 2, StringSplitOptions.RemoveEmptyEntries))
				.Where(parts => parts.Length >= 2)
				.ToDictionary(strings => strings[0].Trim().ToLower(), strings => strings[1].Trim());
		}

		private static ActionType ParseActionType(string str)
		{
			return (ActionType) Enum.Parse(typeof (ActionType), str, true);
		}
	}
}
