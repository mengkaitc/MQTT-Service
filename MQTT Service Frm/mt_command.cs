using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Service_Frm
{
    /// <summary>
    /// 物联网平台给设备或边设备下发命令
    /// </summary>
    public class mt_command
    {
        public string deviceId { get; set; }//平台生成的设备唯一标识
        public string msgType { get; set; } = "cloudReq";//固定值"cloudReq"，表示平台下发的请求。

        public string serviceId { get; set; }//服务的 id。
        public string cmd { get; set; }//服务的命令名
        public object paras { get; set; }//命令的参数
        public int? mid { get; set; }//命令 id
    }
}
