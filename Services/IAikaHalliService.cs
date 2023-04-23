using AikaHalli.Data;

namespace AikaHalli.Services
{
	public interface IAikaHalliService
	{
		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <param name="userid">UserId</param>
		public Task<List<UserTask>> GetAllUserTasks(string userid);

		///<summary>
		///</summary>
		///<returns></returns>
		///<param name="userTask">New User Task to be added</param>
		public Task AddUserTask(UserTask userTask);

		///<summary>
		///</summary>
		///<returns></returns>
		///<param name="taskId">User Task id to be updated</param>
		///<param name="userTask">User Task with new values</param>
		public Task UpdateUserTask(int taskId, UserTask updatedUserTask);

		///<summary>
		///</summary>
		///<returns></returns>
		///<param name="taskId">User Task to be deleted</param>
		public Task DeleteUserTask(int taskId);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <param name="userId">UserId</param>
		public Task<List<TimeEntry>> GetAllUserTimeEntries(string userId);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <param name="userId">UserId</param>
		public Task<List<TimeEntry>> GetAllUserTimeEntriesToday(string userId);

		///<summary>
		///</summary>
		///<returns></returns>
		///<param name="timeEntry">New Time Entry to be added</param>
		public Task AddTimeEntry(TimeEntry timeEntry);

		///<summary>
		///</summary>
		///<returns></returns>
		///<param name="userId">User Id used to search for TaskIds</param>
		public Task<List<int>> GetAllUserTasksIdList(string userId);

		///<summary>
		///</summary>
		///<returns></returns>
		///<param name="entryId">Time Entry id to be updated</param>
		///<param name="userTask">Time Entry with new values</param>
		public Task UpdateTimeEntry(int entryId, TimeEntry updatedTimeEntry);

		///<summary>
		///</summary>
		///<returns></returns>
		///<param name="entryId">Time Entryto be deleted</param>
		public Task DeleteTimeEntry(int entryId);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <param name="userId">UserId</param>
		public Task<TimeEntry> GetCurrentTimeEntry(string userId);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <param name="userId">UserId</param>
		public Task DownloadUserTaskDurations(string userId);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <param name="userId">UserId</param>
		public Task<List<TimeEntry>> GetUserTasksAndDurations(string userId);
	}
}
