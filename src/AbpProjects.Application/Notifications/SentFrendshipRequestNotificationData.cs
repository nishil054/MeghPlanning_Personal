using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Notifications
{
	[Serializable]
	public class SentFrendshipRequestNotificationData : NotificationData
	{
		public string SenderUserName { get; set; }

		public string FriendshipMessage { get; set; }

		public SentFrendshipRequestNotificationData(string senderUserName, string friendshipMessage)
		{
			SenderUserName = senderUserName;
			FriendshipMessage = friendshipMessage;
		}
	}
}
