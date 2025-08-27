using Praise.App.Models;
using Praise.App.PageModels;

namespace Praise.App.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}