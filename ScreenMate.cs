using System;
using Xwt;
using Xwt.Drawing;
using System.Collections.Generic;

namespace ScreenMate
{
	public class ScreenMate
	{
		private readonly SeriesCollection SeriesCollection;
		private readonly ImageView Viewer;
		private readonly Window Window;
		private IDisposable Timer;

		public ScreenMate ()
		{	
			SeriesCollection = new SeriesCollection();
			if (!SeriesCollection.HasSeries) // TODO: message
				HandleNotFoundSeries();
			Viewer = new ImageView();
			Window = new Window
			{
				Content = Viewer,
				Decorated = false,
				Visible = false,
				Resizable = false,
				ShowInTaskbar = false,
			};
			Window.CloseRequested += HandleCloseRequested;
			Window.Show();
		}

		public void Start()
		{
			CreateTrayIcon(SeriesCollection.Names);
			Viewer.Image = SeriesCollection.CurrentState;
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
			Timer = Application.TimeoutInvoke(SeriesCollection.Timeout, NextState);
		}

		private void ChangeSeries()
		{
			Timer.Dispose();
			UpdateSeries();
			SetTimer();
		}

		private void UpdateSeries()
		{
			Viewer.Image = SeriesCollection.CurrentState;
		}

		private bool NextState()
		{
			SeriesCollection.Next();
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
			SeriesCollection.ChangeSeriesByName(item.Label);
			ChangeSeries();
		}

#endregion
	}
}
