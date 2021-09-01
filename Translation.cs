//using Newtonsoft.Json;
using System.Collections.Generic;
//using System.IO;
namespace Plugin
{
    public class Translation
    {
        public  Dictionary<string, string> language = new Dictionary<string, string>();
        public static Translation Load(string path)
        {
            Translation translation = new Translation();
            translation.language.Add("SuccessfullyDelete", "删除成功!");
            translation.language.Add("SuccessfullyAdd", "添加成功!");
            translation.language.Add("SuccessfullyEnable", "启用成功!");
            translation.language.Add("SuccessfullyDisable", "禁用成功!");
            translation.language.Add("SuccessfullyReload", "重载成功!");
            translation.language.Add("FailedAdd", "添加失败! 该玩家已经在名单中");
            translation.language.Add("FailedDelete", "删除失败 ! 该玩家不在名单中");
            translation.language.Add("FailedEnable", "启用失败 ! 插件已打开");
            translation.language.Add("FailedDisable", "禁用失败 ! 插件已关闭");
            translation.language.Add("DisconnectHour", "『好好学习』今日游戏时间已到！");
            translation.language.Add("DisconnectWeek", "『好好学习』周五、周六、周天 的 20~21点才能进入！");
            translation.language.Add("HelpText", "用法: 输入 /gs help 显示帮助信息.");
            translation.language.Add("NotEnabled", "插件已被禁用，请检查配置文件!");
            translation.language.Add("AllHelpText", "『好好学习』插件帮助\n/gs help, 显示帮助信息\n/gs add {name}, 添加玩家名到名单中\n/gs del {name}, 将玩家移出名单\n/gs list, 显示名单上的全部玩家\n/gs true, 启用插件\n/gs false, 停用插件\n/gs reload, 重载插件");
            return translation;
        }
    }
}
