using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Newtonsoft.Json;

namespace Plugin
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        #region Plugin Info
        public override string Author => "hufang360";
        public override string Description => "好好学习";
        public override string Name => "GoodStudy";
        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        #endregion
        public static Config _config;
        public static Translation _translation;
        public static string config_path = Path.Combine(TShock.SavePath, "GoodStudyConfig.json");
        public static string translation_path = Path.Combine(TShock.SavePath, "GoodStudyLanguage.json");

        public Plugin(Main game) : base(game)
        {

        }

        #region Initialize/Dispose
        public override void Initialize()
        {
            Load();

            Commands.ChatCommands.Add(new Command(new List<string>() { "goodstudy" }, GoodStudy, "goodstudy", "gs") { HelpText = "好好学习" });
            ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
			ServerApi.Hooks.GameUpdate.Register(this, OnGameUpdate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerJoin.Deregister(this, OnServerJoin);
		    	ServerApi.Hooks.GameUpdate.Deregister(this, OnGameUpdate);
            }
            base.Dispose(disposing);
        }
        #endregion

        private void Load() {
            _config = Config.Load(config_path);
            _translation = Translation.Load(translation_path);
            File.WriteAllText(config_path, JsonConvert.SerializeObject(_config, Formatting.Indented));
            File.WriteAllText(translation_path, JsonConvert.SerializeObject(_translation, Formatting.Indented));
        }
        private void OnServerJoin(JoinEventArgs args)
        {
            if (!_config.enable)
            {
                return;
            }

            DateTime now = DateTime.Now;
            bool hourPass = false;
            bool weekPass = false;
            if( now.Hour==20 || now.Hour == 21){
                hourPass =  true;
            }
            if( now.DayOfWeek == DayOfWeek.Friday
                || now.DayOfWeek == DayOfWeek.Saturday
                || now.DayOfWeek == DayOfWeek.Sunday )
            {
                weekPass = true;
            }

            var player = TShock.Players[args.Who];
            if ( _config.names.Contains(player.Name) )
            {
                if( !weekPass ){
                    player.Disconnect(_translation.language["DisconnectWeek"]);
                } else {
                    if ( !hourPass ){
                        player.Disconnect(_translation.language["DisconnectHour"]);
                    }
                }
            }

        }

        private void OnGameUpdate(EventArgs args)
        {
            if ( !_config.enable)
            {
                return;
            }

            DateTime now = DateTime.Now;
            bool hourPass = false;
            bool weekPass = false;
            if( now.Hour==20 || now.Hour == 21)
            {
                hourPass =  true;
            }

            if( now.DayOfWeek == DayOfWeek.Friday
                || now.DayOfWeek == DayOfWeek.Saturday
                || now.DayOfWeek == DayOfWeek.Sunday )
            {
                weekPass = true;
            }

            foreach(TSPlayer player in TShock.Players){
                if ( player != null &&_config.names.Contains(player.Name) ){
                    Console.WriteLine("hourPass:"+hourPass);
                    Console.WriteLine("weekPass:"+weekPass);

                    if( !weekPass ){
                        player.Disconnect(_translation.language["DisconnectWeek"]);
                    } else {
                        if ( !hourPass ){
                            player.Disconnect(_translation.language["DisconnectHour"]);
                        }
                    }
                }
            }
        }

        private bool CheckTime(){
            DateTime now = DateTime.Now;
            bool hourPass = false;
            bool weekPass = false;
            if( now.Hour==20 || now.Hour == 21){
                hourPass =  true;
            }
            if( now.DayOfWeek == DayOfWeek.Friday
                || now.DayOfWeek == DayOfWeek.Saturday
                || now.DayOfWeek == DayOfWeek.Sunday )
            {
                weekPass = true;
            }
            return hourPass && weekPass;
        }

        private void GoodStudy(CommandArgs args)
        {
            if (args.Parameters.Count<string>() == 0)
            {
                args.Player.SendErrorMessage(_translation.language["HelpText"]);
                return;
            }

            switch (args.Parameters[0].ToLowerInvariant())
            {
                default:
                    args.Player.SendErrorMessage(_translation.language["HelpText"]);
                    break;

                //  显示帮助
                case "help":
                    args.Player.SendErrorMessage(_translation.language["AllHelpText"]);
                    break;

                //  重载
                case "reload":
                    _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(config_path));
                    _translation = JsonConvert.DeserializeObject<Translation>(File.ReadAllText(translation_path));
                    args.Player.SendSuccessMessage(_translation.language["SuccessfullyReload"]);
                    break;

                // 名单
                case "list":
                    foreach (var name in _config.names)
                    {
                        args.Player.SendInfoMessage(name);
                    }
                    break;

                // 添加
                case "add":
                    if( _config.enable ){
                        if (_config.names.Contains(args.Parameters[1]))
                        {
                            args.Player.SendSuccessMessage(_translation.language["FailedAdd"]);
                        } else {
                            _config.names.Add(args.Parameters[1]);
                            args.Player.SendSuccessMessage(_translation.language["SuccessfullyAdd"]);
                            File.WriteAllText(config_path, JsonConvert.SerializeObject(_config, Formatting.Indented));
                        }
                    } else {
                        args.Player.SendErrorMessage(_translation.language["NotEnabled"]);
                    }
                    break;

                // 删除
                case "del":
                    if( _config.enable ){
                        if (_config.names.Contains(args.Parameters[1]))
                        {
                            _config.names.Remove(args.Parameters[1]);
                            args.Player.SendSuccessMessage(_translation.language["SuccessfullyDelete"]);
                            File.WriteAllText(config_path, JsonConvert.SerializeObject(_config, Formatting.Indented));
                        }
                    } else {
                        args.Player.SendErrorMessage(_translation.language["NotEnabled"]);
                    }
                    break;

                case "true":
                    if (_config.enable){
                        args.Player.SendSuccessMessage(_translation.language["SuccessfullyEnable"]);
                    } else {
                        _config.enable = true;
                        args.Player.SendErrorMessage(_translation.language["FailedEnable"]);
                    }
                    break;
                case "false":
                    if (_config.enable == false){
                        args.Player.SendSuccessMessage(_translation.language["FailedDisable"]);
                    } else {
                        _config.enable = false;
                        args.Player.SendSuccessMessage(_translation.language["SuccessfullyDisable"]);
                    }
                    break;
            }
        }

    }
}
