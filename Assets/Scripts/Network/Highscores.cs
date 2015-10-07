using System;
using System.Collections;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Network {
    public class Highscores: MonoBehaviour {
        private const string _ld33Key = "SECRET";
        public Text Username;

        public JSONNode GetTop() {
            var data = Utils.GetRequest("http://45.55.134.136:5000/ld33/highscore");
            if (!data.StartsWith("[Error]")){
                var recs = JSON.Parse(data);
//                Debug.Log(recs[0]["Username"] + ": " + recs[0]["Score"]);
                return recs;
            }
            return "[Error]: Something wrong...";
        }


        public bool SendRecord([NotNull] string name, int score, int mode) {
            var key = _ld33Key;
            var s = name + score + mode + key;
            var tmp = Utils.GetHashString(s);

            var json = String.Format(
                "{{\"Username\": \"{0}\", \"Score\": {1}, \"Mode\": {2}, \"Key\": \"{3}\"}}", name, score, mode, tmp);

            var data = Utils.PostRequest("http://45.55.134.136:5000/ld33/highscore", json);

            if (data.StartsWith("[Error]"))
                return false;
            return true;
        }

        public bool SendStats(string name, int score, int mode, bool is_win, int gold, int playtime) {
            var key = _ld33Key;

            int is_win_int;
            if (is_win) is_win_int = 1;
            else is_win_int = 0;

            var s = name + score + mode + gold + playtime + is_win_int + key;
            var tmp = Utils.GetHashString(s);

            var json = String.Format(
                "{{\"Username\": \"{0}\", \"Score\": {1}, \"Mode\": {2}, \"Gold\": {3}, \"Playtime\": {4}, \"IsWin\": {5}, \"Key\": \"{6}\"}}",
                name, score, mode, gold, playtime, is_win_int, tmp);

            //            var dict = new Dictionary<string, string> { { "Username", name }, { "Score", score.ToString()}, { "Mode", mode.ToString()}, { "Gold", gold.ToString() }, { "Playtime", playtime.ToString()},
            //            { "IsWin", is_win_int.ToString()}, { "Key", tmp} };


            var data = Utils.PostRequest("http://45.55.134.136:5000/ld33/stats", json);

            if (data.StartsWith("[Error]")){
                Debug.Log(data);
                return false;
            }
            return true;
        }
    }
}