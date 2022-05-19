using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Service_Frm
{
    public class sjjgEntity
    {

        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<sjjgEntityData> records { get; set; }
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
    public class sjjgEntityPT
    {
        /// <summary>
        /// 响应状态
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<sjjgEntityData> data { get; set; }

        public string code { get; set; }
    }
    public class sjjgEntityData
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string mechanismId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string mechanismName { get; set; }

        /// <summary>
        /// 数据处理类型，增量类型(1:新增; 2:修改; 3:删除;)，全量时为空
        /// </summary>
        public string dataType { get;set; }
        /// <summary>
        /// 路径(备用)
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 备用字段1
        /// </summary>
        public string backup1 { get; set; }
        /// <summary>
        /// 备用字段2
        /// </summary>
        public string backup2 { get; set; }
    }
}
