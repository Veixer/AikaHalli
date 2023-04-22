using AikaHalli.Data;
using AikaHalli.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AikaHalli.Pages
{
	public class TasksModel : PageModel
	{
		private readonly IAikaHalliService _aikaHalliService;

		public TasksModel(IAikaHalliService aikaHalliService)
		{
			_aikaHalliService = aikaHalliService;
		}

		[BindProperty]
		public List<UserTask> Items { get; set; }
		
		[BindProperty]
		public UserTask NewItem { get; set; }

		public UserTask UpdatedUserTask { get; set; }

		public string UserId { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Items = new List<UserTask>();
			try
			{
				UserId = GetUserId();
				Items = await _aikaHalliService.GetAllUserTasks(UserId);
			}
			catch (Exception)
			{

				throw;
			}            
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			return RedirectToPage("Tasks");
		}

		public async Task<IActionResult> OnPostAddNewUserTask()
		{
			try
			{
				if (NewItem is not null)
				{
					UserId = GetUserId();
					NewItem.UserId = UserId;
					await _aikaHalliService.AddUserTask(NewItem);
				}                
			}
			catch (Exception)
			{

				throw;
			}
			return RedirectToPage("Tasks");
		}

		public async Task<IActionResult> OnPostUpdateUserTask(int id)
		{
			try
			{
				var taskNameField = "Items[" + id.ToString() + "].TaskName";
				var taskDescriptionField = "Items[" + id.ToString() + "].TaskDescription";

				UpdatedUserTask = new UserTask
				{
					UserId = id.ToString(),
					TaskName = Request.Form[taskNameField],
					TaskDescription = Request.Form[taskDescriptionField]
				};

				if (UpdatedUserTask != null)
				{
					await _aikaHalliService.UpdateUserTask(id, UpdatedUserTask);
				}

				Items = await _aikaHalliService.GetAllUserTasks(UserId);
			}
			catch (Exception)
			{
				throw;
			}
			return RedirectToPage("Tasks");
		}

		public async Task<IActionResult> OnPostDeleteUserTask(int id)
		{
			try
			{
					await _aikaHalliService.DeleteUserTask(id);
				}
			catch (Exception)
			{

				throw;
			}            
			return RedirectToPage("Tasks");
		}


		public string GetUserId()
		{
			ClaimsPrincipal cprincipal = this.User;
			UserId = cprincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
			return UserId;
		}
	}
}
