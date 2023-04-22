using AikaHalli.Data;
using AikaHalli.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AikaHalli.Pages
{
    public class TimeEntriesModel : PageModel
    {
		private readonly IAikaHalliService _aikaHalliService;

        public TimeEntriesModel(IAikaHalliService aikaHalliService)
		{
			_aikaHalliService = aikaHalliService;
		}

		[BindProperty]
		public List<TimeEntry> Items { get; set; }

		[BindProperty]
		public TimeEntry NewItem { get; set; }

		[BindProperty]
		public List<int> TaskIdList { get; set; }

		public TimeEntry UpdatedTimeEntry { get; set; }

		public string UserId { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Items = new List<TimeEntry>();
			try
			{
				UserId = GetUserId();
				Items = await _aikaHalliService.GetAllUserTimeEntries(UserId);
				TaskIdList = await _aikaHalliService.GetAllUserTasksIdList(UserId);
			}
			catch (Exception)
			{

				throw;
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			return RedirectToPage("TimeEntries");
		}

		public async Task<IActionResult> OnPostAddNewTimeEntry()
		{
			try
			{
				if (NewItem is not null)
				{
					UserId = GetUserId();
					NewItem.UserId = UserId;
					await _aikaHalliService.AddTimeEntry(NewItem);
				}
			}
			catch (Exception)
			{

				throw;
			}
			return RedirectToPage("TimeEntries");
		}

		public async Task<IActionResult> OnPostUpdateTimeEntry(int id)
		{
			try
			{
				var taskIdField = "Items[" + id.ToString() + "].TaskId";
				var startTimeField = "Items[" + id.ToString() + "].StartTime";
				var endTimeField = "Items[" + id.ToString() + "].EndTime";
				var notesField = "Items[" + id.ToString() + "].Notes";

				var taskIdFromForm = Convert.ToInt32(Request.Form[taskIdField]);
				var startTimeFromForm = Convert.ToDateTime(Request.Form[startTimeField]);
				var endTimeFromField = Convert.ToDateTime(Request.Form[endTimeField]);


				UpdatedTimeEntry = new TimeEntry
				{
					UserId = id.ToString(),
					TaskId = taskIdFromForm,
					StartTime = startTimeFromForm,
					EndTime = endTimeFromField,
					Notes = Request.Form[notesField]
				};

				if (UpdatedTimeEntry != null)
				{
					await _aikaHalliService.UpdateTimeEntry(id, UpdatedTimeEntry);
				}

				Items = await _aikaHalliService.GetAllUserTimeEntries(UserId);
			}
			catch (Exception)
			{
				throw;
			}
			return RedirectToPage("TimeEntries");
		}

		public async Task<IActionResult> OnPostDeleteTimeEntry(int id)
		{
			try
			{
				await _aikaHalliService.DeleteTimeEntry(id);
			}
			catch (Exception)
			{

				throw;
			}
			return RedirectToPage("TimeEntries");
		}

		public string GetUserId()
		{
			ClaimsPrincipal cprincipal = this.User;
			UserId = cprincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
			return UserId;
		}
	}
}
