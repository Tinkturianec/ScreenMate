using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xwt.Drawing;

namespace ScreenMate
{
	public class SeriesCollection
	{
		private readonly Series[] SeriesList;
		private Series CurrentSeries;

		public SeriesCollection ()
		{
			SeriesList = Directory
				.GetDirectories(Directory.GetCurrentDirectory())
				.Select(directory => new Series(directory))
				.Where(series => series.HasStates)
				.ToArray();
			CurrentSeries = SeriesList.First();
		}

		public bool HasSeries
		{ 
			get { return SeriesList.Any(); } 
		}

		public Image CurrentState
		{
			get { return CurrentSeries.CurrentState; }
		}

		public IEnumerable<string> Names
		{
			get { return SeriesList.Select(series => series.Name); }
		}

		public int Timeout {
			get { return CurrentSeries.Timeout; }
		}

		public void ChangeSeriesByName (string label)
		{
			CurrentSeries = SeriesList.FirstOrDefault(series => series.Name == label);
			CurrentSeries.Reset();
		}

		public void Next ()
		{
			CurrentSeries.Next();
		}
	}
}
