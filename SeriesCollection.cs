using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xwt.Drawing;

namespace ScreenMate
{
	public class SeriesCollection
	{
		private readonly Series[] seriesList;
		private Series currentSeries;

		public SeriesCollection ()
		{
			seriesList = Directory
				.GetDirectories(Directory.GetCurrentDirectory())
				.Select(directory => new Series(directory))
				.Where(series => series.HasStates)
				.ToArray();
			currentSeries = seriesList.First();
		}

		public bool HasSeries
		{ 
			get { return seriesList.Any(); } 
		}

		public Image CurrentState
		{
			get { return currentSeries.CurrentState; }
		}

		public IEnumerable<string> Names
		{
			get { return seriesList.Select(series => series.Name); }
		}

		public int Timeout {
			get { return currentSeries.Timeout; }
		}

		public void ChangeSeriesByName (string label)
		{
			currentSeries = seriesList.FirstOrDefault(series => series.Name == label);
			currentSeries.Reset();
		}

		public void Next ()
		{
			currentSeries.Next();
		}
	}
}
