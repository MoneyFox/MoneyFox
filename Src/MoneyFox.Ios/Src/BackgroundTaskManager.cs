using System;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Ios
{
	public class BackgroundTaskManager : IBackgroundTaskManager
	{
		public void StartBackgroundTask()
		{
		}

	    public void StopBackgroundTasks()
	    {
	        //throw new NotImplementedException();
	    }

	    public void StartBackgroundTasks()
	    {
	        //throw new NotImplementedException();
	    }

	    public void StartBackupSyncTask()
	    {
	        throw new NotImplementedException();
	    }

	    public void StopBackupSyncTask()
	    {
	        throw new NotImplementedException();
	    }
	}
}
