using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Service_Frm
{
    public class bfinfoEntity
    {

        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<bfinfoEntityData> records { get; set; }
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
    public class bfinfoEntityData
    {
        public string useLocaleId { get; set; }//库房id
        /// <summary>
        /// 主键
        /// </summary>
        public string bookId { get; set; }
        /// <summary>
        /// 实物id
        /// </summary>
        public string icode { get; set; }
        /// <summary>
        /// 备用
        /// </summary>
        public string pkScrapId { get; set; }
        public string scrapNum { get; set; }
        /// <summary>
        /// 报废原因
        /// </summary>
        public string scrapReason { get; set; }
        /// <summary>
        /// 报废时间（格式:yyyy-MM-dd）
        /// </summary>
        public string scrapTime { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string applyUser { get; set; }
        /// <summary>
        /// 申请时间（格式:yyyy-MM-dd）
        /// </summary>
        public string applyTime { get; set; }
        /// <summary>
        /// 购置时间（格式:yyyy-MM-dd）
        /// </summary>
        public string buyDate { get; set; }
        /// <summary>
        /// 采购批次
        /// </summary>
        public string planName { get; set; }
        public string userGq { get; set; }
        public string useBsname { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        public string useBzName { get; set; }
        /// <summary>
        /// 班组id
        /// </summary>
        public string useBzId { get; set; }
        /// <summary>
        /// 工器具分类id
        /// </summary>
        public string classid { get; set; }

        /// <summary>
        /// 工器具分类
        /// </summary>
        public string gqjClass { get; set; }
        /// <summary>
        /// 工器具名称id
        /// </summary>
        public string classifyid { get; set; }
        /// <summary>
        /// 工器具名称
        /// </summary>
        public string gqjClassify { get; set; }
        /// <summary>
        /// 工器具规格id
        /// </summary>
        public string gqjStandardid { get; set; }
        /// <summary>
        /// 工器具规格
        /// </summary>
        public string gqjStandard { get; set; }
        /// <summary>
        /// 工器具编号
        /// </summary>
        public string factoryNum { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string facName { get; set; }

        public string blgXgs { get; set; }
        public string blgSgs { get; set; }
        /// <summary>
        /// 是否是易耗品（是否是易耗品：0：不是；1：是）
        /// </summary>
        public string isplan { get; set; }
        public string path { get; set; }
        public string status { get; set; }
        /// <summary>
        /// 数据处理类型，增量类型(1:新增; 2:修改; 3:删除;)，全量时为空
        /// </summary>
        public string dataType { get; set; }

    }
}
