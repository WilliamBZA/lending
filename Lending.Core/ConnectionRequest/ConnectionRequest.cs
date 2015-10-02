using System;

namespace Lending.Core.ConnectionRequest
{
    public class ConnectionRequest
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }

        public ConnectionRequest()
        {
            
        }

        public ConnectionRequest(Guid fromUserId, Guid toUserId)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
        }
    }
}