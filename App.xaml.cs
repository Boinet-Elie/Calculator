using Microsoft.Extensions.DependencyInjection;

namespace MonAppMaui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Configure pour un format téléphone (390x844 pixels - dimensions typiques iPhone)
		var window = new Window(new AppShell())
		{
			Width = 390,
			Height = 844,
			MinimumWidth = 290,
			MinimumHeight = 500
		};
		
		return window;
	}
}