using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Service_Frm
{
    /// <summary>
    /// 风控用工器具
    /// </summary>
    public class bookEntity
    {

        /// <summary>
        /// 返回的信息
        /// </summary>
        public List<bookEntityData> records { get; set; }
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
    public class bookEntityData
    {
        public string useLocaleId { get; set; }//主键
        public string bookId { get; set; }//主键
        public string useOrgId { get; set; }//使用单位ID
        public string useOrg { get; set; }//使用单位
        public string useGqId { get; set; }//使用部门ID
        public string useGq { get; set; }//使用部门名称
        public string useBsId { get; set; }//使用专业室ID
        public string useBsName { get; set; }//使用专业室
        public string useBzId { get; set; }//使用班组id
        public string useBzName { get; set; }//使用班组
        public string buyDate { get; set; }//购置日期（格式:yyyy-MM-dd）
        public string lastTestTime { get; set; }//上次试验日期（格式:yyyy-MM-dd）
        public string nextTestTime { get; set; }//下次试验日期（格式:yyyy-MM-dd）
        public string testPeriod { get; set; }//试验周期（月）
        public string status { get; set; }//状态(0:在役;1:作废；,2:待审核 ；3:已退回 ；9：已删除)
        public string classId { get; set; }//工器具分类id
        public string gqjClass { get; set; }//工器具分类
        public string classifyId { get; set; }//工器具名称id
        public string gqjClassify { get; set; }//工器具名称
        public string gqjStandardId { get; set; }//规格型号id
        public string gqjStandard { get; set; }//规格型号
        /// <summary>
        /// 电压等级（
        //1：0.4kV;
        //2：1 kV;
        //3：3 kV;
        //4：6 kV；
        //5：10 kV；
        //6：20 kV；
        //7：35 kV；
        //8：66 kV；
        //9：110 kV；
        //10：220 kV；
        //11：330 kV；
        //12：500 kV；
        //13：750 kV；
        //14：1000 kV；
        //15：±500kV；
        //16：±660kV；
        //17：±800kV；
        //18：±1000kV；
        //19：100V；
        //20：0.4-10kV；
        //空则为无电压等级
        //）
        /// </summary>
        public string voltageGrade { get; set; }
        public string inUserId { get; set; }//入库扫描人ID
        public string inUserName { get; set; }//入库扫描人姓名
        public string createOrg { get; set; }//入库扫描单位
        public string createOrgId { get; set; }//入库扫描单位id
        public string facCode { get; set; }//生产厂家id
        public string facName { get; set; }//生产厂家
        public string perName { get; set; }//负责人
        public string perTell { get; set; }//负责人联系方式
        public string guaranteePeriod { get; set; }//质保年限
        public string factoryTime { get; set; }//出厂日期（格式:yyyy-MM-dd）
        public string pkUserLocaleId { get; set; }//保管地点ID
        public string useAddressName { get; set; }//保管地点名称
        public string blgXgsId { get; set; }//所属县ID
        public string blgSgsId { get; set; }//所属市ID
        public string blgXgs { get; set; }//所属县
        public string blgSgs { get; set; }//所属市
        public string pkTPlan { get; set; }//采购批次id
        public string planName { get; set; }//采购批次
        public string sort { get; set; }//是否为易耗品标识（是否是易耗品：0：不是；1：是）
        public string nodeStatus { get; set; }//节点状态（0：计划审批通过 ；1：二维码审批通过；2：验收通过）   

        public string createTime { get; set; }//入库时间（格式:yyyy-MM-dd）
        public string useRecordStatus { get; set; }//领用归还状态(0：领用；1：送检出库；2：归还或送检入库)  通过公共方法获取后改为（0：在库，1：正在使用，2：送检出库）
        public string flag { get; set; }//标志位（0：历史台账；1：新增台账）
        public string icode { get; set; }//实物ID
        public string factoryNum { get; set; }//工器具编号
        public string dataType{get;set; }//数据处理类型，增量类型(1:新增; 2:修改; 3:删除;)，全量时为空
        public string commentStatus { get; set; }//送检预警状态(0：试验超期；1：试验即将超期（7天）；2：试验要超期（30天）)
        public string comment { get; set; }//使用次数
        public string isPrint { get; set; }//是否可打印（0：否；1：是）

    }
}
