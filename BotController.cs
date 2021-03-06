using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RepubliqueBot.Actions;
using RepubliqueBot.Models;
using Microsoft.Extensions.Caching.Memory;

namespace RepubliqueBot
{
    public class BotController : Controller
    {

        private IMemoryCache _cache;

        public BotController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public ActionResult Index()
        {
            var req = Request.Body;
            var json = new StreamReader(req).ReadToEnd();
            Update update = JsonConvert.DeserializeObject<Update>(json);
            IAction action;
            if (update.Message == null) return Content("OK");
            switch (update.Message.Command)
            {
                //TODO: add user concerned by vote here
                case Models.Commands.VoteBan: action = new VoteBanAction(update.Message, update.Message.Param); break;
                case Models.Commands.VoteMute: action = new VoteMuteAction(update.Message, update.Message.Param); break;
                case Models.Commands.VoteNoSticker: action = new VoteNoStickerAction(update.Message, update.Message.Param); break;
                case Models.Commands.VoteRelease: action = new VoteReleaseAction(update.Message, update.Message.Param); break;
                case Models.Commands.SetTitle: action = new SetTitleAction(update.Message); break;
                default: action = new NoneAction(); break;
            }
            action.execute();
            return Content("OK");
        }
    }
}