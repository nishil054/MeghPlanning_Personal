using Abp;
using Abp.Dependency;
using Abp.Localization;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Notifications
{
	public class MyService : ITransientDependency
	{
		private readonly INotificationPublisher _notificationPublisher;

		public MyService(INotificationPublisher notificationPublisher)
		{
			_notificationPublisher = notificationPublisher;
		}

		//Send a general notification to a specific user
		//public async Task Publish_SentFrendshipRequest(string senderUserName, string friendshipMessage, UserIdentifier targetUserId)
		//{
		//	await _notificationPublisher.PublishAsync("SentFrendshipRequest", new SentFrendshipRequestNotificationData(senderUserName, friendshipMessage), userIds: new[] { targetUserId });
		//}

		//Send an entity notification to a specific user
		//public async Task Publish_CommentPhoto(string commenterUserName, string comment, Guid photoId, UserIdentifier photoOwnerUserId)
		//{
		//	await _notificationPublisher.PublishAsync("CommentPhoto", new CommentPhotoNotificationData(commenterUserName, comment), new EntityIdentifier(typeof(Photo), photoId), userIds: new[] { photoOwnerUserId });
		//}

		//Send a general notification to all subscribed users in current tenant (tenant in the session)
		public async Task Publish_LowDisk(int remainingDiskInMb)
		{
			//Example "LowDiskWarningMessage" content for English -> "Attention! Only {remainingDiskInMb} MBs left on the disk!"
			var data = new LocalizableMessageNotificationData(new LocalizableString("LowDiskWarningMessage", "MyLocalizationSourceName"));
			data["remainingDiskInMb"] = remainingDiskInMb;

			await _notificationPublisher.PublishAsync("System.LowDisk", data, severity: NotificationSeverity.Warn);
		}
		//public async Task Publish_AddCountry(string message)
		//{
		//	//Example "LowDiskWarningMessage" content for English -> "Attention! Only {remainingDiskInMb} MBs left on the disk!"
		//	var data = new LocalizableMessageNotificationData(new LocalizableString("LowDiskWarningMessage", "MyLocalizationSourceName"));
		//	data["remainingDiskInMb"] = message;

		//	await _notificationPublisher.PublishAsync("System.LowDisk", data, severity: NotificationSeverity.Warn);
		//}
	}
}
