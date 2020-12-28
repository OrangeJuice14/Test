using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace HRMWebApp.Hubs
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        public void LaySoDangKyNghiDangChoXet()
        {
            Clients.All.capNhatThongBaoNghiPhep();
        }
        public void LaySoCongTacDangChoXet()
        {
            Clients.All.capNhatThongBaoCongTac();
        }
    }
}