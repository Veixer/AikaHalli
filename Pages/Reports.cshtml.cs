using AikaHalli.Data;
using AikaHalli.Models;
using AikaHalli.Services;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Security.Claims;

namespace AikaHalli.Pages
{
    public class ReportsModel : PageModel
    {
		private readonly IAikaHalliService _aikaHalliService;

		public ReportsModel(IAikaHalliService aikaHalliService)
		{
			_aikaHalliService = aikaHalliService;
		}

		[BindProperty]
		public List<TaskDuration> Items { get; set; }

		[BindProperty]
		public List<UserTask> TaskList { get; set; }

		public string UserId { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Items = new List<TaskDuration>();
			try
			{
				UserId = GetUserId();
				Items = await _aikaHalliService.GetUserTasksAndDurations(UserId);
				TaskList = await _aikaHalliService.GetAllUserTasks(UserId);
			}
			catch (Exception)
			{
				throw;
			}
			return Page();
		}

		public async Task<IActionResult> OnPostDownloadCsv()
		{
			Items = new List<TaskDuration>();
			try
			{
				UserId = GetUserId();
				Items = await _aikaHalliService.GetUserTasksAndDurations(UserId);

				var memoryStream = new MemoryStream();
				using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
				using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
				{
					await csv.WriteRecordsAsync(Items);
					await writer.FlushAsync();
				}

				memoryStream.Position = 0;
				return File(memoryStream, "text/csv", "task_durations.csv");
			}
			catch (Exception)
			{

				throw;
			}
		}

		public string GetUserId()
		{
			ClaimsPrincipal cprincipal = this.User;
			UserId = cprincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
			return UserId;
		}

		public string ParseMinutesToTime(int minutes)
		{
			TimeSpan time = TimeSpan.FromMinutes(minutes);
			string str = time.ToString(@"hh\:mm");

			return str;

		}
	}
}
