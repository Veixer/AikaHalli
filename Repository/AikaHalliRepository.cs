﻿using AikaHalli.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AikaHalli.Repository
{
	/// <summary>
	/// In this class there are all methods related to accessing database for AikaHalli application.
	/// </summary>
	public class AikaHalliRepository : IAikaHalliRepository
	{
		private readonly IDbContextFactory<ApplicationDbContext> _appContextFactory;

		public AikaHalliRepository(IDbContextFactory<ApplicationDbContext> appContextFactory)
		{
			_appContextFactory = appContextFactory;
		}

		/// <inheritdoc/>
		public async Task<List<UserTask>> GetAllUserTasks(string userId)
		{
			using var context = _appContextFactory.CreateDbContext();
			var userTasks = await context.UserTasks.Where(x => x.UserId == userId).ToListAsync();

			return userTasks;
		}

		/// <inheritdoc/>
		public async Task<UserTask> GetUserTask(int taskId)
		{
			using var context = _appContextFactory.CreateDbContext();
			var userTask = await context.UserTasks.Where(x => x.TaskId == taskId).FirstOrDefaultAsync();

			return userTask;
		}

		/// <inheritdoc/>
		public async Task AddUserTask(UserTask userTask)
		{
			using var context = _appContextFactory.CreateDbContext();
			context.UserTasks.Add(userTask);

			await context.SaveChangesAsync();
		}

		/// <inheritdoc/>
		public async Task UpdateUserTask(int taskId, UserTask updatedUserTask)
		{
			using var context = _appContextFactory.CreateDbContext();
			var originalUserTask = context.UserTasks.SingleOrDefault(x => x.TaskId == taskId);
			if (originalUserTask != null)
			{
				originalUserTask.TaskName = updatedUserTask.TaskName;
				originalUserTask.TaskDescription = updatedUserTask.TaskDescription;
			}

			await context.SaveChangesAsync();
		}

		/// <inheritdoc/>
		public async Task DeleteUserTask(int taskId)
		{
			using var context = _appContextFactory.CreateDbContext();
			var taskToRemove = context.UserTasks.Where(x => x.TaskId == taskId).FirstOrDefault();
			if (taskToRemove != null)
			{
				context.UserTasks.Remove(taskToRemove);
			}
			await context.SaveChangesAsync();
		}

		/// <inheritdoc/>
		public async Task<List<TimeEntry>> GetAllUserTimeEntries(string userId)
		{
			using var context = _appContextFactory.CreateDbContext();
			var timeEntries = await context.TimeEntries.Where(x => x.UserId == userId).ToListAsync();

			return timeEntries;
		}

		/// <inheritdoc/>
		public async Task AddTimeEntry(TimeEntry timeEntry)
		{
			using var context = _appContextFactory.CreateDbContext();
			context.TimeEntries.Add(timeEntry);

			await context.SaveChangesAsync();
		}

		public async Task<TimeEntry> GetTimeEntry(int entryId)
		{
			using var context = _appContextFactory.CreateDbContext();
			var timeEntry = await context.TimeEntries.Where(x => x.EntryId == entryId).FirstOrDefaultAsync();

			return timeEntry;
		}

		public async Task UpdateTimeEntry(int entryId, TimeEntry updatedTimeEntry)
		{
			using var context = _appContextFactory.CreateDbContext();
			var originalTimeEntry = context.TimeEntries.SingleOrDefault(x => x.EntryId == entryId);
			if (originalTimeEntry != null)
			{
				originalTimeEntry.TaskId = updatedTimeEntry.TaskId;
				originalTimeEntry.StartTime = updatedTimeEntry.StartTime;
				originalTimeEntry.EndTime = updatedTimeEntry.EndTime;
				originalTimeEntry.Duration = updatedTimeEntry.Duration;
				originalTimeEntry.EntryDate = updatedTimeEntry.EntryDate;
				originalTimeEntry.Notes = updatedTimeEntry.Notes;
			}

			await context.SaveChangesAsync();
		}

		public async Task DeleteTimeEntry(int entryId)
		{
			using var context = _appContextFactory.CreateDbContext();
			var entryToRemove = context.TimeEntries.Where(x => x.EntryId == entryId).FirstOrDefault();
			if (entryToRemove != null)
			{
				context.TimeEntries.Remove(entryToRemove);
			}
			await context.SaveChangesAsync();
		}
	}
}
