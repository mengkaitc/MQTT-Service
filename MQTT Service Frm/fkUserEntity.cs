using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Service_Frm
{
    public class fkUserEntity
    {
        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<fkUserEntityData> records { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int? current { get; set; }//
        /// <summary>
        /// 总计页数
        /// </summary>
        public int? pages { get; set; }//
        /// <summary>
        /// 每页条数
        /// </summary>
        public int? size { get; set; }
        /// <summary>
        /// 总计条数
        /// </summary>
        public int? total { get; set; }
        /// <summary>
        /// 下发类型(1：增量;2：全量)
        /// </summary>
        public string issueType { get; set; }
    }
    public class fkUserEntityData
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 用户密码(暂不提供)
        /// </summary>
        public string pwd { get; set; }
        /// <summary>
        /// 直属组织机构id
        /// </summary>
        public string baseOrgUnitId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string fullName { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// 用户电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 用户图片(暂不提供)
        /// </summary>
        public string userImage { get; set; }
    }
}
