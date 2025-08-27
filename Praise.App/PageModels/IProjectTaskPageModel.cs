using CommunityToolkit.Mvvm.Input;
using Praise.App.Models;

namespace Praise.App.PageModels;

public interface IProjectTaskPageModel
{
	IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
	bool IsBusy { get; }
}