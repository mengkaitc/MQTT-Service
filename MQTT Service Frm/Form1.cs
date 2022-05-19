using MQTTnet;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MQTT_Service_Frm
{
    public partial class Form1 : Form
    {
        private static string _username = "";
        private static string _password = "";
        private static IMqttServer mqttServer = null;
        private delegate void addMsgDelegate(string msg);
        private static addMsgDelegate showMsg;

        public Form1()
        {
            InitializeComponent();
            showMsg = new addMsgDelegate(AddMsg);
        }
        /// <summary>
        /// 往listbox加一条项目
        /// </summary>
        /// <param name="msg"></param>
        void AddMsg(string msg)
        {
            if (this.listMsg.InvokeRequired)
            {
                // 很帅的调自己
                this.listMsg.Invoke(showMsg, msg);
            }
            else
            {
                if (this.listMsg.Items.Count > 100)
                {
                    this.listMsg.Items.RemoveAt(0);
                }
                this.listMsg.Items.Add(msg);
                this.listMsg.TopIndex = this.listMsg.Items.Count - (int)(this.listMsg.Height / this.listMsg.ItemHeight);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _username = username.Text.Trim();
            _password = password.Text.Trim();
            //MqttNetTrace.TraceMessagePublished += MqttNetTrace_TraceMessagePublished;
            MqttNetGlobalLogger.LogMessagePublished += MqttNetTrace_TraceMessagePublished;

            //new Thread(StartMqttServer).Start();
            Task.Run(
                () => {
                    StartMqttServer();
                }
            );

            //while (true)
            //{
            //    var inputString = Console.ReadLine().ToLower().Trim();

            //    if (inputString == "exit")
            //    {
            //        mqttServer?.StopAsync();
            //        Console.WriteLine("MQTT服务已停止！");
            //        break;
            //    }
            //    else if (inputString == "clients")
            //    {
            //        foreach (var item in mqttServer.GetConnectedClients())
            //        {
            //            Console.WriteLine($"客户端标识：{item.ClientId}，协议版本：{item.ProtocolVersion}");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine($"命令[{inputString}]无效！");
            //    }
            //}
        }
        private static void StartMqttServer()
        {
            if (mqttServer == null)
            {
                // Configure MQTT server.
                var optionsBuilder = new MqttServerOptionsBuilder()
                    .WithConnectionBacklog(100)
                    .WithDefaultEndpointPort(11883)
                    .WithConnectionValidator(ValidatingMqttClients())
                    ;

                // Start a MQTT server.
                mqttServer = new MqttFactory().CreateMqttServer();
                mqttServer.ApplicationMessageReceived += MqttServer_ApplicationMessageReceived;
                mqttServer.ClientConnected += MqttServer_ClientConnected;
                mqttServer.ClientDisconnected += MqttServer_ClientDisconnected;

                //Task.Run(async () => { await mqttServer.StartAsync(optionsBuilder.Build()); });
                mqttServer.StartAsync(optionsBuilder.Build()).Wait();
                showMsg("MQTT服务启动成功！");
            }



            //if (mqttServer == null)
            //{
            //    try
            //    {

            //        var options = new MqttServerOptions
            //        {
            //            ConnectionValidator = p =>
            //            {
            //                //if (p.ClientId == "c001")
            //                //{
            //                string timestamp = DateTime.Now.ToString("yyyyMMddHH");
            //                if (p.Username != _username || p.Password != EncryptUtil.HmacSHA256(_password, timestamp))
            //                {
            //                return MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
            //                }
            //                //}

            //                return MqttConnectReturnCode.ConnectionAccepted;
            //            }
            //        };

            //        mqttServer = new MqttServerFactory().CreateMqttServer(options) as MqttServer;
            //        mqttServer.ApplicationMessageReceived += MqttServer_ApplicationMessageReceived;
            //        mqttServer.ClientConnected += MqttServer_ClientConnected;//连接信息
            //        mqttServer.ClientDisconnected += MqttServer_ClientDisconnected;
            //    }
            //    catch (Exception ex)
            //    {
            //        showMsg(ex.Message);
            //        return;
            //    }
            //}

            //mqttServer.StartAsync();
            //showMsg("MQTT服务启动成功！");
        }
        private static Action<MqttConnectionValidatorContext> ValidatingMqttClients()
        {
            // Setup client validator.    
            var options = new MqttServerOptions();
            options.ConnectionValidator = c =>
            {
                Dictionary<string, string> c_u = new Dictionary<string, string>();
                c_u.Add("D108554502lQ4zA", "9BA19F778E664C05B1B5160081D15305");
                c_u.Add("client002", "username002");
                c_u.Add("D1142690798n0Se", "D039E0E009DD4D74ABF671792980D9EC");
                Dictionary<string, string> u_psw = new Dictionary<string, string>();
                u_psw.Add("9BA19F778E664C05B1B5160081D15305", "<AcMk1r&ysoq1&Hg2:oMA|)M");
                u_psw.Add("username002", "psw002");
                u_psw.Add("D039E0E009DD4D74ABF671792980D9EC", "Wtr0XH60M+&Ib)3-wTCd5fr:");
                string timestamp = DateTime.Now.ToString("yyyyMMddHH");
                if (c_u.ContainsKey(c.ClientId) && c_u[c.ClientId] == c.Username)
                {
                    if (u_psw.ContainsKey(c.Username) && u_psw[c.Username] == c.Password)//EncryptUtil.HmacSHA256(u_psw[c.Username], timestamp)
                    {
                        c.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
                    }
                    else
                    {
                        c.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                    }
                }
                else
                {
                    c.ReturnCode = MqttConnectReturnCode.ConnectionRefusedIdentifierRejected;
                }
            };
            return options.ConnectionValidator;
        }
        private static void MqttServer_ClientConnected(object sender, MqttClientConnectedEventArgs e)
        {
            showMsg($"客户端[{e.Client.ClientId}]已连接，协议版本：{e.Client.ProtocolVersion}");
        }

        private static void MqttServer_ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            showMsg($"客户端[{e.Client.ClientId}]已断开连接！");
        }

        private static void MqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            showMsg($"客户端[{e.ClientId}]>> 主题：{e.ApplicationMessage.Topic} 负荷：{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)} Qos：{e.ApplicationMessage.QualityOfServiceLevel} 保留：{e.ApplicationMessage.Retain}");
        }
        private static void MqttNetTrace_TraceMessagePublished(object sender, MqttNetLogMessagePublishedEventArgs e)
        {
            var trace = $">> [{e.TraceMessage.Timestamp:O}] [{e.TraceMessage.ThreadId}] [{e.TraceMessage.Source}] [{e.TraceMessage.Level}]: {e.TraceMessage.Message}";
            if (e.TraceMessage.Exception != null)
            {
                trace += Environment.NewLine + e.TraceMessage.Exception.ToString();
            }

            Console.WriteLine(trace);
        }
        //private static void MqttNetTrace_TraceMessagePublished(object sender, MqttNetTraceMessagePublishedEventArgs e)
        //{
        //    if (sender != null)
        //    {

        //    }
        //    /*Console.WriteLine($">> 线程ID：{e.ThreadId} 来源：{e.Source} 跟踪级别：{e.Level} 消息: {e.Message}");
        //    if (e.Exception != null)
        //    {
        //        Console.WriteLine(e.Exception);
        //    }*/
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            var appMsg2 = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(textBox2.Text), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //测试工器具
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "queryBook";
            bookEntity be = new bookEntity();
            be.current = 1;
            be.pages = 4;
            be.issueType = "2";
            List<bookEntityData> records = new List<bookEntityData>();
            for (int i = 0; i < 10; i++)
            {
                bookEntityData bd = new bookEntityData();
                if (i != 0 && i != 5)
                {
                    bd.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                bd.bookId = "测试bookid" + i;
                bd.useOrgId = "测试useOrgid" + i;
                bd.useOrg = "测试useOrg" + i;
                bd.useGqId = "测试useGqid" + i;
                bd.useGq = "测试useGq" + i;
                bd.useBsId = "测试useBsid" + i;
                bd.useBsName = "测试useBsid" + i;
                bd.inUserId = "测试inuserid" + i;
                bd.inUserName = "测试inusername" + i;
                bd.createOrg = "测试createorg" + i;
                bd.createOrgId = "测试createorgid" + i;
                bd.perName = "测试perName" + i;
                bd.perTell = "测试perTell" + i;
                bd.pkUserLocaleId = "测试pkUserlocaleid" + i;
                bd.useAddressName = "测试useAddressName" + i;
                bd.blgXgsId = "测试blgXgsid" + i;
                bd.blgXgs = "测试blgXgs" + i;
                bd.blgSgsId = "测试blgSgsid" + i;
                bd.blgSgs = "测试blgSgs" + i;
                bd.nodeStatus = "测试nodestatus" + i;
                bd.flag = "测试flag" + i;
                bd.isPrint = "测试isPrint" + i;
                bd.comment = "测试comment" + i;
                bd.classId = "测试classid" + i;
                bd.gqjClass = "测试gqjClass" + i;
                bd.classifyId = "测试classifyid" + i;
                bd.gqjClassify = "测试gqjClassify" + i;
                bd.gqjStandardId = "测试gqjStandardid" + i;
                bd.gqjStandard = "测试gqjStandard" + i;
                bd.voltageGrade = i.ToString();
                bd.pkTPlan = "测试pkTPlan" + i;
                bd.planName = "测试planName" + i;
                bd.facCode = "测试facCode" + i;
                bd.facName = "测试facName" + i;
                bd.factoryTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.buyDate = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.createTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.lastTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.nextTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.testPeriod = "测试testPeriod" + i;
                bd.useBzId = "useBanzid1";
                bd.useBzName = "测试useBzname1";
                bd.guaranteePeriod = "测试guaranteePeriod" + i;
                bd.icode = "icode" + i;
                bd.commentStatus = "测试commentStatus" + i;
                bd.sort = "测试sort" + i;
                bd.status = i.ToString();
                bd.useRecordStatus = "2";
                bd.factoryNum = "测试factoryNum" + i;
                bd.dataType = "";
                records.Add(bd);
            }
            be.records = records;
            mcom.paras = be;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.ExactlyOnce, false);
            mqttServer.PublishAsync(appMsg);

            mt_command mcom2 = new mt_command();
            mcom2.deviceId = tex_deviceId.Text.Trim();
            mcom2.cmd = "queryBook";
            bookEntity be2 = new bookEntity();
            be2.current = 2;
            be2.pages = 4;
            be2.issueType = "2";
            List<bookEntityData> records2 = new List<bookEntityData>();
            for (int i = 10; i < 20; i++)
            {
                bookEntityData bd = new bookEntityData();
                if (i != 10 && i != 15)
                {
                    bd.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                bd.bookId = "测试bookid" + i;
                bd.useOrgId = "测试useOrgid" + i;
                bd.useOrg = "测试useOrg" + i;
                bd.useGqId = "测试useGqid" + i;
                bd.useGq = "测试useGq" + i;
                bd.useBsId = "测试useBsid" + i;
                bd.inUserId = "测试inuserid" + i;
                bd.inUserName = "测试inusername" + i;
                bd.createOrg = "测试createorg" + i;
                bd.createOrgId = "测试createorgid" + i;
                bd.perName = "测试perName" + i;
                bd.perTell = "测试perTell" + i;
                bd.pkUserLocaleId = "测试pkUserlocaleid" + i;
                bd.useAddressName = "测试useAddressName" + i;
                bd.blgXgsId = "测试blgXgsid" + i;
                bd.blgXgs = "测试blgXgs" + i;
                bd.blgSgsId = "测试blgSgsid" + i;
                bd.blgSgs = "测试blgSgs" + i;
                bd.nodeStatus = "测试nodestatus" + i;
                bd.flag = "测试flag" + i;
                bd.isPrint = "测试isPrint" + i;
                bd.comment = "测试comment" + i;
                bd.classId = "测试classid" + i;
                bd.gqjClass = "测试gqjClass" + i;
                bd.classifyId = "测试classifyid" + i;
                bd.gqjClassify = "测试gqjClassify" + i;
                bd.gqjStandardId = "测试gqjStandardid" + i;
                bd.gqjStandard = "测试gqjStandard" + i;
                bd.voltageGrade = (i - 10).ToString();
                bd.pkTPlan = "测试pkTPlan" + i;
                bd.planName = "测试planName" + i;
                bd.facCode = "测试facCode" + i;
                bd.facName = "测试facName" + i;
                bd.factoryTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.buyDate = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.createTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.lastTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.nextTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.testPeriod = "测试testPeriod" + i;
                bd.useBzId = "useBanzid1";
                bd.useBzName = "测试useBzname1";
                bd.guaranteePeriod = "测试guaranteePeriod" + i;
                bd.icode = "icode" + i;
                bd.commentStatus = "测试commentStatus" + i;
                bd.sort = "测试sort" + i;
                bd.status = i.ToString();
                bd.useRecordStatus = "2";
                bd.dataType = "";
                records2.Add(bd);
            }
            be2.records = records2;
            mcom2.paras = be2;
            var appMsg2 = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom2)), MqttQualityOfServiceLevel.ExactlyOnce, false);
            mqttServer.PublishAsync(appMsg2);
            
            mqttServer.PublishAsync(appMsg2);

            mt_command mcom4 = new mt_command();
            mcom4.deviceId = tex_deviceId.Text.Trim();
            mcom4.cmd = "queryBook";
            bookEntity be4 = new bookEntity();
            be4.current = 3;
            be4.pages = 4;
            be4.issueType = "2";
            List<bookEntityData> records4 = new List<bookEntityData>();
            be4.records = records4;
            mcom4.paras = be4;
            var appMsg4 = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom4)), MqttQualityOfServiceLevel.ExactlyOnce, false);
            mqttServer.PublishAsync(appMsg4);



            mt_command mcom3 = new mt_command();
            mcom3.deviceId = tex_deviceId.Text.Trim();
            mcom3.cmd = "queryBook";
            bookEntity be3 = new bookEntity();
            be3.current = 4;
            be3.pages = 4;
            be3.issueType = "2";
            List<bookEntityData> records3 = new List<bookEntityData>();
            for (int i = 20; i < 30; i++)
            {
                bookEntityData bd = new bookEntityData();
                if (i != 20 && i != 25)
                {
                    bd.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                bd.bookId = "测试bookid" + i;
                bd.useOrgId = "测试useOrgid" + i;
                bd.useOrg = "测试useOrg" + i;
                bd.useGqId = "测试useGqid" + i;
                bd.useGq = "测试useGq" + i;
                bd.useBsId = "测试useBsid" + i;
                bd.inUserId = "测试inuserid" + i;
                bd.inUserName = "测试inusername" + i;
                bd.createOrg = "测试createorg" + i;
                bd.createOrgId = "测试createorgid" + i;
                bd.perName = "测试perName" + i;
                bd.perTell = "测试perTell" + i;
                bd.pkUserLocaleId = "测试pkUserlocaleid" + i;
                bd.useAddressName = "测试useAddressName" + i;
                bd.blgXgsId = "测试blgXgsid" + i;
                bd.blgXgs = "测试blgXgs" + i;
                bd.blgSgsId = "测试blgSgsid" + i;
                bd.blgSgs = "测试blgSgs" + i;
                bd.nodeStatus = "测试nodestatus" + i;
                bd.flag = "测试flag" + i;
                bd.isPrint = "测试isPrint" + i;
                bd.comment = "测试comment" + i;
                bd.classId = "测试classid" + i;
                bd.gqjClass = "测试gqjClass" + i;
                bd.classifyId = "测试classifyid" + i;
                bd.gqjClassify = "测试gqjClassify" + i;
                bd.gqjStandardId = "测试gqjStandardid" + i;
                bd.gqjStandard = "测试gqjStandard" + i;
                bd.voltageGrade = (i - 10).ToString();
                bd.pkTPlan = "测试pkTPlan" + i;
                bd.planName = "测试planName" + i;
                bd.facCode = "测试facCode" + i;
                bd.facName = "测试facName" + i;
                bd.factoryTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.buyDate = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.createTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.lastTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.nextTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.testPeriod = "测试testPeriod" + i;
                bd.useBzId = "useBanzid1";
                bd.useBzName = "测试useBzname1";
                bd.guaranteePeriod = "测试guaranteePeriod" + i;
                bd.icode = "icode" + i;
                bd.commentStatus = "测试commentStatus" + i;
                bd.sort = "测试sort" + i;
                bd.status = i.ToString();
                bd.useRecordStatus = "2";
                bd.dataType = "";
                records3.Add(bd);
            }
            be3.records = records3;
            mcom3.paras = be3;
            var appMsg3 = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom3)), MqttQualityOfServiceLevel.ExactlyOnce, false);
            mqttServer.PublishAsync(appMsg3);


            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "getUserOrg";
            organizationEntity oe = new organizationEntity();
            oe.current = 1;
            oe.pages = 1;
            oe.issueType = "2";
            List<organizationEntityData> records = new List<organizationEntityData>();
            for (int i = 0; i < 10; i++)
            {
                organizationEntityData oed = new organizationEntityData();
                if (i != 0 && i != 5)
                {
                    oed.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                if (i < 7)
                {
                    oed.useOrgId = "useOrgid" + i;
                    oed.useOrg = "useOrg" + i;
                    oed.useGqId = "useGqid" + i;
                    oed.userGq = "userGq" + i;
                    oed.useBsId = "useBsid" + i;
                    oed.useBsName = "useBsname" + i;
                    oed.useBanzId = "useBanzid" + i;
                    oed.useBanzName = "useBanzname" + i;
                    oed.blgXgsId = "blgXgsid" + i;
                    oed.blgXgs = "blgXgs" + i;
                    oed.blgSgsId = "blgSgsid" + i;
                    oed.blgSgs = "blgSgs" + i;
                    oed.dataType = "";
                    
                }
                else {
                    switch (i) {
                        case 7:
                            oed.useOrgId = "useOrgid" + i;
                            oed.useOrg = "useOrg" + i;
                            oed.useGqId = "useGqid" + i;
                            oed.userGq = "userGq" + i;
                            oed.useBsId = "useBsid" + i;
                            oed.useBsName = "useBsname" + i;
                            oed.useBanzId = "";
                            oed.useBanzName = "";
                            oed.blgXgsId = "blgXgsid" + i;
                            oed.blgXgs = "blgXgs" + i;
                            oed.blgSgsId = "blgSgsid" + i;
                            oed.blgSgs = "blgSgs" + i;
                            oed.dataType = "";
                            break;
                        case 8:
                            oed.useOrgId = "useOrgid" + i;
                            oed.useOrg = "useOrg" + i;
                            oed.useGqId = "useGqid" + i;
                            oed.userGq = "userGq" + i;
                            oed.useBsId = "";
                            oed.useBsName = "" ;
                            oed.useBanzId = "";
                            oed.useBanzName = "";
                            oed.blgXgsId = "blgXgsid" + i;
                            oed.blgXgs = "blgXgs" + i;
                            oed.blgSgsId = "blgSgsid" + i;
                            oed.blgSgs = "blgSgs" + i;
                            oed.dataType = "";
                            break;
                        case 9:
                            oed.useOrgId = "useOrgid" + i;
                            oed.useOrg = "useOrg" + i;
                            oed.useGqId = "";
                            oed.userGq = "";
                            oed.useBsId = "";
                            oed.useBsName = "";
                            oed.useBanzId = "";
                            oed.useBanzName = "";
                            oed.blgXgsId = "blgXgsid" + i;
                            oed.blgXgs = "blgXgs" + i;
                            oed.blgSgsId = "blgSgsid" + i;
                            oed.blgSgs = "blgSgs" + i;
                            oed.dataType = "";
                            break;
                    }
                }
                records.Add(oed);
            }
            oe.records = records;
            mcom.paras = oe;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "queryTask";
            jobPlanEntity je = new jobPlanEntity();
            je.current = 1;
            je.pages = 2;
            je.issueType = "2";
            List<jobPlanEntityData> records = new List<jobPlanEntityData>();
            for (int i = 0; i < 8; i++)
            {
                jobPlanEntityData jed = new jobPlanEntityData();
                if (i==3 || i == 7) {
                    jed.useBzId = "useBanzid9";
                }
                else if (i != 0 && i != 5)
                {
                    jed.useBzId = "useBanzid1";
                }
                jed.jobPlanId = "jobPlanId" + i;
                jed.jobName = "jobName" + i;
                jed.startTime = DateTime.Now.ToString("yyyy-MM-dd");
                jed.stopTime = DateTime.Now.ToString("yyyy-MM-dd");
                jed.jobContent = "jobContent" + i;
                jed.useBzName = "useBzname" + i;
                jed.dataType = "";
                records.Add(jed);
            }
            je.records = records;
            mcom.paras = je;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
            publishTask();
        }
        private void publishTask() {

            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "queryTask";
            jobPlanEntity je = new jobPlanEntity();
            je.current = 2;
            je.pages = 2;
            je.issueType = "2";
            List<jobPlanEntityData> records = new List<jobPlanEntityData>();
            for (int i = 8; i < 10; i++)
            {
                jobPlanEntityData jed = new jobPlanEntityData();
                if (i == 3 || i == 7)
                {
                    jed.useBzId = "useBanzid9";
                }
                else if (i != 0 && i != 5)
                {
                    jed.useBzId = "useBanzid1";
                }
                jed.jobPlanId = "jobPlanId" + i;
                jed.jobName = "jobName" + i;
                jed.startTime = DateTime.Now.ToString("yyyy-MM-dd");
                jed.stopTime = DateTime.Now.ToString("yyyy-MM-dd");
                jed.jobContent = "jobContent" + i;
                jed.useBzName = "useBzname" + i;
                jed.dataType = "";
                records.Add(jed);
            }
            je.records = records;
            mcom.paras = je;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "queryMechanism";
            sjjgEntity oe = new sjjgEntity();
            oe.current = 1;
            oe.pages = 1;
            oe.issueType = "2";
            List<sjjgEntityData> records = new List<sjjgEntityData>();
            for (int i = 0; i < 10; i++)
            {
                sjjgEntityData oed = new sjjgEntityData();
                
                oed.id = "id" + i;
                oed.mechanismId = "mechanismId" + i;
                oed.mechanismName = "mechanismName" + i;
                oed.path = "path" + i;
                oed.backup1 = "backup1" + i;
                oed.backup2 = "backup2" + i;
                oed.dataType = "";
                records.Add(oed);
            }
            oe.records = records;
            mcom.paras = oe;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "getbfinfolist";
            bfinfoEntity oe = new bfinfoEntity();
            oe.current = 1;
            oe.pages = 1;
            oe.issueType = "2";
            List<bfinfoEntityData> records = new List<bfinfoEntityData>();
            for (int i = 0; i < 10; i++)
            {
                bfinfoEntityData oed = new bfinfoEntityData();
                if (i != 0 && i != 5)
                {
                    oed.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                oed.bookId = "icode" + i;
                oed.pkScrapId = "pkScrapid" + i;
                oed.classid = "classid" + i;
                oed.gqjClass = "绝缘操作杆";
                oed.classifyid = "classifyid" + i;
                oed.userGq = "userGq" + i;
                oed.gqjClassify = "gqjClassify" + i;
                oed.gqjStandardid = "gqjStandardid" + i;
                oed.gqjStandard = "gqjStandard" + i;
                oed.factoryNum = "factoryNum" + i;
                oed.scrapReason = "scrapReason" + i;
                oed.scrapTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                oed.applyUser = "applyUser" + i;
                oed.applyTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                oed.buyDate = DateTime.Now.ToString("yyyy-MM-dd"); ;
                oed.planName = "planName" + i;
                oed.facName = "facName" + i;
                oed.icode = "icode" + i;
                oed.useBzId = "useBzid" + i;
                oed.useBzName = "useBzname" + i;
                oed.blgSgs = "1";
                oed.dataType = "";
                records.Add(oed);
            }
            oe.records = records;
            mcom.paras = oe;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "getUser";
            fkUserEntity je = new fkUserEntity();
            je.current = 1;
            je.pages = 2;
            je.issueType = "2";
            List<fkUserEntityData> records = new List<fkUserEntityData>();
            for (int i = 0; i < 10; i++)
            {
                fkUserEntityData jed = new fkUserEntityData();
                if (i == 3 || i == 7)
                {
                    jed.baseOrgUnitId = "useOrgid9   99999999";
                }
                else if (i != 0 && i != 5)
                {
                    jed.baseOrgUnitId = "useBanzid1    99999999";
                }
                jed.id = "id" + i;
                jed.pwd = "pwd" + i;
                jed.name = "name1" + i;
                jed.status = "1";
                jed.fullName = "fullName1" + i;
                jed.number = "number" + i;
                jed.phone = "phone" + i;
                jed.userImage = null;
                records.Add(jed);
            }
            je.records = records;
            mcom.paras = je;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtLeastOnce, false);
            mqttServer.PublishAsync(appMsg);
            user2();
        }
        void user2() {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "getUser";
            fkUserEntity je = new fkUserEntity();
            je.current = 2;
            je.pages = 2;
            je.issueType = "2";
            List<fkUserEntityData> records = new List<fkUserEntityData>();
            for (int i = 10; i < 20; i++)
            {
                fkUserEntityData jed = new fkUserEntityData();
                if (i == 3 || i == 7)
                {
                    jed.baseOrgUnitId = "useOrgid9";
                }
                else if (i != 0 && i != 5)
                {
                    jed.baseOrgUnitId = "useBanzid1";
                }
                jed.id = "id" + i;
                jed.pwd = "pwd" + i;
                jed.name = "name1" + i;
                jed.status = "1";
                jed.fullName = "22222fullName1" + i;
                jed.number = "number" + i;
                jed.phone = "phone" + i;
                jed.userImage = null;
                records.Add(jed);
            }
            je.records = records;
            mcom.paras = je;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtLeastOnce, false);
            mqttServer.PublishAsync(appMsg);


        }
        private void listMsg_MouseClick(object sender, MouseEventArgs e)
        {
            try {
                Clipboard.SetDataObject(listMsg.SelectedItem.ToString());
            }
            catch (Exception ex) {

            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //测试工器具
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "queryBook";
            bookEntity be = new bookEntity();
            be.current = 1;
            be.pages = 4;
            be.issueType = "1";
            List<bookEntityData> records = new List<bookEntityData>();
            for (int i = 0; i < 10; i++)
            {
                bookEntityData bd = new bookEntityData();
                if (i != 0 && i != 5)
                {
                    bd.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                bd.bookId = "测试bookid" + i;
                bd.useOrgId = "测试useOrgid" + i;
                bd.useOrg = "测试useOrg" + i;
                bd.useGqId = "测试useGqid" + i;
                bd.useGq = "测试useGq" + i;
                bd.useBsId = "测试useBsid" + i;
                bd.useBsName = "测试useBsid" + i;
                bd.inUserId = "测试inuserid" + i;
                bd.inUserName = "测试inusername" + i;
                bd.createOrg = "测试createorg" + i;
                bd.createOrgId = "测试createorgid" + i;
                bd.perName = "测试perName" + i;
                bd.perTell = "测试perTell" + i;
                bd.pkUserLocaleId = "测试pkUserlocaleid" + i;
                bd.useAddressName = "测试useAddressName" + i;
                bd.blgXgsId = "测试blgXgsid" + i;
                bd.blgXgs = "测试blgXgs" + i;
                bd.blgSgsId = "测试blgSgsid" + i;
                bd.blgSgs = "测试blgSgs" + i;
                bd.nodeStatus = "测试nodestatus" + i;
                bd.flag = "测试flag" + i;
                bd.isPrint = "测试isPrint" + i;
                bd.comment = "测试comment" + i;
                bd.classId = "测试classid" + i;
                bd.gqjClass = "测试gqjClass" + i;
                bd.classifyId = "测试classifyid" + i;
                bd.gqjClassify = "测试gqjClassify" + i;
                bd.gqjStandardId = "测试gqjStandardid" + i;
                bd.gqjStandard = "测试gqjStandard" + i;
                bd.voltageGrade = i.ToString();
                bd.pkTPlan = "测试pkTPlan" + i;
                bd.planName = "测试planName" + i;
                bd.facCode = "测试facCode" + i;
                bd.facName = "测试facName" + i;
                bd.factoryTime = "测试factoryTime" + i;
                bd.buyDate = "测试buyDate" + i;
                bd.createTime = "测试createtime" + i;
                bd.lastTestTime = "测试lastTestTime" + i;
                bd.nextTestTime = "测试nextTestTime" + i;
                bd.testPeriod = "测试testPeriod" + i;
                bd.useBzId = "测试useBzid" + i;
                bd.useBzName = "测试useBzname " + i;
                bd.guaranteePeriod = "测试guaranteePeriod" + i;
                bd.icode = "icode" + i;
                bd.commentStatus = "测试commentStatus" + i;
                bd.sort = "测试sort" + i;
                bd.status = i.ToString();
                bd.useRecordStatus = "2";
                bd.factoryNum = "测试factoryNum" + i;
                bd.dataType = "3";
                records.Add(bd);
            }
            be.records = records;
            mcom.paras = be;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);

            mt_command mcom2 = new mt_command();
            mcom2.deviceId = tex_deviceId.Text.Trim();
            mcom2.cmd = "queryBook";
            bookEntity be2 = new bookEntity();
            be2.current = 2;
            be2.pages = 4;
            be2.issueType = "1";
            List<bookEntityData> records2 = new List<bookEntityData>();
            for (int i = 10; i < 20; i++)
            {
                bookEntityData bd = new bookEntityData();
                if (i != 10 && i != 15)
                {
                    bd.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                bd.bookId = "测试bookid" + i;
                bd.useOrgId = "测试useOrgid" + i;
                bd.useOrg = "测试useOrg" + i;
                bd.useGqId = "测试useGqid" + i;
                bd.useGq = "测试useGq" + i;
                bd.useBsId = "测试useBsid" + i;
                bd.inUserId = "测试inuserid" + i;
                bd.inUserName = "测试inusername" + i;
                bd.createOrg = "测试createorg" + i;
                bd.createOrgId = "测试createorgid" + i;
                bd.perName = "测试perName" + i;
                bd.perTell = "测试perTell" + i;
                bd.pkUserLocaleId = "测试pkUserlocaleid" + i;
                bd.useAddressName = "测试useAddressName" + i;
                bd.blgXgsId = "测试blgXgsid" + i;
                bd.blgXgs = "测试blgXgs" + i;
                bd.blgSgsId = "测试blgSgsid" + i;
                bd.blgSgs = "测试blgSgs" + i;
                bd.nodeStatus = "测试nodestatus" + i;
                bd.flag = "测试flag" + i;
                bd.isPrint = "测试isPrint" + i;
                bd.comment = "测试comment" + i;
                bd.classId = "测试classid" + i;
                bd.gqjClass = "测试gqjClass" + i;
                bd.classifyId = "测试classifyid" + i;
                bd.gqjClassify = "测试gqjClassify" + i;
                bd.gqjStandardId = "测试gqjStandardid" + i;
                bd.gqjStandard = "测试gqjStandard" + i;
                bd.voltageGrade = (i - 10).ToString();
                bd.pkTPlan = "测试pkTPlan" + i;
                bd.planName = "测试planName" + i;
                bd.facCode = "测试facCode" + i;
                bd.facName = "测试facName" + i;
                bd.factoryTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.buyDate = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.createTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.lastTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.nextTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.testPeriod = "测试testPeriod" + i;
                bd.useBzId = "测试useBzid" + i;
                bd.useBzName = "测试useBzname " + i;
                bd.guaranteePeriod = "测试guaranteePeriod" + i;
                bd.icode = "icode" + i;
                bd.commentStatus = "测试commentStatus" + i;
                bd.sort = "测试sort" + i;
                bd.status = i.ToString();
                bd.useRecordStatus = "2";
                bd.dataType = "3";
                records2.Add(bd);
            }
            be2.records = records2;
            mcom2.paras = be2;
            var appMsg2 = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom2)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg2);

            mqttServer.PublishAsync(appMsg2);

            mt_command mcom4 = new mt_command();
            mcom4.deviceId = tex_deviceId.Text.Trim();
            mcom4.cmd = "queryBook";
            bookEntity be4 = new bookEntity();
            be4.current = 3;
            be4.pages = 4;
            List<bookEntityData> records4 = new List<bookEntityData>();
            be4.records = records4;
            mcom4.paras = be4;
            be4.issueType = "1";
            var appMsg4 = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom4)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg4);



            mt_command mcom3 = new mt_command();
            mcom3.deviceId = tex_deviceId.Text.Trim();
            mcom3.cmd = "queryBook";
            bookEntity be3 = new bookEntity();
            be3.current = 4;
            be3.pages = 4;
            be3.issueType = "1";
            List<bookEntityData> records3 = new List<bookEntityData>();
            for (int i = 20; i < 30; i++)
            {
                bookEntityData bd = new bookEntityData();
                if (i != 20 && i != 25)
                {
                    bd.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                bd.bookId = "测试bookid" + i;
                bd.useOrgId = "测试useOrgid" + i;
                bd.useOrg = "测试useOrg" + i;
                bd.useGqId = "测试useGqid" + i;
                bd.useGq = "测试useGq" + i;
                bd.useBsId = "测试useBsid" + i;
                bd.inUserId = "测试inuserid" + i;
                bd.inUserName = "测试inusername" + i;
                bd.createOrg = "测试createorg" + i;
                bd.createOrgId = "测试createorgid" + i;
                bd.perName = "测试perName" + i;
                bd.perTell = "测试perTell" + i;
                bd.pkUserLocaleId = "测试pkUserlocaleid" + i;
                bd.useAddressName = "测试useAddressName" + i;
                bd.blgXgsId = "测试blgXgsid" + i;
                bd.blgXgs = "测试blgXgs" + i;
                bd.blgSgsId = "测试blgSgsid" + i;
                bd.blgSgs = "测试blgSgs" + i;
                bd.nodeStatus = "测试nodestatus" + i;
                bd.flag = "测试flag" + i;
                bd.isPrint = "测试isPrint" + i;
                bd.comment = "测试comment" + i;
                bd.classId = "测试classid" + i;
                bd.gqjClass = "测试gqjClass" + i;
                bd.classifyId = "测试classifyid" + i;
                bd.gqjClassify = "测试gqjClassify" + i;
                bd.gqjStandardId = "测试gqjStandardid" + i;
                bd.gqjStandard = "测试gqjStandard" + i;
                bd.voltageGrade = (i - 10).ToString();
                bd.pkTPlan = "测试pkTPlan" + i;
                bd.planName = "测试planName" + i;
                bd.facCode = "测试facCode" + i;
                bd.facName = "测试facName" + i;
                bd.factoryTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.buyDate = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.createTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.lastTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.nextTestTime = DateTime.Now.ToString("yyyy-MM-dd"); ;
                bd.testPeriod = "测试testPeriod" + i;
                bd.useBzId = "测试useBzid" + i;
                bd.useBzName = "测试useBzname " + i;
                bd.guaranteePeriod = "测试guaranteePeriod" + i;
                bd.icode = "icode" + i;
                bd.commentStatus = "测试commentStatus" + i;
                bd.sort = "测试sort" + i;
                bd.status = i.ToString();
                bd.useRecordStatus = "2";
                bd.dataType = "3";
                bd.factoryNum = "测试factoryNum" + i;
                records3.Add(bd);
            }
            be3.records = records3;
            mcom3.paras = be3;
            var appMsg3 = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom3)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg3);

        }

        private void button11_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "getUserOrg";
            organizationEntity oe = new organizationEntity();
            oe.current = 1;
            oe.pages = 1;
            oe.issueType = "1";
            List<organizationEntityData> records = new List<organizationEntityData>();
            for (int i = 0; i < 10; i++)
            {
                organizationEntityData oed = new organizationEntityData();
                if (i != 0 && i != 5)
                {
                    oed.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                if (i < 7)
                {
                    oed.useOrgId = "useOrgid" + i;
                    oed.useOrg = "useOrg" + i;
                    oed.useGqId = "useGqid" + i;
                    oed.userGq = "userGq" + i;
                    oed.useBsId = "useBsid" + i;
                    oed.useBsName = "useBsname" + i;
                    oed.useBanzId = "useBanzid" + i;
                    oed.useBanzName = "useBanzname" + i;
                    oed.blgXgsId = "blgXgsid" + i;
                    oed.blgXgs = "blgXgs" + i;
                    oed.blgSgsId = "blgSgsid" + i;
                    oed.blgSgs = "blgSgs" + i;
                    oed.dataType = "3";

                }
                else
                {
                    switch (i)
                    {
                        case 7:
                            oed.useOrgId = "useOrgid" + i;
                            oed.useOrg = "useOrg" + i;
                            oed.useGqId = "useGqid" + i;
                            oed.userGq = "userGq" + i;
                            oed.useBsId = "useBsid" + i;
                            oed.useBsName = "useBsname" + i;
                            oed.useBanzId = "";
                            oed.useBanzName = "";
                            oed.blgXgsId = "blgXgsid" + i;
                            oed.blgXgs = "blgXgs" + i;
                            oed.blgSgsId = "blgSgsid" + i;
                            oed.blgSgs = "blgSgs" + i;
                            oed.dataType = "3";
                            break;
                        case 8:
                            oed.useOrgId = "useOrgid" + i;
                            oed.useOrg = "useOrg" + i;
                            oed.useGqId = "useGqid" + i;
                            oed.userGq = "userGq" + i;
                            oed.useBsId = "";
                            oed.useBsName = "";
                            oed.useBanzId = "";
                            oed.useBanzName = "";
                            oed.blgXgsId = "blgXgsid" + i;
                            oed.blgXgs = "blgXgs" + i;
                            oed.blgSgsId = "blgSgsid" + i;
                            oed.blgSgs = "blgSgs" + i;
                            oed.dataType = "3";
                            break;
                        case 9:
                            oed.useOrgId = "useOrgid" + i;
                            oed.useOrg = "useOrg" + i;
                            oed.useGqId = "";
                            oed.userGq = "";
                            oed.useBsId = "";
                            oed.useBsName = "";
                            oed.useBanzId = "";
                            oed.useBanzName = "";
                            oed.blgXgsId = "blgXgsid" + i;
                            oed.blgXgs = "blgXgs" + i;
                            oed.blgSgsId = "blgSgsid" + i;
                            oed.blgSgs = "blgSgs" + i;
                            oed.dataType = "3";
                            break;
                    }
                }
                records.Add(oed);
            }
            oe.records = records;
            mcom.paras = oe;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "queryTask";
            jobPlanEntity je = new jobPlanEntity();
            je.current = 1;
            je.pages = 1;
            je.issueType = "1";
            List<jobPlanEntityData> records = new List<jobPlanEntityData>();
            for (int i = 0; i < 10; i++)
            {
                jobPlanEntityData jed = new jobPlanEntityData();
                if (i == 3 || i == 7)
                {
                    jed.useBzId = "useBanzid9";
                }
                else if (i != 0 && i != 5)
                {
                    jed.useBzId = "useBanzid1";
                }
                jed.jobPlanId = "jobPlanId" + i;
                jed.jobName = "jobName" + i;
                jed.startTime = DateTime.Now.ToString("yyyy-MM-dd");
                jed.stopTime = DateTime.Now.ToString("yyyy-MM-dd");
                jed.jobContent = "jobContent" + i;
                jed.useBzName = "useBzname" + i;
                jed.dataType = "3";
                records.Add(jed);
            }
            je.records = records;
            mcom.paras = je;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "queryMechanism";
            sjjgEntity oe = new sjjgEntity();
            oe.current = 1;
            oe.pages = 1;
            oe.issueType = "1";
            List<sjjgEntityData> records = new List<sjjgEntityData>();
            for (int i = 0; i < 10; i++)
            {
                sjjgEntityData oed = new sjjgEntityData();

                oed.id = "id" + i;
                oed.mechanismId = "mechanismId" + i;
                oed.mechanismName = "mechanismName" + i;
                oed.path = "path" + i;
                oed.backup1 = "backup1" + i;
                oed.backup2 = "backup2" + i;
                oed.dataType = "3";
                records.Add(oed);
            }
            oe.records = records;
            mcom.paras = oe;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "getbfinfolist";
            bfinfoEntity oe = new bfinfoEntity();
            oe.current = 1;
            oe.pages = 1;
            oe.issueType = "1";
            List<bfinfoEntityData> records = new List<bfinfoEntityData>();
            for (int i = 0; i < 10; i++)
            {
                bfinfoEntityData oed = new bfinfoEntityData();
                if (i != 0 && i != 5)
                {
                    oed.useLocaleId = tex_useLocaleId.Text.Trim();
                }
                oed.bookId = "bookid" + i;
                oed.pkScrapId = "pkScrapid" + i;
                oed.classid = "classid" + i;
                oed.gqjClass = "绝缘操作杆";
                oed.classifyid = "classifyid" + i;
                oed.userGq = "userGq" + i;
                oed.gqjClassify = "gqjClassify" + i;
                oed.gqjStandardid = "gqjStandardid" + i;
                oed.gqjStandard = "gqjStandard" + i;
                oed.factoryNum = "factoryNum" + i;
                oed.scrapReason = "scrapReason" + i;
                oed.scrapTime = DateTime.Now.ToString("yyyy-MM-dd");
                oed.applyUser = "applyUser" + i;
                oed.applyTime = DateTime.Now.ToString("yyyy-MM-dd");
                oed.buyDate = DateTime.Now.ToString("yyyy-MM-dd");
                oed.planName = "planName" + i;
                oed.facName = "facName" + i;
                oed.icode = "icode" + i;
                oed.useBzId = "useBzid" + i;
                oed.useBzName = "useBzname" + i;
                oed.blgSgs = "1";
                oed.dataType = "3";
                records.Add(oed);
            }
            oe.records = records;
            mcom.paras = oe;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            for (int i = 0;i<500;i++) {
                test(i+1, 500);
            }
        }
        public async void test(int current, int pages) {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "queryTask";
            jobPlanEntity je = new jobPlanEntity();
            je.current = current;
            je.pages = pages;
            je.issueType = "2";
            List<jobPlanEntityData> records = new List<jobPlanEntityData>();
            for (int i = 0; i < 10; i++)
            {
                jobPlanEntityData jed = new jobPlanEntityData();
                if (i == 3 || i == 7)
                {
                    jed.useBzId = "useBanzid9";
                }
                else if (i != 0 && i != 5)
                {
                    jed.useBzId = "useBanzid1";
                }
                jed.jobPlanId = "jobPlanId" + i;
                jed.jobName = "jobName" + i;
                jed.startTime = DateTime.Now.ToString("yyyy-MM-dd");
                jed.stopTime = DateTime.Now.ToString("yyyy-MM-dd");
                jed.jobContent = "jobContent" + i;
                jed.useBzName = "useBzname" + i;
                jed.dataType = "";
                records.Add(jed);
            }
            je.records = records;
            mcom.paras = je;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }
        private void button16_Click(object sender, EventArgs e)
        {
            listMsg.Items.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            mt_command mcom = new mt_command();
            mcom.deviceId = tex_deviceId.Text.Trim();
            mcom.cmd = "getUser";
            fkUserEntity je = new fkUserEntity();
            je.current = 1;
            je.pages = 1;
            je.issueType = "1";
            List<fkUserEntityData> records = new List<fkUserEntityData>();
            for (int i = 0; i < 10; i++)
            {
                fkUserEntityData jed = new fkUserEntityData();
                if (i == 3 || i == 7)
                {
                    jed.baseOrgUnitId = "useBzid9";
                }
                else if (i != 0 && i != 5)
                {
                    jed.baseOrgUnitId = "useBzid1";
                }
                jed.id = "id" + i;
                jed.pwd = "pwd" + i;
                jed.name = "name" + i;
                jed.status = "1";
                jed.fullName = "fullName" + i;
                jed.number = "number" + i;
                jed.phone = "phone" + i;
                jed.userImage = null;
                records.Add(jed);
            }
            je.records = records;
            mcom.paras = je;
            var appMsg = new MqttApplicationMessage(textBox1.Text, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mcom)), MqttQualityOfServiceLevel.AtMostOnce, false);
            mqttServer.PublishAsync(appMsg);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tex_useLocaleId_TextChanged(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            byte[] arr = Convert.FromBase64String(textBox3.Text);
            string str = System.Text.Encoding.UTF8.GetString(arr);
            textBox4.Text = str;
        }
    }
}
