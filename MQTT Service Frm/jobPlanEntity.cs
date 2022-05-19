using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Service_Frm
{
    public class jobPlanEntity
    {
        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<jobPlanEntityData> records { get; set; }
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
    public class jobPlanEntityPT
    {
        /// <summary>
        /// 响应状态
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<jobPlanEntityData> data { get; set; }
        /// <summary>
        /// 响应信息
        /// </summary>
        public string resultHint { get; set; }
        public string errorPage { get; set; }
        public string type { get; set; }
        
    }
    public class jobPlanEntityData
    {
        /// <summary>
        /// 工作任务id
        /// </summary>
        public string jobPlanId { get; set; }
        /// <summary>
        /// 工作任务名称
        /// </summary>
        public string jobName { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string jobContent { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string stopTime { get; set; }
        /// <summary>
        /// 组织机构id
        /// </summary>
        public string useBzId { get; set; }
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string useBzName { get; set; }
        /// <summary>
        /// 数据处理类型，增量类型(1:新增; 2:修改; 3:删除;)，全量时为空
        /// </summary>
        public string dataType { get; set; }

    }
}
