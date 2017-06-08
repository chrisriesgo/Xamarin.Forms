﻿using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

// Apply the default category of "Issues" to all of the tests in this assembly
// We use this as a catch-all for tests which haven't been individually categorized
#if UITEST
[assembly: NUnit.Framework.Category("Issues")]
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 9953179, "PopAsync crashing after RemovePage when support packages are updated to 25.1.1", PlatformAffected.Android)]
	public class Bugzilla53179 : TestNavigationPage
	{
		class TestPage : ContentPage
		{
			Button nextBtn, rmBtn, popBtn;

			public TestPage(int index)
			{
				nextBtn = new Button { Text = "Next Page" };
				rmBtn = new Button { Text = "Remove previous page" };
				popBtn = new Button { Text = "Back" };

				nextBtn.Clicked += async (sender, e) => await Navigation.PushAsync(new TestPage(index + 1));
				rmBtn.Clicked += (sender, e) =>
				{
					var stackSize = Navigation.NavigationStack.Count;
					Navigation.RemovePage(Navigation.NavigationStack[stackSize - 2]);
					popBtn.IsVisible = true;
					rmBtn.IsVisible = false;
				};
				popBtn.Clicked += async (sender, e) => await Navigation.PopAsync();

				switch (index)
				{
					case 3:
						nextBtn.IsVisible = false;
						popBtn.IsVisible = false;
						break;
					default:
						rmBtn.IsVisible = false;
						popBtn.IsVisible = false;
						break;
				}

				Content = new StackLayout
				{
					Children = {
					new Label { Text = $"This is page {index}"},
					nextBtn,
					rmBtn,
					popBtn
				}
				};
			}
		}


		protected override void Init()
		{
			PushAsync(new TestPage(1));
		}

#if UITEST
		[Test]
		public void Bugzilla53179Test()
		{
			RunningApp.Screenshot ("I am at Issue 1");
			RunningApp.WaitForElement (q => q.Marked ("IssuePageLabel"));
			RunningApp.Screenshot ("I see the Label");
		}
#endif
	}
}