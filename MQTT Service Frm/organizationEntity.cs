using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Service_Frm
{
    public class organizationEntity
    {
        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<organizationEntityData> records { get; set; }
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
    public class organizationEntityPT
    {

        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<organizationEntityData> data { get; set; }
        /// <summary>
        /// 响应信息
        /// </summary>
        public string result { get; set; }
        public string code { get; set; }
    }
    public class organizationEntityData
    {
        public string useLocaleId { get; set; }//库房id
        /// <summary>
        /// 库房所在组织机构id
        /// </summary>
        public string useBzid { get; set; }
        /// <summary>
        /// 库房所在组织机构名称
        /// </summary>
        public string useBzname { get; set; }
        /// <summary>
        /// 使用单位id
        /// </summary>
        public string useOrgId { get; set; }
        /// <summary>
        /// 使用单位名称
        /// </summary>
        public string useOrg { get; set; }
        /// <summary>
        /// 使用部门id
        /// </summary>
        public string useGqId { get; set; }
        /// <summary>
        /// 使用部门名称
        /// </summary>
        public string userGq { get; set; }

        /// <summary>
        /// 使用专业室id
        /// </summary>
        public string useBsId { get; set; }
        /// <summary>
        /// 使用专业室
        /// </summary>
        public string useBsName { get; set; }
        /// <summary>
        /// 使用班组id
        /// </summary>
        public string useBanzId { get; set; }
        /// <summary>
        /// 使用班组名称
        /// </summary>
        public string useBanzName { get; set; }
        /// <summary>
        /// 所属县id
        /// </summary>
        public string blgXgsId { get; set; }
        /// <summary>
        /// 所属县
        /// </summary>
        public string blgXgs { get; set; }
        /// <summary>
        /// 所属市id
        /// </summary>
        public string blgSgsId { get; set; }
        /// <summary>
        /// 所属市
        /// </summary>
        public string blgSgs { get; set; }
        /// <summary>
        /// 数据处理类型，增量类型(1:新增; 2:修改; 3:删除;)，全量时为空
        /// </summary>
        public string dataType { get; set; }

    }
}
