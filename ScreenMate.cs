using System;
using Xwt;
using Xwt.Drawing;
using System.Collections.Generic;

namespace ScreenMate
{
	public class ScreenMate
	{
		private readonly SeriesCollection seriesCollection;
		private readonly ImageView viewer;
		private readonly Window window;
		private IDisposable timer;

		public ScreenMate ()
		{	
			seriesCollection = new SeriesCollection();
			if (!seriesCollection.HasSeries)
				HandleNotFoundSeries();
			viewer = new ImageView();
			window = new Window
			{
				Content = viewer,
				Decorated = false,
				Visible = false,
				Resizable = false,
				ShowInTaskbar = false,
			};
			window.CloseRequested += HandleCloseRequested;
			window.Show();
		}

		public void Start()
		{
			CreateTrayIcon(seriesCollection.Names);
			viewer.Image = seriesCollection.CurrentState;
		}

		private void CreateTrayIcon(IEnumerable<string> names)
		{
			var statusIcon = Application.CreateStatusIcon();
			statusIcon.Image = Image.FromFile("screenmate.ico");
			statusIcon.Menu = new Menu();

			foreach (var name in names)
			{
				var item = new MenuItem
				{
					Label = name
				};
				item.Clicked += HandleMenuClick;
				statusIcon.Menu.Items.Add(item);
			}
			var exitItem = new MenuItem
			{
				Label = "Выйти",
			};
			exitItem.Clicked += HandleCloseRequested;
			statusIcon.Menu.Items.Add(exitItem);

			SetTimer();
		}

		private void SetTimer ()
		{
			timer = Application.TimeoutInvoke(seriesCollection.Timeout, NextState);
		}

		private void ChangeSeries()
		{
			timer.Dispose();
			UpdateSeries();
			SetTimer();
		}

		private void UpdateSeries()
		{
			viewer.Image = seriesCollection.CurrentState;
		}

		private bool NextState()
		{
			seriesCollection.Next();
			UpdateSeries();
			return true;
		}

#region Handlers

		private static void HandleNotFoundSeries ()
		{
			throw new NotImplementedException();
		}

		private static void HandleCloseRequested (object sender, EventArgs args)
		{
			Application.Exit();
		}

		private void HandleMenuClick (object sender, EventArgs e)
		{
			var item = (MenuItem)sender;
			seriesCollection.ChangeSeriesByName(item.Label);
			ChangeSeries();
		}

#endregion
	}
}
