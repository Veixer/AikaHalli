using AikaHalli.Data;
using AikaHalli.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AikaHalli.Pages
{
    public class TimeTrackerModel : PageModel
    {
		private readonly IAikaHalliService _aikaHalliService;

		public TimeTrackerModel(IAikaHalliService aikaHalliService)
		{
			_aikaHalliService = aikaHalliService;
		}

		[BindProperty]
		public List<TimeEntry> Items { get; set; }

		[BindProperty]
		public TimeEntry NewItem { get; set; }

		[BindProperty]
		public TimeEntry CurrentItem { get; set; }

		[BindProperty]
		public List<UserTask> TaskList { get; set; }

		[BindProperty]
		public List<int> TaskIdList { get; set; }

		public string UserId { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Items = new List<TimeEntry>();
			try
			{
				UserId = GetUserId();
				Items = await _aikaHalliService.GetAllUserTimeEntriesToday(UserId);
				TaskList = await _aikaHalliService.GetAllUserTasks(UserId);
				TaskIdList = await _aikaHalliService.GetAllUserTasksIdList(UserId);
				CurrentItem = await _aikaHalliService.GetCurrentTimeEntry(UserId);
			}
			catch (Exception)
			{

				throw;
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAddNewTimeEntry()
		{
			try
			{
				if (NewItem is not null)
				{
					UserId = GetUserId();
					NewItem.UserId = UserId;
					NewItem.StartTime = DateTime.Now;
					NewItem.Notes = Request.Form["NewItem.Notes"];
					await _aikaHalliService.AddTimeEntry(NewItem);
					Items = await _aikaHalliService.GetAllUserTimeEntriesToday(UserId);
				}
			}
			catch (Exception)
			{

				throw;
			}
			return RedirectToPage("TimeTracker");
		}

		public async Task<IActionResult> OnPostStopCurrentTimeEntry()
		{
			try
			{
				if (CurrentItem is not null)
				{
					UserId = GetUserId();
					CurrentItem = await _aikaHalliService.GetCurrentTimeEntry(UserId);
					CurrentItem.EndTime = DateTime.Now;
					await _aikaHalliService.UpdateTimeEntry(CurrentItem.EntryId, CurrentItem);
				}
			}
			catch (Exception)
			{

				throw;
			}
			return RedirectToPage("TimeTracker");
		}

		public string GetUserId()
		{
			ClaimsPrincipal cprincipal = this.User;
			UserId = cprincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
			return UserId;
		}
	}
}
