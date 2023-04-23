using AikaHalli.Data;
using AikaHalli.Models;
using AikaHalli.Repository;
using System.Threading.Tasks;

namespace AikaHalli.Services
{
	/// <summary>
	/// In this class there are all methods related to accessing database for Aikahalli application.
	/// </summary>
	public class AikaHalliService : IAikaHalliService
	{
		private readonly IAikaHalliRepository _aikaHalliRepository;

		public AikaHalliService(IAikaHalliRepository aikaHalliRepository)
		{
			_aikaHalliRepository = aikaHalliRepository;
		}

		/// <inheritdoc/>
		public async Task<List<UserTask>> GetAllUserTasks(string userId)
		{
			return await _aikaHalliRepository.GetAllUserTasks(userId);
		}

		/// <inheritdoc/>
		public async Task AddUserTask(UserTask userTask)
		{
			await _aikaHalliRepository.AddUserTask(userTask);
		}

		/// <inheritdoc/>
		public async Task UpdateUserTask(int taskId, UserTask updatedUserTask)
		{
			var originalUserTask = _aikaHalliRepository.GetUserTask(taskId).Result;
			if (originalUserTask != null && HasChanges(originalUserTask, updatedUserTask))
			{
				_aikaHalliRepository.UpdateUserTask(taskId, updatedUserTask);
			}
		}

		/// <inheritdoc/>
		public async Task DeleteUserTask(int taskId)
		{
			await _aikaHalliRepository.DeleteUserTask(taskId);
		}

		/// <inheritdoc/>
		public async Task<List<TimeEntry>> GetAllUserTimeEntries(string userId)
		{
			return await _aikaHalliRepository.GetAllUserTimeEntries(userId);
		}

		/// <inheritdoc/>
		public async Task<List<TimeEntry>> GetAllUserTimeEntriesToday(string userId)
		{
			return await _aikaHalliRepository.GetAllUserTimeEntriesToday(userId);
		}

		/// <inheritdoc/>
		public async Task AddTimeEntry(TimeEntry timeEntry)
		{
			HandleTimeEntry(timeEntry);
			await _aikaHalliRepository.AddTimeEntry(timeEntry);
		}

		/// <inheritdoc/>
		public async Task<List<int>> GetAllUserTasksIdList(string userId)
		{
			var userTasks = await GetAllUserTasks(userId);
			var userTaskIdList = userTasks.Select(x => x.TaskId).ToList();

			return userTaskIdList;
		}

		/// <inheritdoc/>
		public async Task UpdateTimeEntry(int entryId, TimeEntry updatedTimeEntry)
		{
			HandleTimeEntry(updatedTimeEntry);
			var originalUserTask = _aikaHalliRepository.GetTimeEntry(entryId).Result;
			if (originalUserTask != null && HasChanges(originalUserTask, updatedTimeEntry))
			{
				await _aikaHalliRepository.UpdateTimeEntry(entryId, updatedTimeEntry);
			}
		}

		/// <inheritdoc/>
		public async Task<TimeEntry> GetCurrentTimeEntry(string userId)
		{
			return await _aikaHalliRepository.GetCurrentTimeEntry(userId);
		}

		/// <inheritdoc/>
		public async Task DeleteTimeEntry(int entryId)
		{
			await _aikaHalliRepository.DeleteTimeEntry(entryId);
		}

		/// <inheritdoc/>
		public async Task<List<TaskDuration>> GetUserTasksAndDurations(string userId)
		{
			return await _aikaHalliRepository.GetUserTasksAndDurations(userId);
		}

		/// <inheritdoc/>
		//public async Task DownloadUserTaskDurations(string userId)
		//{
		//	var taskDurationsList = await _aikaHalliRepository.GetUserTasksAndDurations(userId);

		//	// download csv
		//}

		private bool HasChanges(UserTask userTask1, UserTask userTask2)
		{
			if (userTask1 is null || userTask2 is null)
			{
				return false;
			}
			var hasChanges = userTask1.TaskName != userTask2.TaskName || userTask1.TaskDescription != userTask2.TaskDescription;

			return hasChanges;
		}

		private bool HasChanges(TimeEntry timeEntry1, TimeEntry timeEntry2)
		{
			if (timeEntry1 is null || timeEntry2 is null)
			{
				return false;
			}
			var hasChanges = timeEntry1.TaskId != timeEntry2.TaskId || timeEntry1.StartTime != timeEntry2.StartTime 
							|| timeEntry1.EndTime != timeEntry2.EndTime || timeEntry1.Notes != timeEntry2.Notes;

			return hasChanges;
		}

		private TimeEntry HandleTimeEntry(TimeEntry te)
		{
			if (te.EndTime is not null)
			{
				te.Duration = CountMinutes(te.StartTime, te.EndTime);
			}
			
			te.EntryDate = DateTime.Now;
			
			return te;
		}

		private int CountMinutes(DateTime? dt1, DateTime? dt2)
		{
			int diffInt = 0;

			if (dt1 != null ||dt2 != null)
			{
				var dtt1 = (DateTime)dt1;
				var dtt2 = (DateTime)dt2;
				TimeSpan span = dtt2.Subtract(dtt1);
				var diff = Math.Round(span.TotalMinutes, MidpointRounding.AwayFromZero);
				diffInt = Convert.ToInt32(diff);
			}
			

			return diffInt;
		}

		
	}
}
